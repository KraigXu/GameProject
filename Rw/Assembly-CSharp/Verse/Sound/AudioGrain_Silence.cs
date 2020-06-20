using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020004CF RID: 1231
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x0600242F RID: 9263 RVA: 0x000D8604 File Offset: 0x000D6804
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000D8614 File Offset: 0x000D6814
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		// Token: 0x040015D7 RID: 5591
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
