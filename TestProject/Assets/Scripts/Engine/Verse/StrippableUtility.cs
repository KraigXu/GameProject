using System;
using RimWorld;

namespace Verse
{
	
	public static class StrippableUtility
	{
		
		public static bool CanBeStrippedByColony(Thing th)
		{
			IStrippable strippable = th as IStrippable;
			if (strippable == null)
			{
				return false;
			}
			if (!strippable.AnythingToStrip())
			{
				return false;
			}
			Pawn pawn = th as Pawn;
			return pawn == null || (!pawn.IsQuestLodger() && (pawn.Downed || (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure)));
		}

		
		public static void CheckSendStrippingImpactsGoodwillMessage(Thing th)
		{
			Pawn pawn;
			if ((pawn = (th as Pawn)) != null && !pawn.Dead && pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.Faction.def.hidden)
			{
				Messages.Message("MessageStrippingWillAngerFaction".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
