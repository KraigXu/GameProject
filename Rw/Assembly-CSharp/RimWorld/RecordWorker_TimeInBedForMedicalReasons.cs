using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3B RID: 2875
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x060043AD RID: 17325 RVA: 0x0016C73C File Offset: 0x0016A93C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest == null || pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
