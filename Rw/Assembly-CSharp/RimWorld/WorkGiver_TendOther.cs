using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000760 RID: 1888
	public class WorkGiver_TendOther : WorkGiver_Tend
	{
		// Token: 0x06003165 RID: 12645 RVA: 0x0011379E File Offset: 0x0011199E
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && pawn != t;
		}
	}
}
