using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000710 RID: 1808
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		// Token: 0x06002FAD RID: 12205 RVA: 0x0010C88C File Offset: 0x0010AA8C
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || mentalState_InsultingSpree.target == null || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Insult, mentalState_InsultingSpree.target);
		}
	}
}
