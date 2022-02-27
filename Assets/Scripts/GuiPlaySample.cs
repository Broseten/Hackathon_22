using LLQ.Speech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiPlaySample : MonoBehaviour
{
	[SerializeField] private Person Person;
	[SerializeField] private SpeechMetadataAudio LipSyncData;

	public void Click_PlaySample()
	{
		Person.GoToState("mluvim");
		Person.PlaySound(LipSyncData);
	}

	public void Click_StopSample()
	{
		Person.GoToState("stojim");
		Person.StopSound();
	}
}
