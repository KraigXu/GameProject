using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D64 RID: 3428
	public class CompProperties_StatOffsetBase : CompProperties
	{
		// Token: 0x06005381 RID: 21377 RVA: 0x001BF060 File Offset: 0x001BD260
		public virtual IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			yield break;
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x001BF06C File Offset: 0x001BD26C
		public virtual float GetMaxOffset(bool forAbstract = false)
		{
			float num = 0f;
			for (int i = 0; i < this.offsets.Count; i++)
			{
				num += this.offsets[i].MaxOffset(forAbstract);
			}
			return num;
		}

		// Token: 0x04002E27 RID: 11815
		public StatDef statDef;

		// Token: 0x04002E28 RID: 11816
		public List<FocusStrengthOffset> offsets = new List<FocusStrengthOffset>();
	}
}
