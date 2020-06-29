using System;

namespace Verse.Sound
{
	
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x000D8E38 File Offset: 0x000D7038
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		
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
