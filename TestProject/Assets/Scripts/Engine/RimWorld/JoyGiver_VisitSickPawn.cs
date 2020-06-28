using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000702 RID: 1794
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		// Token: 0x06002F69 RID: 12137 RVA: 0x0010ABDC File Offset: 0x00108DDC
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!InteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return null;
			}
			Pawn pawn2 = SickPawnVisitUtility.FindRandomSickPawn(pawn, JoyCategory.Low);
			if (pawn2 == null)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
		}
	}
}
