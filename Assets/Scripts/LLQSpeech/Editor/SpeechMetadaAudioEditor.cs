using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LLQ.Speech
{
	[CustomEditor(typeof(SpeechMetadataAudio))]
	public class SpeechMetadaAudioEditor : Editor
	{
		private TTSTemplateProvider templateProvider;
		private string[] allCultureIds;
		private int selectedCultureIdIndex;
		private string cultureId;

		public override void OnInspectorGUI()
		{
			//DrawDefaultInspector();

			SpeechMetadataAudio sma = target as SpeechMetadataAudio;
			if(!sma)
				return;

			var tp = EditorGUILayout.ObjectField(templateProvider, typeof(TTSTemplateProvider), false) as TTSTemplateProvider;
			if(tp != templateProvider)
			{
				if(tp != null)
					allCultureIds = tp.GetAllLangs().ToArray();
				else
					allCultureIds = null;
				templateProvider = tp;
			}

			EditorGUILayout.LabelField("Source string");
			sma.sourceString = EditorGUILayout.TextArea(sma.sourceString, GUILayout.MaxHeight(200));

			if(templateProvider == null)
			{
				GUILayout.Label("Select a TemplateProvider");
				return;
			}

			if(allCultureIds != null)
			{
				selectedCultureIdIndex = EditorGUILayout.Popup(selectedCultureIdIndex, allCultureIds);
				cultureId = allCultureIds[selectedCultureIdIndex];
			}
			else
			{
				selectedCultureIdIndex = 0;
				cultureId = string.Empty;
			}

			var btnLabel = sma.Audio ? "\nRe-synthesize\n" : "\nSynthesize\n";

			if(templateProvider != null)
			{
				if(GUILayout.Button(btnLabel))
				{
					Synthesize(sma);
				}
			}
		}

		private async void Synthesize(SpeechMetadataAudio sma)
		{
			using(var synth = new AzureTTSSynthesizer())
			{
				var smaPath = AssetDatabase.GetAssetPath(sma);
				var outputPath = Path.Combine(Path.GetDirectoryName(smaPath), Path.GetFileNameWithoutExtension(smaPath)) + ".wav";
				Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

				string ssml = templateProvider.TextToSSML(cultureId, sma.sourceString);
				SynthOutput synthOutput = await synth.Synthesize(ssml);

				if(synthOutput != null)
				{
					Debug.Log("Write wav to: " + outputPath);
					File.WriteAllBytes(outputPath, synthOutput.AudioData);

					// modify the import settings
					AssetDatabase.ImportAsset(outputPath); // needed to get the importer
					AudioImporter ai = AssetImporter.GetAtPath(outputPath) as AudioImporter;
					var set = ai.defaultSampleSettings;
					set.loadType = AudioClipLoadType.CompressedInMemory;
					ai.defaultSampleSettings = set;
					AssetDatabase.ImportAsset(outputPath);

					var ac = AssetDatabase.LoadAssetAtPath<AudioClip>(outputPath);
					sma.Audio = ac;
					sma.Visemes = synthOutput.Visemes;

					EditorUtility.SetDirty(sma);
					AssetDatabase.SaveAssets();
				}
			}
		}
	}
}