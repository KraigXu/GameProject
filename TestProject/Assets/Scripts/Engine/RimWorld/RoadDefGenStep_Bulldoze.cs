using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A51 RID: 2641
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		// Token: 0x06003E82 RID: 16002 RVA: 0x0014B298 File Offset: 0x00149498
		public override void Place(Map map, IntVec3 tile, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			while (tile.Impassable(map))
			{
				foreach (Thing thing in tile.GetThingList(map))
				{
					if (thing.def.passability == Traversability.Impassable)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
				}
			}
		}
	}
}
