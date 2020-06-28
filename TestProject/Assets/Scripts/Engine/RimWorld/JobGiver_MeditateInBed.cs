using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E2 RID: 1762
	public class JobGiver_MeditateInBed : JobGiver_Meditate
	{
		// Token: 0x06002EE4 RID: 12004 RVA: 0x001076D0 File Offset: 0x001058D0
		protected override bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() != null && pawn.Awake();
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x001076E4 File Offset: 0x001058E4
		protected override Job TryGiveJob(Pawn pawn)
		{
			LocalTargetInfo targetC = MeditationUtility.BestFocusAt(pawn.Position, pawn);
			Job job = JobMaker.MakeJob(JobDefOf.Meditate, pawn.Position, pawn.InBed() ? pawn.CurrentBed() : new LocalTargetInfo(pawn.Position), targetC);
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
