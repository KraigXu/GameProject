using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000746 RID: 1862
	public abstract class WorkGiver_Haul : WorkGiver_Scanner
	{
		// Token: 0x060030D7 RID: 12503 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x00111EFA File Offset: 0x001100FA
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x00111F0C File Offset: 0x0011010C
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling().Count == 0;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x00111F26 File Offset: 0x00110126
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, forced))
			{
				return null;
			}
			return HaulAIUtility.HaulToStorageJob(pawn, t);
		}
	}
}
