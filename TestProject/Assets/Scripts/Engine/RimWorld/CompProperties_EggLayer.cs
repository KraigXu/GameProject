using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086B RID: 2155
	public class CompProperties_EggLayer : CompProperties
	{
		// Token: 0x0600351D RID: 13597 RVA: 0x00122C50 File Offset: 0x00120E50
		public CompProperties_EggLayer()
		{
			this.compClass = typeof(CompEggLayer);
		}

		// Token: 0x04001C48 RID: 7240
		public float eggLayIntervalDays = 1f;

		// Token: 0x04001C49 RID: 7241
		public IntRange eggCountRange = IntRange.one;

		// Token: 0x04001C4A RID: 7242
		public ThingDef eggUnfertilizedDef;

		// Token: 0x04001C4B RID: 7243
		public ThingDef eggFertilizedDef;

		// Token: 0x04001C4C RID: 7244
		public int eggFertilizationCountMax = 1;

		// Token: 0x04001C4D RID: 7245
		public bool eggLayFemaleOnly = true;

		// Token: 0x04001C4E RID: 7246
		public float eggProgressUnfertilizedMax = 1f;
	}
}
