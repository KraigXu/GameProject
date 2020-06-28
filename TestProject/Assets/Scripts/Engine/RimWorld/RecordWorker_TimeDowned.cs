using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B36 RID: 2870
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x060043A3 RID: 17315 RVA: 0x0011D8B2 File Offset: 0x0011BAB2
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
