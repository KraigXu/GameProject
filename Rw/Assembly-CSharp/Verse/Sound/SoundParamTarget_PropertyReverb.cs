using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004F2 RID: 1266
	public class SoundParamTarget_PropertyReverb : SoundParamTarget
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002483 RID: 9347 RVA: 0x000D9266 File Offset: 0x000D7466
		public override string Label
		{
			get
			{
				return "ReverbFilter-interpolant";
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x000D926D File Offset: 0x000D746D
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterReverb);
			}
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000D927C File Offset: 0x000D747C
		public override void SetOn(Sample sample, float value)
		{
			AudioReverbFilter audioReverbFilter = sample.source.GetComponent<AudioReverbFilter>();
			if (audioReverbFilter == null)
			{
				audioReverbFilter = sample.source.gameObject.AddComponent<AudioReverbFilter>();
			}
			ReverbSetup reverbSetup;
			if (value < 0.001f)
			{
				reverbSetup = this.baseSetup;
			}
			if (value > 0.999f)
			{
				reverbSetup = this.targetSetup;
			}
			else
			{
				reverbSetup = ReverbSetup.Lerp(this.baseSetup, this.targetSetup, value);
			}
			reverbSetup.ApplyTo(audioReverbFilter);
		}

		// Token: 0x04001618 RID: 5656
		[Description("The base setup for the reverb.\n\nOnly used if no parameters are touching this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		// Token: 0x04001619 RID: 5657
		[Description("The interpolation target setup for this filter.\n\nWhen the interpolant parameter is at 1, these settings will be active.")]
		private ReverbSetup targetSetup = new ReverbSetup();
	}
}
