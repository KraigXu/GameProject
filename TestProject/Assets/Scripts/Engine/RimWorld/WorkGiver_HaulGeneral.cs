using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000747 RID: 1863
	public class WorkGiver_HaulGeneral : WorkGiver_Haul
	{
		// Token: 0x060030DC RID: 12508 RVA: 0x00111F3B File Offset: 0x0011013B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t is Corpse)
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}
	}
}
