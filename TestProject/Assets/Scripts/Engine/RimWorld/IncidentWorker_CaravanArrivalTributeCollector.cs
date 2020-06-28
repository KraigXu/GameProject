using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009FB RID: 2555
	public class IncidentWorker_CaravanArrivalTributeCollector : IncidentWorker_TraderCaravanArrival
	{
		// Token: 0x06003CCA RID: 15562 RVA: 0x001418D4 File Offset: 0x0013FAD4
		protected override bool TryResolveParmsGeneral(IncidentParms parms)
		{
			if (!base.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			parms.faction = Faction.Empire;
			parms.traderKind = (from t in DefDatabase<TraderKindDef>.AllDefsListForReading
			where t.category == "TributeCollector"
			select t).RandomElementByWeight((TraderKindDef t) => this.TraderKindCommonality(t, map, parms.faction));
			return true;
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x00141971 File Offset: 0x0013FB71
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && this.FactionCanBeGroupSource(Faction.Empire, (Map)parms.target, false);
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x00141995 File Offset: 0x0013FB95
		protected override float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
		{
			return traderKind.CalculatedCommonality;
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x001419A0 File Offset: 0x0013FBA0
		protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
		{
			TaggedString baseLetterLabel = "LetterLabelTributeCollectorArrival".Translate().CapitalizeFirst();
			TaggedString taggedString = "LetterTributeCollectorArrival".Translate(parms.faction.Named("FACTION")).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x04002394 RID: 9108
		public const string TributeCollectorTraderKindCategory = "TributeCollector";
	}
}
