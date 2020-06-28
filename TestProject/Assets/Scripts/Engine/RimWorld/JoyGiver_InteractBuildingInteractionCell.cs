using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F1 RID: 1777
	public class JoyGiver_InteractBuildingInteractionCell : JoyGiver_InteractBuilding
	{
		// Token: 0x06002F1B RID: 12059 RVA: 0x00108F18 File Offset: 0x00107118
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (t.InteractionCell.Standable(t.Map) && !t.IsForbidden(pawn) && !t.InteractionCell.IsForbidden(pawn) && !pawn.Map.pawnDestinationReservationManager.IsReserved(t.InteractionCell))
			{
				return JobMaker.MakeJob(this.def.jobDef, t, t.InteractionCell);
			}
			return null;
		}
	}
}
