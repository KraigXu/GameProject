using System;
using Verse;

namespace RimWorld
{
	
	public class Alert_TatteredApparel : Alert_Thought
	{
		
		// (get) Token: 0x060056D4 RID: 22228 RVA: 0x001CCB03 File Offset: 0x001CAD03
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}

		
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}
	}
}
