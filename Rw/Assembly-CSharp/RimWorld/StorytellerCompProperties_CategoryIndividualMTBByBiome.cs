using System;

namespace RimWorld
{
	// Token: 0x02000A06 RID: 2566
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x06003D16 RID: 15638 RVA: 0x00143731 File Offset: 0x00141931
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}

		// Token: 0x040023A4 RID: 9124
		public IncidentCategoryDef category;

		// Token: 0x040023A5 RID: 9125
		public bool applyCaravanVisibility;
	}
}
