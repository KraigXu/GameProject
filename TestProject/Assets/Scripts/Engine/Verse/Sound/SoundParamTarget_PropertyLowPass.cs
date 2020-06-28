using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004ED RID: 1261
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x000D90E0 File Offset: 0x000D72E0
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x000D90F7 File Offset: 0x000D72F7
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000D9104 File Offset: 0x000D7304
		public override void SetOn(Sample sample, float value)
		{
			AudioLowPassFilter audioLowPassFilter = sample.source.GetComponent<AudioLowPassFilter>();
			if (audioLowPassFilter == null)
			{
				audioLowPassFilter = sample.source.gameObject.AddComponent<AudioLowPassFilter>();
			}
			if (this.filterProperty == LowPassFilterProperty.Cutoff)
			{
				audioLowPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == LowPassFilterProperty.Resonance)
			{
				audioLowPassFilter.lowpassResonanceQ = value;
			}
		}

		// Token: 0x0400160D RID: 5645
		private LowPassFilterProperty filterProperty;
	}
}
