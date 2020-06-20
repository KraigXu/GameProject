using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC3 RID: 3523
	public class Tradeable : Transferable
	{
		// Token: 0x17000F1F RID: 3871
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

		// Token: 0x17000F20 RID: 3872
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

		// Token: 0x17000F21 RID: 3873
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

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06005560 RID: 21856 RVA: 0x001C5C54 File Offset: 0x001C3E54
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06005561 RID: 21857 RVA: 0x001C5C61 File Offset: 0x001C3E61
		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06005562 RID: 21858 RVA: 0x001C5C6E File Offset: 0x001C3E6E
		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06005563 RID: 21859 RVA: 0x001C5C88 File Offset: 0x001C3E88
		public virtual bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06005564 RID: 21860 RVA: 0x001C5C9F File Offset: 0x001C3E9F
		public override bool HasAnyThing
		{
			get
			{
				return this.FirstThingColony != null || this.FirstThingTrader != null;
			}
		}

		// Token: 0x17000F27 RID: 3879
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

		// Token: 0x17000F28 RID: 3880
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

		// Token: 0x17000F29 RID: 3881
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

		// Token: 0x17000F2A RID: 3882
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

		// Token: 0x17000F2B RID: 3883
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

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x0600556A RID: 21866 RVA: 0x001C5D61 File Offset: 0x001C3F61
		public virtual bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x0600556B RID: 21867 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool IsFavor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600556C RID: 21868 RVA: 0x001C5D7A File Offset: 0x001C3F7A
		public virtual int CostToInt(float cost)
		{
			return Mathf.RoundToInt(cost);
		}

		// Token: 0x17000F2E RID: 3886
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

		// Token: 0x17000F2F RID: 3887
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

		// Token: 0x17000F30 RID: 3888
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

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06005570 RID: 21872 RVA: 0x001C5DDD File Offset: 0x001C3FDD
		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef);
			}
		}

		// Token: 0x17000F32 RID: 3890
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

		// Token: 0x06005572 RID: 21874 RVA: 0x001C5E12 File Offset: 0x001C4012
		public Tradeable()
		{
		}

		// Token: 0x06005573 RID: 21875 RVA: 0x001C5E48 File Offset: 0x001C4048
		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		// Token: 0x06005574 RID: 21876 RVA: 0x001C5E9F File Offset: 0x001C409F
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

		// Token: 0x06005575 RID: 21877 RVA: 0x001C5EC0 File Offset: 0x001C40C0
		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		// Token: 0x06005576 RID: 21878 RVA: 0x001C5ED8 File Offset: 0x001C40D8
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

		// Token: 0x06005577 RID: 21879 RVA: 0x001C5FDC File Offset: 0x001C41DC
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

		// Token: 0x06005578 RID: 21880 RVA: 0x001C6466 File Offset: 0x001C4666
		public virtual float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			if (action == TradeAction.PlayerBuys)
			{
				return this.pricePlayerBuy;
			}
			return this.pricePlayerSell;
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x001C647F File Offset: 0x001C467F
		public override int GetMinimumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return -this.CountHeldBy(Transactor.Trader);
			}
			return -this.CountHeldBy(Transactor.Colony);
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x001C649B File Offset: 0x001C469B
		public override int GetMaximumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return this.CountHeldBy(Transactor.Colony);
			}
			return this.CountHeldBy(Transactor.Trader);
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x001C64B5 File Offset: 0x001C46B5
		public override AcceptanceReport UnderflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("TraderHasNoMore".Translate());
			}
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x001C64E9 File Offset: 0x001C46E9
		public override AcceptanceReport OverflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			return new AcceptanceReport("TraderHasNoMore".Translate());
		}

		// Token: 0x0600557D RID: 21885 RVA: 0x001C651D File Offset: 0x001C471D
		private List<Thing> TransactorThings(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.thingsColony;
			}
			return this.thingsTrader;
		}

		// Token: 0x0600557E RID: 21886 RVA: 0x001C6530 File Offset: 0x001C4730
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

		// Token: 0x0600557F RID: 21887 RVA: 0x001C6568 File Offset: 0x001C4768
		public int CountPostDealFor(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.CountHeldBy(trans) + base.CountToTransferToSource;
			}
			return this.CountHeldBy(trans) + base.CountToTransferToDestination;
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x001C658C File Offset: 0x001C478C
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

		// Token: 0x06005581 RID: 21889 RVA: 0x001C6600 File Offset: 0x001C4800
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

		// Token: 0x06005582 RID: 21890 RVA: 0x001C6668 File Offset: 0x001C4868
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

		// Token: 0x06005583 RID: 21891 RVA: 0x001C66B8 File Offset: 0x001C48B8
		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x001C66C8 File Offset: 0x001C48C8
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

		// Token: 0x04002EBE RID: 11966
		public List<Thing> thingsColony = new List<Thing>();

		// Token: 0x04002EBF RID: 11967
		public List<Thing> thingsTrader = new List<Thing>();

		// Token: 0x04002EC0 RID: 11968
		private int countToTransfer;

		// Token: 0x04002EC1 RID: 11969
		private float pricePlayerBuy = -1f;

		// Token: 0x04002EC2 RID: 11970
		private float pricePlayerSell = -1f;

		// Token: 0x04002EC3 RID: 11971
		private float priceFactorBuy_TraderPriceType;

		// Token: 0x04002EC4 RID: 11972
		private float priceFactorSell_TraderPriceType;

		// Token: 0x04002EC5 RID: 11973
		private float priceFactorSell_ItemSellPriceFactor;

		// Token: 0x04002EC6 RID: 11974
		private float priceGain_PlayerNegotiator;

		// Token: 0x04002EC7 RID: 11975
		private float priceGain_Settlement;
	}
}
