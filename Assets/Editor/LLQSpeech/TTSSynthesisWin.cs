using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LLQ.Speech.Editor
{
	/// <summary>
	/// Editor window for in-editor speech audio assets generation.
	/// </summary>
	public class TTSSynthesisWin : EditorWindow
	{
		private string outputPath;
		private TTSTemplateProvider templateProvider;
		private string[] allCultureIds;
		private int selectedCultureIdIndex;
		private string cultureId;
		private string textContent;
		private Vector2 scroll;

		private GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);

		private static string lastOutputPath = "Assets/Audio/speech.wav"; // default or last selected path
		private static GUIContent titleGuiCont = new GUIContent("TTS");

		[MenuItem("LLQ/Speech Synthesis...")]
		private static TTSSynthesisWin ShowWindow()
		{
			var win = GetWindow<TTSSynthesisWin>();
			win.titleContent = titleGuiCont;
			return win;
		}

		private void CreateGUI()
		{
			outputPath = lastOutputPath;
			textAreaStyle.wordWrap = true;
			textAreaStyle.stretchHeight = true;
		}

		private void OnDestroy()
		{
			if(!string.IsNullOrEmpty(outputPath))
				lastOutputPath = outputPath;
		}

		private void OnGUI()
		{
			GUILayout.Label(outputPath);
			if(GUILayout.Button("Select output path"))
			{
				outputPath = EditorUtility.SaveFilePanelInProject("Select output path", "speech", "wav", "Select output path", outputPath);
			}

			var tp = EditorGUILayout.ObjectField(templateProvider, typeof(TTSTemplateProvider), false) as TTSTemplateProvider;
			if(tp != templateProvider)
			{
				if(tp != null)
					allCultureIds = tp.GetAllLangs().ToArray();
				else
					allCultureIds = null;
				templateProvider = tp;
			}

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

			scroll = EditorGUILayout.BeginScrollView(scroll);
			textContent = EditorGUILayout.TextArea(textContent, textAreaStyle);
			EditorGUILayout.EndScrollView();

			if(!string.IsNullOrEmpty(outputPath) && templateProvider != null)
			{
				if(GUILayout.Button("\nSynthesize\n"))
				{
					Synthesize();
				}
			}
		}

		private async void Synthesize()
		{
			using(var synth = new AzureTTSSynthesizer())
			{
				Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

				string ssml = templateProvider.TextToSSML(cultureId, textContent);
				byte[] audioOutput = await synth.Synthesize(ssml);

				if(audioOutput != null)
				{
					Debug.Log("Write wav to: " + outputPath);
					File.WriteAllBytes(outputPath, audioOutput);

					// modify the import settings
					AssetDatabase.ImportAsset(outputPath); // needed to get the importer
					AudioImporter ai = AssetImporter.GetAtPath(outputPath) as AudioImporter;
					var set = ai.defaultSampleSettings;
					set.loadType = AudioClipLoadType.CompressedInMemory;
					ai.defaultSampleSettings = set;
					AssetDatabase.ImportAsset(outputPath);
				}
			}
		}
	}
}
