using System;

namespace LLQ.Speech
{
	/// <summary>
	/// Common interface for speech synthesizers based various text-to-speech service providers.
	/// </summary>
	public interface ITTSSynthesizer : IDisposable
	{
		/// <summary>
		/// Generate speech sound data from a text input.
		/// </summary>
		/// <param name="ssmlInput">Synthesis input containing all necessary meta-information/tags in Speech Synthesis Markup Language (SSML) format.</param>
		/// <returns>Generated sound data, that can be saved as a file and played back.</returns>
		System.Threading.Tasks.Task<byte[]> Synthesize(string ssmlInput);
	}
}
