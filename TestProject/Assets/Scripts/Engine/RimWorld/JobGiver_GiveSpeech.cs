using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C5 RID: 1733
	public class JobGiver_GiveSpeech : ThinkNode_JobGiver
	{
		// Token: 0x06002E8E RID: 11918 RVA: 0x00105B84 File Offset: 0x00103D84
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Building_Throne building_Throne = duty.focusSecond.Thing as Building_Throne;
			if (building_Throne == null || building_Throne.AssignedPawn != pawn)
			{
				return null;
			}
			if (!pawn.CanReach(building_Throne, PathEndMode.InteractionCell, Danger.None, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GiveSpeech, duty.focusSecond);
		}
	}
}
