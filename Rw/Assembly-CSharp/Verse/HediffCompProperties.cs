using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000092 RID: 146
	public class HediffCompProperties
	{
		// Token: 0x060004DD RID: 1245 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostLoad()
		{
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00018BD9 File Offset: 0x00016DD9
		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			int num;
			for (int i = 0; i < parentDef.comps.Count; i = num + 1)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					yield return "two comps with same compClass: " + this.compClass;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04000276 RID: 630
		[TranslationHandle]
		public Type compClass;
	}
}
