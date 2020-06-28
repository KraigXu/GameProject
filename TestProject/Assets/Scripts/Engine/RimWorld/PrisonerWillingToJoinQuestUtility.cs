using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093B RID: 2363
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x060037F8 RID: 14328 RVA: 0x0012C3E8 File Offset: 0x0012A5E8
		public static Pawn GeneratePrisoner(int tile, Faction hostFaction)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, hostFaction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 75f, true, true, true, true, false, false, true, true, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			pawn.guest.SetGuestStatus(hostFaction, true);
			return pawn;
		}

		// Token: 0x04002112 RID: 8466
		private const float RelationWithColonistWeight = 75f;
	}
}
