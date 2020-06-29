﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		
		
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		
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

		
		private static string IncapableOfViolenceLowerTrans;
	}
}
