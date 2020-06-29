using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	
	public class AudioGrain_Folder : AudioGrain
	{
		
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

		
		[LoadAlias("clipPath")]
		[NoTranslate]
		public string clipFolderPath = "";
	}
}
