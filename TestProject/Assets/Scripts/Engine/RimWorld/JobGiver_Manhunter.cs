using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000711 RID: 1809
	public class JobGiver_Manhunter : ThinkNode_JobGiver
	{
		// Token: 0x06002FAF RID: 12207 RVA: 0x0010C8E0 File Offset: 0x0010AAE0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, false) == null)
			{
				return null;
			}
			Pawn pawn2 = this.FindPawnTarget(pawn);
			if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return this.MeleeAttackJob(pawn, pawn2);
			}
			Building building = this.FindTurretTarget(pawn);
			if (building != null)
			{
				return this.MeleeAttackJob(pawn, building);
			}
			if (pawn2 != null)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
				{
					if (!pawnPath.Found)
					{
						return null;
					}
					IntVec3 loc;
					if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
					{
						Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap, false);
						return null;
					}
					IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).RandomCell;
					if (randomCell == pawn.Position)
					{
						return JobMaker.MakeJob(JobDefOf.Wait, 30, false);
					}
					return JobMaker.MakeJob(JobDefOf.Goto, randomCell);
				}
			}
			return null;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x0010CA18 File Offset: 0x0010AC18
		private Job MeleeAttackJob(Pawn pawn, Thing target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = true;
			return job;
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x0010CA50 File Offset: 0x0010AC50
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, true, true);
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x0010CAA8 File Offset: 0x0010ACA8
		private Building FindTurretTarget(Pawn pawn)
		{
			return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing t) => t is Building, 0f, 70f, default(IntVec3), float.MaxValue, false, true);
		}

		// Token: 0x04001AD2 RID: 6866
		private const float WaitChance = 0.75f;

		// Token: 0x04001AD3 RID: 6867
		private const int WaitTicks = 90;

		// Token: 0x04001AD4 RID: 6868
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x04001AD5 RID: 6869
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x04001AD6 RID: 6870
		private const int WanderOutsideDoorRegions = 9;
	}
}
