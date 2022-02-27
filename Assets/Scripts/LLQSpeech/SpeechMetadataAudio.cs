using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LLQ.Speech
{
	[Serializable]
	public class VisemeItem
	{
		public float Time;
		public int VisemeId;
	}

	[SerializeField]
	public class SynthOutput
	{
		public VisemeItem[] Visemes;
		public byte[] AudioData;

		public SynthOutput(VisemeItem[] visemes, byte[] audioData)
		{
			Visemes = visemes;
			AudioData = audioData;
		}
	}

	[CreateAssetMenu(menuName = "LLQ/Speech Metadata Audio")]
	public class SpeechMetadataAudio : ScriptableObject
	{
		// input (kept to allow interative changes)
		public string sourceString;

		// output
		public AudioClip Audio;
		public VisemeItem[] Visemes;

		public void GetVisemeForTime(float time, out int a, out int b, out float t)
		{
			//neefektivni zpusob jak najit elementy sousedni k nejakemu casu
			//prvni optimalizace by byla pamatovat si predchozi snimek a hledat znovu jen pri zmene
			var v = Visemes?
				.SkipWhile(x => x.Time < time)
				.Take(2).ToArray();
			if(v != null && v.Length == 2)
			{
				a = v[0].VisemeId;
				b = v[1].VisemeId;
				t = Mathf.InverseLerp(v[0].Time, v[1].Time, time);
			}
			else
			{
				a = 0;
				b = 0;
				t = 0;
			}
		}
	}
}