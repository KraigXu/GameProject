using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DE RID: 1758
	public class JobGiver_Haul : ThinkNode_JobGiver
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x0010749C File Offset: 0x0010569C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				IntVec3 intVec;
				return !t.IsForbidden(pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, false) && pawn.carryTracker.MaxStackSpaceEver(t.def) > 0 && StoreUtility.TryFindBestBetterStoreCellFor(t, pawn, pawn.Map, StoreUtility.CurrentStoragePriorityOf(t), pawn.Faction, out intVec, true);
			};
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling(), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
			if (thing != null)
			{
				return HaulAIUtility.HaulToStorageJob(pawn, thing);
			}
			return null;
		}
	}
}
