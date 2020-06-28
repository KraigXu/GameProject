using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006B5 RID: 1717
	public class JobGiver_AISapper : ThinkNode_JobGiver
	{
		// Token: 0x06002E5A RID: 11866 RVA: 0x00104583 File Offset: 0x00102783
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AISapper jobGiver_AISapper = (JobGiver_AISapper)base.DeepCopy(resolve);
			jobGiver_AISapper.canMineMineables = this.canMineMineables;
			jobGiver_AISapper.canMineNonMineables = this.canMineNonMineables;
			return jobGiver_AISapper;
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x001045AC File Offset: 0x001027AC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 intVec = (IntVec3)pawn.mindState.duty.focus;
			if (intVec.IsValid && (float)intVec.DistanceToSquared(pawn.Position) < 100f && intVec.GetRoom(pawn.Map, RegionType.Set_Passable) == pawn.GetRoom(RegionType.Set_Passable) && intVec.WithinRegions(pawn.Position, pawn.Map, 9, TraverseMode.NoPassClosedDoors, RegionType.Set_Passable))
			{
				pawn.GetLord().Notify_ReachedDutyLocation(pawn);
				return null;
			}
			if (!intVec.IsValid)
			{
				IAttackTarget attackTarget;
				if (!(from x in pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
				where !x.ThreatDisabled(pawn) && x.Thing.Faction == Faction.OfPlayer && pawn.CanReach(x.Thing, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.PassAllDestroyableThings)
				select x).TryRandomElement(out attackTarget))
				{
					return null;
				}
				intVec = attackTarget.Thing.Position;
			}
			if (!pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.PassAllDestroyableThings))
			{
				return null;
			}
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, intVec, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false), PathEndMode.OnCell))
			{
				IntVec3 cellBeforeBlocker;
				Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
				if (thing != null)
				{
					Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, this.canMineMineables, this.canMineNonMineables);
					if (job != null)
					{
						return job;
					}
				}
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec, 500, true);
		}

		// Token: 0x04001A66 RID: 6758
		private bool canMineMineables = true;

		// Token: 0x04001A67 RID: 6759
		private bool canMineNonMineables = true;

		// Token: 0x04001A68 RID: 6760
		private const float ReachDestDist = 10f;

		// Token: 0x04001A69 RID: 6761
		private const int CheckOverrideInterval = 500;
	}
}
