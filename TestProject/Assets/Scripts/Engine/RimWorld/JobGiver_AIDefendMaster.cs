using System;
using Verse;

namespace RimWorld
{
	
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		
		protected override float GetFlagRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 5f;
		}

		
		private const float RadiusUnreleased = 5f;
	}
}
