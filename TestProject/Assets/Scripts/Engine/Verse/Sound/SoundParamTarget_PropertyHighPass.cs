using System;
using UnityEngine;

namespace Verse.Sound
{
	
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x000D9156 File Offset: 0x000D7356
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty;
			}
		}

		
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x000D916D File Offset: 0x000D736D
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		
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

		
		private HighPassFilterProperty filterProperty;
	}
}
