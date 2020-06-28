using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5C RID: 3420
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x0600534D RID: 21325 RVA: 0x001BDFB4 File Offset: 0x001BC1B4
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}

		// Token: 0x04002DFC RID: 11772
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x04002DFD RID: 11773
		public float HiveSpawnRadius = 10f;

		// Token: 0x04002DFE RID: 11774
		public FloatRange HiveSpawnIntervalDays = new FloatRange(2f, 3f);

		// Token: 0x04002DFF RID: 11775
		public SimpleCurve ReproduceRateFactorFromNearbyHiveCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(7f, 0.35f),
				true
			}
		};
	}
}
