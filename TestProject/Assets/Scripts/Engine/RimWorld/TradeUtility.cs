using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DCF RID: 3535
	public static class TradeUtility
	{
		// Token: 0x060055CD RID: 21965 RVA: 0x001C7710 File Offset: 0x001C5910
		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		// Token: 0x060055CE RID: 21966 RVA: 0x001C7774 File Offset: 0x001C5974
		public static bool PlayerSellableNow(Thing t, ITrader trader)
		{
			t = t.GetInnerIfMinified();
			if (!TradeUtility.EverPlayerSellable(t.def))
			{
				return false;
			}
			if (t.IsNotFresh())
			{
				return false;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null && apparel.WornByCorpse)
			{
				return false;
			}
			if (EquipmentUtility.IsBiocoded(t))
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			return pawn == null || ((pawn.GetExtraHostFaction(null) == null || pawn.GetExtraHostFaction(null) != trader.Faction) && (!pawn.IsQuestLodger() || pawn.GetExtraHomeFaction(null) != trader.Faction));
		}

		// Token: 0x060055CF RID: 21967 RVA: 0x001C77FC File Offset: 0x001C59FC
		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveDropPodInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			});
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x001C7825 File Offset: 0x001C5A25
		public static IEnumerable<Thing> AllLaunchableThingsForTrade(Map map, ITrader trader = null)
		{
			HashSet<Thing> yieldedThings = new HashSet<Thing>();
			foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
			{
				foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
				{
					List<Thing> thingList = c.GetThingList(map);
					int num;
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						Thing thing = thingList[i];
						if (thing.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(thing, trader) && !yieldedThings.Contains(thing))
						{
							yieldedThings.Add(thing);
							yield return thing;
						}
						num = i;
					}
					thingList = null;
				}
				IEnumerator<IntVec3> enumerator2 = null;
			}
			IEnumerator<Building_OrbitalTradeBeacon> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060055D1 RID: 21969 RVA: 0x001C783C File Offset: 0x001C5A3C
		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.PrisonersOfColonySpawned)
			{
				if (pawn.guest.PrisonerIsSecure)
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			foreach (Pawn pawn2 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn2.RaceProps.Animal && pawn2.HostFaction == null && !pawn2.InMentalState && !pawn2.Downed && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(pawn2.def))
				{
					yield return pawn2;
				}
			}
			enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060055D2 RID: 21970 RVA: 0x001C784C File Offset: 0x001C5A4C
		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			if (thing is Pawn)
			{
				return null;
			}
			foreach (Thing thing2 in trader.Goods)
			{
				if (TransferableUtility.TransferAsOne(thing2, thing, TransferAsOneMode.Normal) && thing2.CanStackWith(thing) && thing2.def.stackLimit != 1)
				{
					return thing2;
				}
			}
			return null;
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x001C78C4 File Offset: 0x001C5AC4
		public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
		{
			while (debt > 0)
			{
				Thing thing = null;
				foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
				{
					foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
					{
						foreach (Thing thing2 in map.thingGrid.ThingsAt(c))
						{
							if (thing2.def == resDef)
							{
								thing = thing2;
								goto IL_9D;
							}
						}
					}
				}
				IL_9D:
				if (thing == null)
				{
					Log.Error("Could not find any " + resDef + " to transfer to trader.", false);
					return;
				}
				int num = Math.Min(debt, thing.stackCount);
				if (trader != null)
				{
					trader.GiveSoldThingToTrader(thing, num, TradeSession.playerNegotiator);
				}
				else
				{
					thing.SplitOff(num).Destroy(DestroyMode.Vanish);
				}
				debt -= num;
			}
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x001C79E8 File Offset: 0x001C5BE8
		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x001C79F8 File Offset: 0x001C5BF8
		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x001C7A54 File Offset: 0x001C5C54
		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		// Token: 0x060055D7 RID: 21975 RVA: 0x001C7AB8 File Offset: 0x001C5CB8
		public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
			}
		}

		// Token: 0x060055D8 RID: 21976 RVA: 0x001C7AF4 File Offset: 0x001C5CF4
		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase, TradeCurrency currency = TradeCurrency.Silver)
		{
			if (currency == TradeCurrency.Favor)
			{
				return thing.RoyalFavorValue;
			}
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true);
			float num = thing.MarketValue * 0.6f * priceFactorSell_TraderPriceType * statValue * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_FactionBase;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x001C7B6C File Offset: 0x001C5D6C
		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.4f * priceFactorBuy_TraderPriceType * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x04002EE4 RID: 12004
		public const float MinimumBuyPrice = 0.5f;

		// Token: 0x04002EE5 RID: 12005
		public const float MinimumSellPrice = 0.01f;

		// Token: 0x04002EE6 RID: 12006
		public const float PriceFactorBuy_Global = 1.4f;

		// Token: 0x04002EE7 RID: 12007
		public const float PriceFactorSell_Global = 0.6f;
	}
}
