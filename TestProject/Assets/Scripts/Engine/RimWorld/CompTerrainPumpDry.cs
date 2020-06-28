using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6D RID: 3437
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x060053B8 RID: 21432 RVA: 0x001BF986 File Offset: 0x001BDB86
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x001BF99C File Offset: 0x001BDB9C
		public static void AffectCell(Map map, IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(map);
			TerrainDef terrainToDryTo = CompTerrainPumpDry.GetTerrainToDryTo(map, terrain);
			if (terrainToDryTo != null)
			{
				map.terrainGrid.SetTerrain(c, terrainToDryTo);
			}
			TerrainDef terrainDef = map.terrainGrid.UnderTerrainAt(c);
			if (terrainDef != null)
			{
				TerrainDef terrainToDryTo2 = CompTerrainPumpDry.GetTerrainToDryTo(map, terrainDef);
				if (terrainToDryTo2 != null)
				{
					map.terrainGrid.SetUnderTerrain(c, terrainToDryTo2);
				}
			}
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x001BF9F1 File Offset: 0x001BDBF1
		private static TerrainDef GetTerrainToDryTo(Map map, TerrainDef terrainDef)
		{
			if (terrainDef.driesTo == null)
			{
				return null;
			}
			if (map.Biome == BiomeDefOf.SeaIce)
			{
				return TerrainDefOf.Ice;
			}
			return terrainDef.driesTo;
		}
	}
}
