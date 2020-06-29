using System;
using Verse;

namespace RimWorld
{
	
	public static class PrisonerWillingToJoinQuestUtility
	{
		
		public static Pawn GeneratePrisoner(int tile, Faction hostFaction)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, hostFaction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 75f, true, true, true, true, false, false, true, true, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			pawn.guest.SetGuestStatus(hostFaction, true);
			return pawn;
		}

		
		private const float RelationWithColonistWeight = 75f;
	}
}
