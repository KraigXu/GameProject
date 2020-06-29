using System;
using Verse;

namespace RimWorld
{
	
	public static class DownedRefugeeQuestUtility
	{
		
		public static Pawn GenerateRefugee(int tile)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 20f, true, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, new float?(0.2f), null, null, null, null, null, null, null));
			HealthUtility.DamageUntilDowned(pawn, false);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			return pawn;
		}

		
		public static Faction GetRandomFactionForRefugee()
		{
			Faction result;
			if (Rand.Chance(0.6f) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, true, false, TechLevel.Undefined))
			{
				return result;
			}
			return null;
		}

		
		private const float RelationWithColonistWeight = 20f;

		
		private const float ChanceToRedressWorldPawn = 0.2f;
	}
}
