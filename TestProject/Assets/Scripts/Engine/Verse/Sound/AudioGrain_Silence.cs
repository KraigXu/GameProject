using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	
	public class AudioGrain_Silence : AudioGrain
	{
		
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			yield return new ResolvedGrain_Silence(this);
			yield break;
		}

		
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
