using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000504 RID: 1284
	public static class AudioSourceMaker
	{
		// Token: 0x060024E5 RID: 9445 RVA: 0x000DADD4 File Offset: 0x000D8FD4
		public static AudioSource NewAudioSourceOn(GameObject go)
		{
			if (go.GetComponent<AudioSource>() != null)
			{
				Log.Warning("Adding audio source on " + go + " that already has one.", false);
				return go.GetComponent<AudioSource>();
			}
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.dopplerLevel = 0f;
			audioSource.playOnAwake = false;
			return audioSource;
		}

		// Token: 0x0400165F RID: 5727
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;
	}
}
