﻿using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class TradeDeal
	{
		
		
		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

		
		
		public Tradeable CurrencyTradeable
		{
			get
			{
				for (int i = 0; i < this.tradeables.Count; i++)
				{
					if ((TradeSession.TradeCurrency == TradeCurrency.Favor) ? this.tradeables[i].IsFavor : (this.tradeables[i].ThingDef == ThingDefOf.Silver))
					{
						return this.tradeables[i];
					}
				}
				return null;
			}
		}

		
		
		public List<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		
		public TradeDeal()
		{
			this.Reset();
		}

		
		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		
		private void AddAllTradeables()
		{
			foreach (Thing t in TradeSession.trader.ColonyThingsWillingToBuy(TradeSession.playerNegotiator))
			{
				if (TradeUtility.PlayerSellableNow(t, TradeSession.trader))
				{
					string text;
					if (!TradeSession.playerNegotiator.IsWorldPawn() && !this.InSellablePosition(t, out text))
					{
						if (text != null && !this.cannotSellReasons.Contains(text))
						{
							this.cannotSellReasons.Add(text);
						}
					}
					else
					{
						this.AddToTradeables(t, Transactor.Colony);
					}
				}
			}
			if (!TradeSession.giftMode)
			{
				foreach (Thing t2 in TradeSession.trader.Goods)
				{
					this.AddToTradeables(t2, Transactor.Trader);
				}
			}
			if (!TradeSession.giftMode)
			{
				if (this.tradeables.Find((Tradeable x) => x.IsCurrency) == null)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
					thing.stackCount = 0;
					this.AddToTradeables(thing, Transactor.Trader);
				}
			}
			if (TradeSession.TradeCurrency == TradeCurrency.Favor)
			{
				this.tradeables.Add(new Tradeable_RoyalFavor());
			}
		}

		
		private bool InSellablePosition(Thing t, out string reason)
		{
			if (!t.Spawned)
			{
				reason = null;
				return false;
			}
			if (t.Position.Fogged(t.Map))
			{
				reason = null;
				return false;
			}
			Room room = t.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				int num = GenRadial.NumCellsInRadius(6.9f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = t.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(t.Map) && intVec.GetRoom(t.Map, RegionType.Set_Passable) == room)
					{
						List<Thing> thingList = intVec.GetThingList(t.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].PreventPlayerSellingThingsNearby(out reason))
							{
								return false;
							}
						}
					}
				}
			}
			reason = null;
			return true;
		}

		
		private void AddToTradeables(Thing t, Transactor trans)
		{
			Tradeable tradeable = TransferableUtility.TradeableMatching(t, this.tradeables);
			if (tradeable == null)
			{
				if (t is Pawn)
				{
					tradeable = new Tradeable_Pawn();
				}
				else
				{
					tradeable = new Tradeable();
				}
				this.tradeables.Add(tradeable);
			}
			tradeable.AddThing(t, trans);
		}

		
		public void UpdateCurrencyCount()
		{
			if (this.CurrencyTradeable == null || TradeSession.giftMode)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.tradeables.Count; i++)
			{
				Tradeable tradeable = this.tradeables[i];
				if (!tradeable.IsCurrency)
				{
					num += tradeable.CurTotalCurrencyCostForSource;
				}
			}
			this.CurrencyTradeable.ForceToSource(-this.CurrencyTradeable.CostToInt(num));
		}

		
		public bool TryExecute(out bool actuallyTraded)
		{
			if (TradeSession.giftMode)
			{
				this.UpdateCurrencyCount();
				this.LimitCurrencyCountToFunds();
				int goodwillChange = FactionGiftUtility.GetGoodwillChange(this.tradeables, TradeSession.trader.Faction);
				FactionGiftUtility.GiveGift(this.tradeables, TradeSession.trader.Faction, TradeSession.playerNegotiator);
				actuallyTraded = ((float)goodwillChange > 0f);
				return true;
			}
			if (this.CurrencyTradeable == null || this.CurrencyTradeable.CountPostDealFor(Transactor.Colony) < 0)
			{
				Find.WindowStack.WindowOfType<Dialog_Trade>().FlashSilver();
				Messages.Message("MessageColonyCannotAfford".Translate(), MessageTypeDefOf.RejectInput, false);
				actuallyTraded = false;
				return false;
			}
			this.UpdateCurrencyCount();
			this.LimitCurrencyCountToFunds();
			actuallyTraded = false;
			float num = 0f;
			foreach (Tradeable tradeable in this.tradeables)
			{
				if (tradeable.ActionToDo != TradeAction.None)
				{
					actuallyTraded = true;
				}
				if (tradeable.ActionToDo == TradeAction.PlayerSells)
				{
					num += tradeable.CurTotalCurrencyCostForDestination;
				}
				tradeable.ResolveTrade();
			}
			this.Reset();
			if (TradeSession.trader.Faction != null)
			{
				TradeSession.trader.Faction.Notify_PlayerTraded(num, TradeSession.playerNegotiator);
			}
			Pawn pawn = TradeSession.trader as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.TradedWith, new object[]
				{
					TradeSession.playerNegotiator,
					pawn
				});
			}
			if (actuallyTraded)
			{
				TradeSession.playerNegotiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Trade);
			}
			return true;
		}

		
		public bool DoesTraderHaveEnoughSilver()
		{
			return TradeSession.giftMode || (this.CurrencyTradeable != null && this.CurrencyTradeable.CountPostDealFor(Transactor.Trader) >= 0);
		}

		
		private void LimitCurrencyCountToFunds()
		{
			if (this.CurrencyTradeable == null)
			{
				return;
			}
			if (this.CurrencyTradeable.CountToTransferToSource > this.CurrencyTradeable.CountHeldBy(Transactor.Trader))
			{
				this.CurrencyTradeable.ForceToSource(this.CurrencyTradeable.CountHeldBy(Transactor.Trader));
			}
			if (this.CurrencyTradeable.CountToTransferToDestination > this.CurrencyTradeable.CountHeldBy(Transactor.Colony))
			{
				this.CurrencyTradeable.ForceToDestination(this.CurrencyTradeable.CountHeldBy(Transactor.Colony));
			}
		}

		
		private List<Tradeable> tradeables = new List<Tradeable>();

		
		public List<string> cannotSellReasons = new List<string>();
	}
}
