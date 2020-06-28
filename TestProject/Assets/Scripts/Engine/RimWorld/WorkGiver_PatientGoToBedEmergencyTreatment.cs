using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000751 RID: 1873
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x06003111 RID: 12561 RVA: 0x00112B36 File Offset: 0x00110D36
		public override Job NonScanJob(Pawn pawn)
		{
			if (!HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			return base.NonScanJob(pawn);
		}
	}
}
