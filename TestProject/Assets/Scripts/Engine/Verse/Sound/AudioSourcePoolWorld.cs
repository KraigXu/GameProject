using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000507 RID: 1287
	public class AudioSourcePoolWorld
	{
		// Token: 0x060024EA RID: 9450 RVA: 0x000DAF70 File Offset: 0x000D9170
		public AudioSourcePoolWorld()
		{
			GameObject gameObject = new GameObject("OneShotSourcesWorldContainer");
			gameObject.transform.position = Vector3.zero;
			for (int i = 0; i < 32; i++)
			{
				GameObject gameObject2 = new GameObject("OneShotSource_" + i.ToString());
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				this.sourcesWorld.Add(AudioSourceMaker.NewAudioSourceOn(gameObject2));
			}
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000DB000 File Offset: 0x000D9200
		public AudioSource GetSourceWorld()
		{
			foreach (AudioSource audioSource in this.sourcesWorld)
			{
				if (!audioSource.isPlaying)
				{
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x04001665 RID: 5733
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x04001666 RID: 5734
		private const int NumSourcesWorld = 32;
	}
}
