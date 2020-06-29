using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Tradeable : Transferable
	{
		
		// (get) Token: 0x0600555C RID: 21852 RVA: 0x001C5BFD File Offset: 0x001C3DFD
		// (set) Token: 0x0600555D RID: 21853 RVA: 0x001C5C05 File Offset: 0x001C3E05
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		
		// (get) Token: 0x0600555E RID: 21854 RVA: 0x001C5C1A File Offset: 0x001C3E1A
		public Thing FirstThingColony
		{
			get
			{
				if (this.thingsColony.Count == 0)
				{
					return null;
				}
				return this.thingsColony[0];
			}
		}

		
		// (get) Token: 0x0600555F RID: 21855 RVA: 0x001C5C37 File Offset: 0x001C3E37
		public Thing FirstThingTrader
		{
			get
			{
				if (this.thingsTrader.Count == 0)
				{
					return null;
				}
				return this.thingsTrader[0];
			}
		}

		
		// (get) Token: 0x06005560 RID: 21856 RVA: 0x001C5C54 File Offset: 0x001C3E54
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		
		// (get) Token: 0x06005561 RID: 21857 RVA: 0x001C5C61 File Offset: 0x001C3E61
		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		
		// (get) Token: 0x06005562 RID: 21858 RVA: 0x001C5C6E File Offset: 0x001C3E6E
		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		
		// (get) Token: 0x06005563 RID: 21859 RVA: 0x001C5C88 File Offset: 0x001C3E88
		public virtual bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
			}
		}

		
		// (get) Token: 0x06005564 RID: 21860 RVA: 0x001C5C9F File Offset: 0x001C3E9F
		public override bool HasAnyThing
		{
			get
			{
				return this.FirstThingColony != null || this.FirstThingTrader != null;
			}
		}

		
		// (get) Token: 0x06005565 RID: 21861 RVA: 0x001C5CB4 File Offset: 0x001C3EB4
		public override Thing AnyThing
		{
			get
			{
				if (this.FirstThingColony != null)
				{
					return this.FirstThingColony.GetInnerIfMinified();
				}
				if (this.FirstThingTrader != null)
				{
					return this.FirstThingTrader.GetInnerIfMinified();
				}
				Log.Error(base.GetType() + " lacks AnyThing.", false);
				return null;
			}
		}

		
		// (get) Token: 0x06005566 RID: 21862 RVA: 0x001C5D00 File Offset: 0x001C3F00
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		
		// (get) Token: 0x06005567 RID: 21863 RVA: 0x001C5D17 File Offset: 0x001C3F17
		public ThingDef StuffDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.Stuff;
			}
		}

		
		// (get) Token: 0x06005568 RID: 21864 RVA: 0x001C5D2E File Offset: 0x001C3F2E
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		
		// (get) Token: 0x06005569 RID: 21865 RVA: 0x001C5D49 File Offset: 0x001C3F49
		public TradeAction ActionToDo
		{
			get
			{
				if (this.CountToTransfer == 0)
				{
					return TradeAction.None;
				}
				if (base.CountToTransferToDestination > 0)
				{
					return TradeAction.PlayerSells;
				}
				return TradeAction.PlayerBuys;
			}
		}

		
		// (get) Token: 0x0600556A RID: 21866 RVA: 0x001C5D61 File Offset: 0x001C3F61
		public virtual bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		
		// (get) Token: 0x0600556B RID: 21867 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool IsFavor
		{
			get
			{
				return false;
			}
		}

		
		public virtual int CostToInt(float cost)
		{
			return Mathf.RoundToInt(cost);
		}

		
		// (get) Token: 0x0600556D RID: 21869 RVA: 0x001C5D82 File Offset: 0x001C3F82
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				if (TradeSession.Active && TradeSession.giftMode)
				{
					return TransferablePositiveCountDirection.Destination;
				}
				return TransferablePositiveCountDirection.Source;
			}
		}

		
		// (get) Token: 0x0600556E RID: 21870 RVA: 0x001C5D95 File Offset: 0x001C3F95
		public float CurTotalCurrencyCostForSource
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToSource * this.GetPriceFor(this.ActionToDo);
			}
		}

		
		// (get) Token: 0x0600556F RID: 21871 RVA: 0x001C5DB9 File Offset: 0x001C3FB9
		public float CurTotalCurrencyCostForDestination
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToDestination * this.GetPriceFor(this.ActionToDo);
			}
		}

		
		// (get) Token: 0x06005570 RID: 21872 RVA: 0x001C5DDD File Offset: 0x001C3FDD
		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef);
			}
		}

		
		// (get) Token: 0x06005571 RID: 21873 RVA: 0x001C5DEA File Offset: 0x001C3FEA
		private bool Bugged
		{
			get
			{
				if (!this.HasAnyThing)
				{
					Log.ErrorOnce(this.ToString() + " is bugged. There will be no more logs about this.", 162112, false);
					return true;
				}
				return false;
			}
		}

		
		public Tradeable()
		{
		}

		
		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		
		public void AddThing(Thing t, Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				this.thingsColony.Add(t);
			}
			if (trans == Transactor.Trader)
			{
				this.thingsTrader.Add(t);
			}
		}

		
		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		
		private void InitPriceDataIfNeeded()
		{
			if (this.pricePlayerBuy > 0f)
			{
				return;
			}
			if (this.IsCurrency)
			{
				this.pricePlayerBuy = this.BaseMarketValue;
				this.pricePlayerSell = this.BaseMarketValue;
				return;
			}
			this.priceFactorBuy_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerBuys).PriceMultiplier();
			this.priceFactorSell_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerSells).PriceMultiplier();
			this.priceGain_PlayerNegotiator = TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true);
			this.priceGain_Settlement = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
			this.priceFactorSell_ItemSellPriceFactor = this.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true);
			this.pricePlayerBuy = TradeUtility.GetPricePlayerBuy(this.AnyThing, this.priceFactorBuy_TraderPriceType, this.priceGain_PlayerNegotiator, this.priceGain_Settlement);
			this.pricePlayerSell = TradeUtility.GetPricePlayerSell(this.AnyThing, this.priceFactorSell_TraderPriceType, this.priceGain_PlayerNegotiator, this.priceGain_Settlement, TradeSession.TradeCurrency);
			if (this.pricePlayerSell >= this.pricePlayerBuy)
			{
				this.pricePlayerSell = this.pricePlayerBuy;
			}
		}

		
		public string GetPriceTooltip(TradeAction action)
		{
			if (!this.HasAnyThing)
			{
				return "";
			}
			this.InitPriceDataIfNeeded();
			string text = (action == TradeAction.PlayerBuys) ? "BuyPriceDesc".Translate() : "SellPriceDesc".Translate();
			if (TradeSession.TradeCurrency != TradeCurrency.Silver)
			{
				return text;
			}
			text += "\n\n";
			text += StatDefOf.MarketValue.LabelCap + ": " + this.BaseMarketValue.ToStringMoney(null);
			if (action == TradeAction.PlayerBuys)
			{
				text += "\n  x " + 1.4f.ToString("F2") + " (" + "Buying".Translate() + ")";
				if (this.priceFactorBuy_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorBuy_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f + Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": -" + this.priceGain_PlayerNegotiator.ToStringPercent();
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": -" + this.priceGain_Settlement.ToStringPercent();
				}
			}
			else
			{
				text += "\n  x " + 0.6f.ToString("F2") + " (" + "Selling".Translate() + ")";
				if (this.priceFactorSell_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorSell_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (this.priceFactorSell_ItemSellPriceFactor != 1f)
				{
					text += "\n  x " + this.priceFactorSell_ItemSellPriceFactor.ToString("F2") + " (" + "ItemSellPriceFactor".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f - Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": " + this.priceGain_PlayerNegotiator.ToStringPercent();
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": " + this.priceGain_Settlement.ToStringPercent();
				}
			}
			text += "\n\n";
			float priceFor = this.GetPriceFor(action);
			text += "FinalPrice".Translate() + ": " + priceFor.ToStringMoney(null);
			if ((action == TradeAction.PlayerBuys && priceFor <= 0.5f) || (action == TradeAction.PlayerBuys && priceFor <= 0.01f))
			{
				text += " (" + "minimum".Translate() + ")";
			}
			return text;
		}

		
		public virtual float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			if (action == TradeAction.PlayerBuys)
			{
				return this.pricePlayerBuy;
			}
			return this.pricePlayerSell;
		}

		
		public override int GetMinimumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return -this.CountHeldBy(Transactor.Trader);
			}
			return -this.CountHeldBy(Transactor.Colony);
		}

		
		public override int GetMaximumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return this.CountHeldBy(Transactor.Colony);
			}
			return this.CountHeldBy(Transactor.Trader);
		}

		
		public override AcceptanceReport UnderflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("TraderHasNoMore".Translate());
			}
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		
		public override AcceptanceReport OverflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			return new AcceptanceReport("TraderHasNoMore".Translate());
		}

		
		private List<Thing> TransactorThings(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.thingsColony;
			}
			return this.thingsTrader;
		}

		
		public virtual int CountHeldBy(Transactor trans)
		{
			List<Thing> list = this.TransactorThings(trans);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].stackCount;
			}
			return num;
		}

		
		public int CountPostDealFor(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.CountHeldBy(trans) + base.CountToTransferToSource;
			}
			return this.CountHeldBy(trans) + base.CountToTransferToDestination;
		}

		
		public virtual void ResolveTrade()
		{
			if (this.ActionToDo == TradeAction.PlayerSells)
			{
				TransferableUtility.TransferNoSplit(this.thingsColony, base.CountToTransferToDestination, delegate(Thing thing, int countToTransfer)
				{
					TradeSession.trader.GiveSoldThingToTrader(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
				return;
			}
			if (this.ActionToDo == TradeAction.PlayerBuys)
			{
				TransferableUtility.TransferNoSplit(this.thingsTrader, base.CountToTransferToSource, delegate(Thing thing, int countToTransfer)
				{
					this.CheckTeachOpportunity(thing, countToTransfer);
					TradeSession.trader.GiveSoldThingToPlayer(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
			}
		}

		
		private void CheckTeachOpportunity(Thing boughtThing, int boughtCount)
		{
			Building building = boughtThing as Building;
			if (building == null)
			{
				MinifiedThing minifiedThing = boughtThing as MinifiedThing;
				if (minifiedThing != null)
				{
					building = (minifiedThing.InnerThing as Building);
				}
			}
			if (building != null && building.def.building != null && building.def.building.boughtConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(building.def.building.boughtConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType(),
				"(",
				this.ThingDef,
				", countToTransfer=",
				this.CountToTransfer,
				")"
			});
		}

		
		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.thingsColony.RemoveAll((Thing x) => x.Destroyed);
				this.thingsTrader.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.thingsColony, "thingsColony", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsTrader, "thingsTrader", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsColony.RemoveAll((Thing x) => x == null) == 0)
				{
					if (this.thingsTrader.RemoveAll((Thing x) => x == null) == 0)
					{
						return;
					}
				}
				Log.Warning("Some of the things were null after loading.", false);
			}
		}

		
		public List<Thing> thingsColony = new List<Thing>();

		
		public List<Thing> thingsTrader = new List<Thing>();

		
		private int countToTransfer;

		
		private float pricePlayerBuy = -1f;

		
		private float pricePlayerSell = -1f;

		
		private float priceFactorBuy_TraderPriceType;

		
		private float priceFactorSell_TraderPriceType;

		
		private float priceFactorSell_ItemSellPriceFactor;

		
		private float priceGain_PlayerNegotiator;

		
		private float priceGain_Settlement;
	}
}
