using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000758 RID: 1880
	public class WorkGiver_Repair : WorkGiver_Scanner
	{
		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x001117B5 File Offset: 0x0010F9B5
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x001130F6 File Offset: 0x001112F6
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x0011310E File Offset: 0x0011130E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction).Count == 0;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x00113130 File Offset: 0x00111330
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (!pawn.Map.listerBuildingsRepairable.Contains(pawn.Faction, building))
			{
				return false;
			}
			if (!building.def.building.repairable)
			{
				return false;
			}
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			if (!t.def.useHitPoints || t.HitPoints == t.MaxHitPoints)
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				return false;
			}
			return pawn.CanReserve(building, 1, -1, null, forced) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) == null && (!building.def.mineable || building.Map.designationManager.DesignationAt(building.Position, DesignationDefOf.Mine) == null) && !building.IsBurning();
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x00113241 File Offset: 0x00111441
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Repair, t);
		}
	}
}
