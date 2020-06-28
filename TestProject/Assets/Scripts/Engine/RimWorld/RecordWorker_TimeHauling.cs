using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B39 RID: 2873
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x060043A9 RID: 17321 RVA: 0x0016C717 File Offset: 0x0016A917
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
