using System;

namespace Verse.Sound
{
	// Token: 0x020004E2 RID: 1250
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000D8EE4 File Offset: 0x000D70E4
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000D8EEB File Offset: 0x000D70EB
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
