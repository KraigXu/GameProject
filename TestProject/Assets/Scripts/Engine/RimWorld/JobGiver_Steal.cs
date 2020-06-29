using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		
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

		
		public const float ItemsSearchRadiusInitial = 7f;

		
		private const float ItemsSearchRadiusOngoing = 12f;
	}
}
