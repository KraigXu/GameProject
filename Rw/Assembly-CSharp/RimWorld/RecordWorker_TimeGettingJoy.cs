using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B38 RID: 2872
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x060043A7 RID: 17319 RVA: 0x0016C6F0 File Offset: 0x0016A8F0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
