using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B35 RID: 2869
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x060043A1 RID: 17313 RVA: 0x0011D8FA File Offset: 0x0011BAFA
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
