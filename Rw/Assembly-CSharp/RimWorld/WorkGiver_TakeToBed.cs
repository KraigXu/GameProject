using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075D RID: 1885
	public abstract class WorkGiver_TakeToBed : WorkGiver_Scanner
	{
		// Token: 0x06003153 RID: 12627 RVA: 0x00113533 File Offset: 0x00111733
		protected Building_Bed FindBed(Pawn pawn, Pawn patient)
		{
			return RestUtility.FindBedFor(patient, pawn, patient.HostFaction == pawn.Faction, false, false);
		}
	}
}
