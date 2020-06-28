using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004D1 RID: 1233
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x06002433 RID: 9267 RVA: 0x000D8644 File Offset: 0x000D6844
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000D865F File Offset: 0x000D685F
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000D8678 File Offset: 0x000D6878
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Clip resolvedGrain_Clip = obj as ResolvedGrain_Clip;
			return resolvedGrain_Clip != null && resolvedGrain_Clip.clip == this.clip;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000D86A7 File Offset: 0x000D68A7
		public override int GetHashCode()
		{
			if (this.clip == null)
			{
				return 0;
			}
			return this.clip.GetHashCode();
		}

		// Token: 0x040015D9 RID: 5593
		public AudioClip clip;
	}
}
