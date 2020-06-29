using System;

namespace Verse.Sound
{
	
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000D8EE4 File Offset: 0x000D70E4
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
