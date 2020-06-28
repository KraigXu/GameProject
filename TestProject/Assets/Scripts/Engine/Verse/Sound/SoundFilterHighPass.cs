using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D7 RID: 1239
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x06002445 RID: 9285 RVA: 0x000D8BDF File Offset: 0x000D6DDF
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}

		// Token: 0x040015EB RID: 5611
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		private float cutoffFrequency = 10000f;

		// Token: 0x040015EC RID: 5612
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		private float highpassResonanceQ = 1f;
	}
}
