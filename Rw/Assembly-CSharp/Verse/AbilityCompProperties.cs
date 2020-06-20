using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000046 RID: 70
	public class AbilityCompProperties
	{
		// Token: 0x06000384 RID: 900 RVA: 0x00012958 File Offset: 0x00010B58
		public virtual IEnumerable<string> ConfigErrors(AbilityDef parentDef)
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

		// Token: 0x040000F9 RID: 249
		[TranslationHandle]
		public Type compClass;
	}
}
