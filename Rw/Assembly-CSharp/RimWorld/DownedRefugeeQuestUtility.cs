using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000939 RID: 2361
	public static class DownedRefugeeQuestUtility
	{
		// Token: 0x060037F0 RID: 14320 RVA: 0x0012C054 File Offset: 0x0012A254
		public static Pawn GenerateRefugee(int tile)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 20f, true, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, new float?(0.2f), null, null, null, null, null, null, null));
			HealthUtility.DamageUntilDowned(pawn, false);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			return pawn;
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x0012C0D8 File Offset: 0x0012A2D8
		public static Faction GetRandomFactionForRefugee()
		{
			Faction result;
			if (Rand.Chance(0.6f) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, true, false, TechLevel.Undefined))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04002110 RID: 8464
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04002111 RID: 8465
		private const float ChanceToRedressWorldPawn = 0.2f;
	}
}
