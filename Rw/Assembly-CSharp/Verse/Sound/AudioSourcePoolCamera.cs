using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000506 RID: 1286
	public class AudioSourcePoolCamera
	{
		// Token: 0x060024E8 RID: 9448 RVA: 0x000DAE68 File Offset: 0x000D9068
		public AudioSourcePoolCamera()
		{
			this.cameraSourcesContainer = new GameObject("OneShotSourcesCameraContainer");
			this.cameraSourcesContainer.transform.parent = Find.Camera.transform;
			this.cameraSourcesContainer.transform.localPosition = Vector3.zero;
			for (int i = 0; i < 16; i++)
			{
				AudioSource audioSource = AudioSourceMaker.NewAudioSourceOn(new GameObject("OneShotSourceCamera_" + i.ToString())
				{
					transform = 
					{
						parent = this.cameraSourcesContainer.transform,
						localPosition = Vector3.zero
					}
				});
				audioSource.bypassReverbZones = true;
				this.sourcesCamera.Add(audioSource);
			}
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000DAF28 File Offset: 0x000D9128
		public AudioSource GetSourceCamera()
		{
			for (int i = 0; i < this.sourcesCamera.Count; i++)
			{
				AudioSource audioSource = this.sourcesCamera[i];
				if (!audioSource.isPlaying)
				{
					audioSource.clip = null;
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x04001662 RID: 5730
		public GameObject cameraSourcesContainer;

		// Token: 0x04001663 RID: 5731
		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		// Token: 0x04001664 RID: 5732
		private const int NumSourcesCamera = 16;
	}
}
