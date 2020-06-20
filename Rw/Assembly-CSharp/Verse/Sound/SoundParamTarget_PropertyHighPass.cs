using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004EF RID: 1263
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x000D9156 File Offset: 0x000D7356
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x000D916D File Offset: 0x000D736D
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000D917C File Offset: 0x000D737C
		public override void SetOn(Sample sample, float value)
		{
			AudioHighPassFilter audioHighPassFilter = sample.source.GetComponent<AudioHighPassFilter>();
			if (audioHighPassFilter == null)
			{
				audioHighPassFilter = sample.source.gameObject.AddComponent<AudioHighPassFilter>();
			}
			if (this.filterProperty == HighPassFilterProperty.Cutoff)
			{
				audioHighPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == HighPassFilterProperty.Resonance)
			{
				audioHighPassFilter.highpassResonanceQ = value;
			}
		}

		// Token: 0x04001611 RID: 5649
		private HighPassFilterProperty filterProperty;
	}
}
