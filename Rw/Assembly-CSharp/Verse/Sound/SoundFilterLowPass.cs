using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D6 RID: 1238
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x06002443 RID: 9283 RVA: 0x000D8BA2 File Offset: 0x000D6DA2
		public override void SetupOn(AudioSource source)
		{
			AudioLowPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioLowPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.lowpassResonanceQ = this.lowpassResonaceQ;
		}

		// Token: 0x040015E9 RID: 5609
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies above this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x040015EA RID: 5610
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float lowpassResonaceQ = 1f;
	}
}
