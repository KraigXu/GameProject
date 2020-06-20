using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D4 RID: 1748
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		// Token: 0x06002EB6 RID: 11958 RVA: 0x0010649C File Offset: 0x0010469C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				return null;
			}
			Thing thing;
			if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 12f, out thing, pawn, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Steal);
				job.targetA = thing;
				job.targetB = c;
				job.count = Mathf.Min(thing.stackCount, (int)(pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / thing.def.VolumePerUnit));
				return job;
			}
			return null;
		}

		// Token: 0x04001A7F RID: 6783
		public const float ItemsSearchRadiusInitial = 7f;

		// Token: 0x04001A80 RID: 6784
		private const float ItemsSearchRadiusOngoing = 12f;
	}
}
