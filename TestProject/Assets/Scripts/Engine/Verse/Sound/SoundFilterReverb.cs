using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D9 RID: 1241
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x06002449 RID: 9289 RVA: 0x000D8C88 File Offset: 0x000D6E88
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}

		// Token: 0x040015F1 RID: 5617
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();
	}
}
