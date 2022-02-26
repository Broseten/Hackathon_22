using System.Collections.Generic;

namespace LLQ.Speech.Editor
{
	/// <summary>
	/// Common interface for various SSML-based TTS templates.
	/// </summary>
	public interface ITTSTemplateProvider
	{
		IEnumerable<string> GetAllLangs();
		string GetTemplate(string cultureId);
		string TextToSSML(string cultureId, string textContent);
	}
}
