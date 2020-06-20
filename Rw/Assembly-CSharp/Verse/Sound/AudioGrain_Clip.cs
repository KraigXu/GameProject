using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004CD RID: 1229
	public class AudioGrain_Clip : AudioGrain
	{
		// Token: 0x0600242B RID: 9259 RVA: 0x000D85BE File Offset: 0x000D67BE
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

		// Token: 0x040015D5 RID: 5589
		[NoTranslate]
		public string clipPath = "";
	}
}
