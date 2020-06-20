using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004CE RID: 1230
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x0600242D RID: 9261 RVA: 0x000D85E1 File Offset: 0x000D67E1
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			foreach (AudioClip clip in ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath))
			{
				yield return new ResolvedGrain_Clip(clip);
			}
			IEnumerator<AudioClip> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040015D6 RID: 5590
		[LoadAlias("clipPath")]
		[NoTranslate]
		public string clipFolderPath = "";
	}
}
