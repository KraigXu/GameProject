using System;

namespace Verse.Sound
{
	// Token: 0x020004E1 RID: 1249
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x000D8EBC File Offset: 0x000D70BC
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000D8EC3 File Offset: 0x000D70C3
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.MusicManagerPlay == null)
			{
				return 1f;
			}
			return Find.MusicManagerPlay.subtleAmbienceSoundVolumeMultiplier;
		}
	}
}
