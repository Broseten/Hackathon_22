using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LLQ.Speech
{
	/// <summary>
	/// Text-to-speech synthesizer using MS Azure Cognitive Services.
	/// Depends on the Azure Speech SDK Unity package.
	/// </summary>
	public class AzureTTSSynthesizer : ITTSSynthesizer
	{
		private const string TTSREGION = "westeurope";
		private const string TTSKEY = "PASTE YOUR KEY HERE";

		private SpeechConfig speechConfig;
		private SpeechSynthesizer synthesizer;

		private List<Tuple<float, int>> Visemes = new List<Tuple<float, int>>();

		public AzureTTSSynthesizer()
		{
			speechConfig = SpeechConfig.FromSubscription(TTSKEY, TTSREGION);

			// output format: Riff == with header (to save as file); Raw == no header (to play immediatelly)
			speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);

			synthesizer = new SpeechSynthesizer(speechConfig, null);    //null for AudioConfig disable audio output during synthesis (else you get your audio data after finish sound playback)
			synthesizer.VisemeReceived += Synthesizer_VisemeReceived;
		}

		public void Dispose()
		{
			synthesizer.Dispose();
			synthesizer = null;
		}

		private void Synthesizer_VisemeReceived(object sender, SpeechSynthesisVisemeEventArgs e)
		{
			float t = e.AudioOffset / 10000000.0f;  //time bude v sec				
			Visemes.Add(new Tuple<float, int>(t, (int)e.VisemeId));
		}

		public async System.Threading.Tasks.Task<SynthOutput> Synthesize(string ssmlInput)
		{
			Visemes.Clear();

			//var result = synthesizer.SpeakSsmlAsync(ssmlInput).Result;	//works as BLOCKING call (dirty hack, if want try without async/await)
			var result = await synthesizer.SpeakSsmlAsync(ssmlInput);
			if(result.Reason == ResultReason.SynthesizingAudioCompleted)
			{
				var vsss = Visemes.Select(v => new VisemeItem { Time = v.Item1, VisemeId = v.Item2 }).ToArray();
				return new SynthOutput(vsss, result.AudioData);
			}
			else if(result.Reason == ResultReason.Canceled)
			{
				var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
				Debug.LogError($"TTS SYNTHESIZATION CANCELED:\nReason=[{cancellation.Reason}]\nErrorDetails=[{cancellation.ErrorDetails}]\nDid you update the subscription info?");
			}

			return null;
		}
	}
}
