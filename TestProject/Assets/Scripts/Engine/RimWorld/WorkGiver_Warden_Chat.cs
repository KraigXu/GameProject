using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000732 RID: 1842
	public class WorkGiver_Warden_Chat : WorkGiver_Warden
	{
		// Token: 0x06003064 RID: 12388 RVA: 0x0010F6E8 File Offset: 0x0010D8E8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			PrisonerInteractionModeDef interactionMode = pawn2.guest.interactionMode;
			if ((interactionMode != PrisonerInteractionModeDefOf.AttemptRecruit && interactionMode != PrisonerInteractionModeDefOf.ReduceResistance) || !pawn2.guest.ScheduledForInteraction || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) || (pawn2.Downed && !pawn2.InBed()) || !pawn.CanReserve(t, 1, -1, null, false) || !pawn2.Awake())
			{
				return null;
			}
			if (interactionMode == PrisonerInteractionModeDefOf.ReduceResistance && pawn2.guest.Resistance <= 0f)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerAttemptRecruit, t);
		}
	}
}
