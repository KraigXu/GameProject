using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000726 RID: 1830
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x000FAF7C File Offset: 0x000F917C
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x0010E858 File Offset: 0x0010CA58
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x0010E86A File Offset: 0x0010CA6A
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x0010E8A0 File Offset: 0x0010CAA0
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
