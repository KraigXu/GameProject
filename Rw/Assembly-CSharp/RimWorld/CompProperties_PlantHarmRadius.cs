using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D36 RID: 3382
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		// Token: 0x06005229 RID: 21033 RVA: 0x001B7460 File Offset: 0x001B5660
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}

		// Token: 0x04002D53 RID: 11603
		public float harmFrequencyPerArea = 0.011f;

		// Token: 0x04002D54 RID: 11604
		public float leaflessPlantKillChance = 0.05f;

		// Token: 0x04002D55 RID: 11605
		public SimpleCurve radiusPerDayCurve;
	}
}
