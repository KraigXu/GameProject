using System;

namespace Verse.Sound
{
	
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x000D8EBC File Offset: 0x000D70BC
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		
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
