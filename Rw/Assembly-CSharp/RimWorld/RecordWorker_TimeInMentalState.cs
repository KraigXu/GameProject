using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3C RID: 2876
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x060043AF RID: 17327 RVA: 0x001031FB File Offset: 0x001013FB
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
