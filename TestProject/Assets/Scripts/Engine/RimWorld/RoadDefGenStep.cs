using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F9 RID: 2297
	public abstract class RoadDefGenStep
	{
		// Token: 0x060036D7 RID: 14039
		public abstract void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance);

		// Token: 0x04001F83 RID: 8067
		public SimpleCurve chancePerPositionCurve;

		// Token: 0x04001F84 RID: 8068
		public float antialiasingMultiplier = 1f;

		// Token: 0x04001F85 RID: 8069
		public int periodicSpacing;
	}
}
