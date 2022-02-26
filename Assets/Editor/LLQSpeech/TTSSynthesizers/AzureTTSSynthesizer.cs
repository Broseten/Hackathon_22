using Microsoft.CognitiveServices.Speech;
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

		public AzureTTSSynthesizer()
		{
			speechConfig = SpeechConfig.FromSubscription(TTSKEY, TTSREGION);

			// output format: Riff == with header (to save as file); Raw == no header (to play immediatelly)
			speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);

			synthesizer = new SpeechSynthesizer(speechConfig, null);	//null for AudioConfig disable audio output during synthesis (else you get your audio data after finish sound playback)
		}

		public void Dispose()
		{
			synthesizer.Dispose();
			synthesizer = null;
		}

		public async System.Threading.Tasks.Task<byte[]> Synthesize(string ssmlInput)
		{
			//var result = synthesizer.SpeakSsmlAsync(ssmlInput).Result;	//works as BLOCKING call (dirty hack, if want try without async/await)
			var result = await synthesizer.SpeakSsmlAsync(ssmlInput);
			if(result.Reason == ResultReason.SynthesizingAudioCompleted)
			{
				return result.AudioData;
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
