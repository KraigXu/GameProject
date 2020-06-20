using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3E RID: 2878
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x060043B3 RID: 17331 RVA: 0x0016C7A0 File Offset: 0x0016A9A0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
