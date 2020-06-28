using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000734 RID: 1844
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x0600306A RID: 12394 RVA: 0x0001028D File Offset: 0x0000E48D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x0010FABE File Offset: 0x0010DCBE
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x0010FAD4 File Offset: 0x0010DCD4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			if (((Pawn)t).guest.interactionMode != PrisonerInteractionModeDefOf.Execution || !pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerExecution, t);
		}

		// Token: 0x04001AE6 RID: 6886
		private static string IncapableOfViolenceLowerTrans;
	}
}
