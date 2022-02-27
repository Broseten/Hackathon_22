using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLQ.Speech
{
	public class Person : MonoBehaviour
	{
		public Animator Animator;
		public AudioSource AudioSource;

		string WantState;// protoze unity neumi nastavit stav na animator disablovaneho objektu
		float WantOffset;

		private float LipSyncTime;
		private SpeechMetadataAudio LipSyncMetadata;

		public Vector4 LookWeights;
		public Camera Camera;

		void OnEnable()
		{
			if(Animator == null)
				Animator = GetComponent<Animator>();

			Animator.keepAnimatorControllerStateOnDisable = true;

			if(WantState != null)
				Animator.Play(WantState, -1, WantOffset);

			WantState = null;
		}

		internal void PlaySound(SpeechMetadataAudio lipSyncData)
		{
			if(AudioSource != null)
				AudioSource.PlayOneShot(lipSyncData.Audio);
			LipSyncMetadata = lipSyncData;
			LipSyncTime = 0;
		}

		internal void StopSound()
		{
			if(AudioSource != null)
				AudioSource.Stop();

			LipSyncMetadata = null;
			SetLipsync(0, 0, 0);
		}

		/// <summary>
		/// prepne animator do zadaneho stavu
		/// </summary>
		/// <param name="stateName">jmeno stavu</param>
		/// <param name="transition">doba blendu v [sec]</param>
		/// <param name="offset">relativni cas v cilove animaci</param>
		public void GoToState(string stateName, float transition = 1, float offset = 0)
		{
			if(Animator == null)
				Animator = GetComponent<Animator>();
			if(Animator == null)
				return;

			if(gameObject.activeInHierarchy)
			{
				WantState = null;
				if(transition > 0)
				{
					var ct = Animator.GetCurrentAnimatorStateInfo(0).length;
					var td = transition / ct;
					Animator.CrossFade(stateName, td, -1, offset);
				}
				else
				{
					Animator.Play(stateName, -1, offset);
				}
			}
			else
			{
				WantState = stateName;
				WantOffset = offset;
			}
		}

		private void Update()
		{
			if(LipSyncMetadata != null)
			{
				var t = LipSyncTime;
				LipSyncTime += Time.deltaTime;

				LipSyncMetadata.GetVisemeForTime(t, out int a, out int b, out float lt);
				SetLipsync(a, b, lt);
			}
		}

		internal void SetLipsync(int a, int b, float t)
		{
			float fa = a / 21f; //HACK - fuj, cas je potreba zadavat normalizovany (0-1) a animace ma delku 21sec
			float fb = b / 21f;
			float ft = (t > .5) ? (1 - ((1 - t) * (1 - t))) : t * t;    //ease in-out, at jsou mekci pohyby
			Animator.SetFloat("lip_a", fa);
			Animator.SetFloat("lip_b", fb);
			Animator.SetLayerWeight(1, 1 - ft);
			Animator.SetLayerWeight(2, ft);
		}
		private void OnAnimatorIK()
		{
			
			//if(LookTarget != null)
			{
			var target = Camera.transform.position;
			/*var da = (transform.position - target).Magnitude2D();

			if(da < LookDistance)
			Weight += Time.deltaTime * 2;
			else
			Weight -= Time.deltaTime;

			Weight = Mathf.Clamp01(Weight);

			if(Weight > .9f)
		
			var bpos = LLQStory.Managers.CameraRigManager.Instance.Controllers.rightBaseController.transform.position;
			var db = (transform.position - bpos).Magnitude2D();
			if(db < da * HandRatio)
			target = bpos;
			}*/

			//var tpos = LLQStory.Managers.CameraRigManager.Instance.Controllers.rightBaseController.transform.position;
			Animator.SetLookAtWeight(1, LookWeights.x, LookWeights.y, LookWeights.z, LookWeights.w);
			Animator.SetLookAtPosition(target);
			//Animator.SetLookAtPosition(LLQStory.Managers.CameraRigManager.Instance.Body.GetHead().transform.position);
			//LLQStory.Managers.CameraRigManager.Instance.Body.GetHead().transform.position;
			}
			
		}
	}
}
