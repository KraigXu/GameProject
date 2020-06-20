using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0A RID: 3594
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x060056D4 RID: 22228 RVA: 0x001CCB03 File Offset: 0x001CAD03
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}

		// Token: 0x060056D5 RID: 22229 RVA: 0x001CCB0A File Offset: 0x001CAD0A
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}
	}
}
