using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D93 RID: 3475
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x06005497 RID: 21655 RVA: 0x001C3624 File Offset: 0x001C1824
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}

		// Token: 0x04002E7D RID: 11901
		public int goodwillImpact = -200;
	}
}
