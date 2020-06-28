using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000298 RID: 664
	public static class StrippableUtility
	{
		// Token: 0x060012BC RID: 4796 RVA: 0x0006BA30 File Offset: 0x00069C30
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

		// Token: 0x060012BD RID: 4797 RVA: 0x0006BA8C File Offset: 0x00069C8C
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
