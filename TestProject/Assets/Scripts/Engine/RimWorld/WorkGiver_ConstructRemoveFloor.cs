using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x000FAF7C File Offset: 0x000F917C
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
		}

		
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		
		public static bool AnyBuildingBlockingFloorRemoval(IntVec3 c, Map map)
		{
			if (!map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return false;
			}
			Building firstBuilding = c.GetFirstBuilding(map);
			return firstBuilding != null && firstBuilding.def.terrainAffordanceNeeded != null && !map.terrainGrid.UnderTerrainAt(c).affordances.Contains(firstBuilding.def.terrainAffordanceNeeded);
		}
	}
}
