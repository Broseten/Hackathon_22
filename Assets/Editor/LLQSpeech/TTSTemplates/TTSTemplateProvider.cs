using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace LLQ.Speech.Editor
{
	/// <summary>
	/// Speech synthesis preset in the form of Speech Synthesis Markup Language (SSML) template.
	/// Can contain multiple language versions to represent a single virtual character in various localizations.
	/// </summary>
	[CreateAssetMenu(menuName = "LLQ/TTS Template")]
	public class TTSTemplateProvider : ScriptableObject, ITTSTemplateProvider
	{
		[Serializable]
		private class TemplateItem
		{
			public string cultureId;
			[TextArea(15, 99)]
			public string templateText; // SSML template; should contain the string "TEXT" as a placeholder that will be replaced by the actual content to be synthesized
		}

		[SerializeField]
		private TemplateItem[] langTemplates; // Dictionary would be better, but that would need a custom Editor/Inspector...

		public IEnumerable<string> GetAllLangs()
		{
			return langTemplates.Select(t => t.cultureId);
		}

		public string GetTemplate(string cultureId)
		{
			if(langTemplates == null || langTemplates.Length == 0)
			{
				Debug.LogError($"No TTS templates provided! Fill in the template {name}.");
				return null;
			}

			var template = langTemplates.FirstOrDefault(t => string.Equals(t.cultureId, cultureId, StringComparison.OrdinalIgnoreCase));
			if(template == null)
			{
				Debug.LogWarning($"No TTS template for {cultureId}; {langTemplates[0].cultureId} will be used instead.");
				template = langTemplates[0]; // take the first as default
			}

			return template.templateText;
		}

		public string TextToSSML(string cultureId, string textContent)
		{
			var ssmlTemplate = GetTemplate(cultureId);

			if(ssmlTemplate == null)
				return textContent;

			StringBuilder escaped = new StringBuilder();
			using(var writer = XmlWriter.Create(escaped, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment }))
			{
				writer.WriteString(textContent);
			}

			return ssmlTemplate.Replace("TEXT", escaped.ToString());
		}
	}
}