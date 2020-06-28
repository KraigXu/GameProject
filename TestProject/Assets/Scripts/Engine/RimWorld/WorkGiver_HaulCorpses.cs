using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000748 RID: 1864
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		// Token: 0x060030DE RID: 12510 RVA: 0x00111F58 File Offset: 0x00110158
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Corpse))
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x00111F70 File Offset: 0x00110170
		public override string PostProcessedGerund(Job job)
		{
			if (job.GetTarget(TargetIndex.B).Thing is Building_Grave)
			{
				return "Burying".Translate();
			}
			return base.PostProcessedGerund(job);
		}
	}
}
