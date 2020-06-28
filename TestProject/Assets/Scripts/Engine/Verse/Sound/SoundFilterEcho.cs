using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D8 RID: 1240
	public class SoundFilterEcho : SoundFilter
	{
		// Token: 0x06002447 RID: 9287 RVA: 0x000D8C1C File Offset: 0x000D6E1C
		public override void SetupOn(AudioSource source)
		{
			AudioEchoFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioEchoFilter>(source);
			orMakeFilterOn.delay = this.delay;
			orMakeFilterOn.decayRatio = this.decayRatio;
			orMakeFilterOn.wetMix = this.wetMix;
			orMakeFilterOn.dryMix = this.dryMix;
		}

		// Token: 0x040015ED RID: 5613
		[EditSliderRange(10f, 5000f)]
		[Description("Echo delay in ms. 10 to 5000. Default = 500.")]
		private float delay = 500f;

		// Token: 0x040015EE RID: 5614
		[EditSliderRange(0f, 1f)]
		[Description("Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (ie simple 1 line delay).")]
		private float decayRatio = 0.5f;

		// Token: 0x040015EF RID: 5615
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the echo signal to pass to output.")]
		private float wetMix = 1f;

		// Token: 0x040015F0 RID: 5616
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the original signal to pass to output.")]
		private float dryMix = 1f;
	}
}
