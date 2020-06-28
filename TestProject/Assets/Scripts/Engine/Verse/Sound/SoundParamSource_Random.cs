using System;

namespace Verse.Sound
{
	// Token: 0x020004E6 RID: 1254
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000D9022 File Offset: 0x000D7222
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000D9029 File Offset: 0x000D7229
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
