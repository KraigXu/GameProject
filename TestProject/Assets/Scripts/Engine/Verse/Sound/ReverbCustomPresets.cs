using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D3 RID: 1235
	public static class ReverbCustomPresets
	{
		// Token: 0x0600243B RID: 9275 RVA: 0x000D8728 File Offset: 0x000D6928
		public static void SetupAs(this ReverbSetup setup, AudioReverbPreset preset)
		{
			GameObject gameObject = new GameObject("OneFrameTemp");
			gameObject.AddComponent<AudioSource>();
			AudioReverbFilter audioReverbFilter = gameObject.AddComponent<AudioReverbFilter>();
			audioReverbFilter.reverbPreset = preset;
			setup.dryLevel = audioReverbFilter.dryLevel;
			setup.room = audioReverbFilter.room;
			setup.roomHF = audioReverbFilter.roomHF;
			setup.roomLF = audioReverbFilter.roomLF;
			setup.decayTime = audioReverbFilter.decayTime;
			setup.decayHFRatio = audioReverbFilter.decayHFRatio;
			setup.reflectionsLevel = audioReverbFilter.reflectionsLevel;
			setup.reflectionsDelay = audioReverbFilter.reflectionsDelay;
			setup.reverbLevel = audioReverbFilter.reverbLevel;
			setup.reverbDelay = audioReverbFilter.reverbDelay;
			setup.hfReference = audioReverbFilter.hfReference;
			setup.lfReference = audioReverbFilter.lfReference;
			setup.diffusion = audioReverbFilter.diffusion;
			setup.density = audioReverbFilter.density;
			UnityEngine.Object.Destroy(audioReverbFilter);
		}
	}
}
