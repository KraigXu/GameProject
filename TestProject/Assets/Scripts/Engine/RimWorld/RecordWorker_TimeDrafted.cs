using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B37 RID: 2871
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x060043A5 RID: 17317 RVA: 0x0011DAB3 File Offset: 0x0011BCB3
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
