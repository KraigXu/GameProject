using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A08 RID: 2568
	public class StorytellerCompProperties_CategoryMTB : StorytellerCompProperties
	{
		// Token: 0x06003D1C RID: 15644 RVA: 0x0014378A File Offset: 0x0014198A
		public StorytellerCompProperties_CategoryMTB()
		{
			this.compClass = typeof(StorytellerComp_CategoryMTB);
		}

		// Token: 0x040023A6 RID: 9126
		public float mtbDays = -1f;

		// Token: 0x040023A7 RID: 9127
		public SimpleCurve mtbDaysFactorByDaysPassedCurve;

		// Token: 0x040023A8 RID: 9128
		public IncidentCategoryDef category;
	}
}
