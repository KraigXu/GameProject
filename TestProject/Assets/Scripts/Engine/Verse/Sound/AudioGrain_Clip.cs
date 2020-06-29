using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	
	public class AudioGrain_Clip : AudioGrain
	{
		
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioClip audioClip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			if (audioClip != null)
			{
				yield return new ResolvedGrain_Clip(audioClip);
			}
			else
			{
				Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath, false);
			}
			yield break;
		}

		
		[NoTranslate]
		public string clipPath = "";
	}
}
