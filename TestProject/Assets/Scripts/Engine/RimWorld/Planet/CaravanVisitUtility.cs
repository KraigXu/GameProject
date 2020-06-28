using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200123F RID: 4671
	[StaticConstructorOnStartup]
	public static class CaravanVisitUtility
	{
		// Token: 0x06006CD3 RID: 27859 RVA: 0x0025FC04 File Offset: 0x0025DE04
		public static Settlement SettlementVisitedNow(Caravan caravan)
		{
			if (!caravan.Spawned || caravan.pather.Moving)
			{
				return null;
			}
			List<Settlement> settlementBases = Find.WorldObjects.SettlementBases;
			for (int i = 0; i < settlementBases.Count; i++)
			{
				Settlement settlement = settlementBases[i];
				if (settlement.Tile == caravan.Tile && settlement.Faction != caravan.Faction && settlement.Visitable)
				{
					return settlement;
				}
			}
			return null;
		}

		// Token: 0x06006CD4 RID: 27860 RVA: 0x0025FC74 File Offset: 0x0025DE74
		public static Command TradeCommand(Caravan caravan, Faction faction = null, TraderKindDef trader = null)
		{
			Pawn bestNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, faction, trader);
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandTrade".Translate();
			command_Action.defaultDesc = "CommandTradeDesc".Translate();
			command_Action.icon = CaravanVisitUtility.TradeCommandTex;
			command_Action.action = delegate
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(caravan);
				if (settlement != null && settlement.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(bestNegotiator, settlement, false));
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(settlement.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, false, true);
				}
			};
			if (bestNegotiator == null)
			{
				if (trader != null && trader.permitRequiredForTrading != null && !caravan.PawnsListForReading.Any((Pawn p) => p.royalty != null && p.royalty.HasPermit(trader.permitRequiredForTrading, faction)))
				{
					command_Action.Disable("CommandTradeFailNeedPermit".Translate(trader.permitRequiredForTrading.LabelCap));
				}
				else
				{
					command_Action.Disable("CommandTradeFailNoNegotiator".Translate());
				}
			}
			if (bestNegotiator != null && bestNegotiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				command_Action.Disable("CommandTradeFailSocialDisabled".Translate());
			}
			return command_Action;
		}

		// Token: 0x040043A3 RID: 17315
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Trade", true);
	}
}
