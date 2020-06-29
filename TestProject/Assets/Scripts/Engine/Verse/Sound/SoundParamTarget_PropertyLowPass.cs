using System;
using UnityEngine;

namespace Verse.Sound
{
	
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x000D90E0 File Offset: 0x000D72E0
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty;
			}
		}

		
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x000D90F7 File Offset: 0x000D72F7
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		
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

		
		private LowPassFilterProperty filterProperty;
	}
}
