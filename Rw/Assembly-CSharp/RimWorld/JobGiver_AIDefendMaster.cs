using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AE RID: 1710
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		// Token: 0x06002E39 RID: 11833 RVA: 0x00103E32 File Offset: 0x00102032
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x00103E3F File Offset: 0x0010203F
		protected override float GetFlagRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 5f;
		}

		// Token: 0x04001A5B RID: 6747
		private const float RadiusUnreleased = 5f;
	}
}
