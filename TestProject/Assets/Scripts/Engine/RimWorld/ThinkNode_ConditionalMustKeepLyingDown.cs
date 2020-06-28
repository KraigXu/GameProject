using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007EA RID: 2026
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		// Token: 0x060033C9 RID: 13257 RVA: 0x0011DE10 File Offset: 0x0011C010
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.CurJob == null || !pawn.GetPosture().Laying())
			{
				return false;
			}
			if (!pawn.Downed)
			{
				if (RestUtility.DisturbancePreventsLyingDown(pawn))
				{
					return false;
				}
				if (!pawn.CurJob.restUntilHealed || !HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					if (!pawn.jobs.curDriver.asleep)
					{
						return false;
					}
					if (!pawn.CurJob.playerForced && RestUtility.TimetablePreventsLayDown(pawn))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
