using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3A RID: 2874
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x060043AB RID: 17323 RVA: 0x0016C731 File Offset: 0x0016A931
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
