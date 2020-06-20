using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2E RID: 3118
	public class MusicManagerEntry
	{
		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06004A4F RID: 19023 RVA: 0x00191DA5 File Offset: 0x0018FFA5
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06004A50 RID: 19024 RVA: 0x00191DB7 File Offset: 0x0018FFB7
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x00191DC9 File Offset: 0x0018FFC9
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			this.audioSource.volume = this.CurSanitizedVolume;
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x00191E00 File Offset: 0x00190000
		private void StartPlaying()
		{
			if (this.audioSource != null && !this.audioSource.isPlaying)
			{
				this.audioSource.Play();
				return;
			}
			if (GameObject.Find("MusicAudioSourceDummy") != null)
			{
				Log.Error("MusicManagerEntry did StartPlaying but there is already a music source GameObject.", false);
				return;
			}
			this.audioSource = new GameObject("MusicAudioSourceDummy")
			{
				transform = 
				{
					parent = Camera.main.transform
				}
			}.AddComponent<AudioSource>();
			this.audioSource.bypassEffects = true;
			this.audioSource.bypassListenerEffects = true;
			this.audioSource.bypassReverbZones = true;
			this.audioSource.priority = 0;
			this.audioSource.clip = SongDefOf.EntrySong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.loop = true;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
		}

		// Token: 0x04002A29 RID: 10793
		private AudioSource audioSource;

		// Token: 0x04002A2A RID: 10794
		private const string SourceGameObjectName = "MusicAudioSourceDummy";
	}
}
