using System;

namespace Verse.Sound
{
	// Token: 0x020004DE RID: 1246
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x000D8E38 File Offset: 0x000D7038
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000D8E3F File Offset: 0x000D703F
		public override float ValueFor(Sample samp)
		{
			if (samp.ExternalParams.sizeAggregator == null)
			{
				return 0f;
			}
			return samp.ExternalParams.sizeAggregator.AggregateSize;
		}
	}
}
