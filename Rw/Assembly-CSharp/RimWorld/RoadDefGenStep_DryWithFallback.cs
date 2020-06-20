using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A53 RID: 2643
	public class RoadDefGenStep_DryWithFallback : RoadDefGenStep
	{
		// Token: 0x06003E86 RID: 16006 RVA: 0x0014B64D File Offset: 0x0014984D
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, this.fallback);
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x0014B65C File Offset: 0x0014985C
		public static void PlaceWorker(Map map, IntVec3 position, TerrainDef fallback)
		{
			while (map.terrainGrid.TerrainAt(position).driesTo != null)
			{
				map.terrainGrid.SetTerrain(position, map.terrainGrid.TerrainAt(position).driesTo);
			}
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
			if (terrainDef.passability == Traversability.Impassable || terrainDef.IsRiver)
			{
				map.terrainGrid.SetTerrain(position, fallback);
			}
		}

		// Token: 0x0400246B RID: 9323
		public TerrainDef fallback;
	}
}
