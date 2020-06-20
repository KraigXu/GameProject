using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006BC RID: 1724
	public class JobGiver_PrepareCaravan_GatherItems : ThinkNode_JobGiver
	{
		// Token: 0x06002E74 RID: 11892 RVA: 0x00105288 File Offset: 0x00103488
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lord);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherItems, thing);
			job.lord = lord;
			return job;
		}
	}
}
