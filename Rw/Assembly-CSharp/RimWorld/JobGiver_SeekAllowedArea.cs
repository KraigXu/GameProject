using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006EB RID: 1771
	public class JobGiver_SeekAllowedArea : ThinkNode_JobGiver
	{
		// Token: 0x06002F03 RID: 12035 RVA: 0x001088EC File Offset: 0x00106AEC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Position.IsForbidden(pawn))
			{
				return null;
			}
			if (this.HasJobWithSpawnedAllowedTarget(pawn))
			{
				return null;
			}
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return null;
			}
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region reg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				if (!r.IsForbiddenEntirely(pawn))
				{
					reg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
			if (reg == null)
			{
				return null;
			}
			IntVec3 c;
			if (!reg.TryFindRandomCellInRegionUnforbidden(pawn, null, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, c);
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x001089B8 File Offset: 0x00106BB8
		private bool HasJobWithSpawnedAllowedTarget(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && (this.IsSpawnedAllowedTarget(curJob.targetA, pawn) || this.IsSpawnedAllowedTarget(curJob.targetB, pawn) || this.IsSpawnedAllowedTarget(curJob.targetC, pawn));
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x00108A00 File Offset: 0x00106C00
		private bool IsSpawnedAllowedTarget(LocalTargetInfo target, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			if (target.HasThing)
			{
				return target.Thing.Spawned && !target.Thing.Position.IsForbidden(pawn);
			}
			return target.Cell.InBounds(pawn.Map) && !target.Cell.IsForbidden(pawn);
		}
	}
}
