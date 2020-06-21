﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001258 RID: 4696
	public class Settlement_TraderTracker : IThingHolder, IExposable
	{
		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06006DC4 RID: 28100 RVA: 0x00127946 File Offset: 0x00125B46
		protected virtual int RegenerateStockEveryDays
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06006DC5 RID: 28101 RVA: 0x00265951 File Offset: 0x00263B51
		public IThingHolder ParentHolder
		{
			get
			{
				return this.settlement;
			}
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06006DC6 RID: 28102 RVA: 0x00265959 File Offset: 0x00263B59
		public List<Thing> StockListForReading
		{
			get
			{
				if (this.stock == null)
				{
					this.RegenerateStock();
				}
				return this.stock.InnerListForReading;
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06006DC7 RID: 28103 RVA: 0x00265974 File Offset: 0x00263B74
		public TraderKindDef TraderKind
		{
			get
			{
				List<TraderKindDef> baseTraderKinds = this.settlement.Faction.def.baseTraderKinds;
				if (baseTraderKinds.NullOrEmpty<TraderKindDef>())
				{
					return null;
				}
				int index = Mathf.Abs(this.settlement.HashOffset()) % baseTraderKinds.Count;
				return baseTraderKinds[index];
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06006DC8 RID: 28104 RVA: 0x002659C0 File Offset: 0x00263BC0
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.settlement.ID, 1933327354);
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06006DC9 RID: 28105 RVA: 0x002659D7 File Offset: 0x00263BD7
		public bool EverVisited
		{
			get
			{
				return this.everGeneratedStock;
			}
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06006DCA RID: 28106 RVA: 0x002659DF File Offset: 0x00263BDF
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.everGeneratedStock && this.stock == null;
			}
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06006DCB RID: 28107 RVA: 0x002659F4 File Offset: 0x00263BF4
		public int NextRestockTick
		{
			get
			{
				if (this.stock == null || !this.everGeneratedStock)
				{
					return -1;
				}
				return ((this.lastStockGenerationTicks == -1) ? 0 : this.lastStockGenerationTicks) + this.RegenerateStockEveryDays * 60000;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06006DCC RID: 28108 RVA: 0x00265A28 File Offset: 0x00263C28
		public virtual string TraderName
		{
			get
			{
				if (this.settlement.Faction == null)
				{
					return this.settlement.LabelCap;
				}
				return "SettlementTrader".Translate(this.settlement.LabelCap, this.settlement.Faction.Name);
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06006DCD RID: 28109 RVA: 0x00265A82 File Offset: 0x00263C82
		public virtual bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && (this.stock == null || this.stock.InnerListForReading.Any((Thing x) => this.TraderKind.WillTrade(x.def)));
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06006DCE RID: 28110 RVA: 0x00265AB4 File Offset: 0x00263CB4
		public virtual float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x06006DCF RID: 28111 RVA: 0x00265ABB File Offset: 0x00263CBB
		public Settlement_TraderTracker(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06006DD0 RID: 28112 RVA: 0x00265ADC File Offset: 0x00263CDC
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpSavedPawns.Clear();
				if (this.stock != null)
				{
					for (int i = this.stock.Count - 1; i >= 0; i--)
					{
						Pawn pawn = this.stock[i] as Pawn;
						if (pawn != null)
						{
							this.stock.Remove(pawn);
							this.tmpSavedPawns.Add(pawn);
						}
					}
				}
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.stock, "stock", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastStockGenerationTicks, "lastStockGenerationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everGeneratedStock, "wasStockGeneratedYet", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving)
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.stock.TryAdd(this.tmpSavedPawns[j], false);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x06006DD1 RID: 28113 RVA: 0x00265BE3 File Offset: 0x00263DE3
		public virtual IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				yield return thing;
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			List<Pawn> pawns = caravan.PawnsListForReading;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (!caravan.IsOwner(pawns[i]))
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006DD2 RID: 28114 RVA: 0x00265BF4 File Offset: 0x00263DF4
		public virtual void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.stock == null)
			{
				this.RegenerateStock();
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.settlement);
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				if (pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!this.stock.TryAdd(pawn, false))
				{
					pawn.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.stock.TryAdd(thing, false))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x00265C80 File Offset: 0x00263E80
		public virtual void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.settlement);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				caravan.AddPawn(pawn, true);
				return;
			}
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn2 == null)
			{
				Log.Error("Could not find any pawn to give sold thing to.", false);
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add sold thing to inventory.", false);
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x00265D08 File Offset: 0x00263F08
		public virtual void TraderTrackerTick()
		{
			if (this.stock != null)
			{
				if (Find.TickManager.TicksGame - this.lastStockGenerationTicks > this.RegenerateStockEveryDays * 60000)
				{
					this.TryDestroyStock();
					return;
				}
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.stock[i] as Pawn;
					if (pawn != null && pawn.Destroyed)
					{
						this.stock.Remove(pawn);
					}
				}
				for (int j = this.stock.Count - 1; j >= 0; j--)
				{
					Pawn pawn2 = this.stock[j] as Pawn;
					if (pawn2 != null && !pawn2.IsWorldPawn())
					{
						Log.Error("Faction base has non-world-pawns in its stock. Removing...", false);
						this.stock.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x00265DD8 File Offset: 0x00263FD8
		public void TryDestroyStock()
		{
			if (this.stock != null)
			{
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Thing thing = this.stock[i];
					this.stock.Remove(thing);
					if (!(thing is Pawn) && !thing.Destroyed)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				this.stock = null;
			}
		}

		// Token: 0x06006DD6 RID: 28118 RVA: 0x00265E3D File Offset: 0x0026403D
		public bool ContainsPawn(Pawn p)
		{
			return this.stock != null && this.stock.Contains(p);
		}

		// Token: 0x06006DD7 RID: 28119 RVA: 0x00265E58 File Offset: 0x00264058
		protected virtual void RegenerateStock()
		{
			this.TryDestroyStock();
			this.stock = new ThingOwner<Thing>(this);
			this.everGeneratedStock = true;
			if (this.settlement.Faction == null || !this.settlement.Faction.IsPlayer)
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.traderDef = this.TraderKind;
				parms.tile = new int?(this.settlement.Tile);
				parms.makingFaction = this.settlement.Faction;
				this.stock.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
			}
			for (int i = 0; i < this.stock.Count; i++)
			{
				Pawn pawn = this.stock[i] as Pawn;
				if (pawn != null)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
			this.lastStockGenerationTicks = Find.TickManager.TicksGame;
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x00265F3F File Offset: 0x0026413F
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.stock;
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x00265F47 File Offset: 0x00264147
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x040043F4 RID: 17396
		public Settlement settlement;

		// Token: 0x040043F5 RID: 17397
		private ThingOwner<Thing> stock;

		// Token: 0x040043F6 RID: 17398
		private int lastStockGenerationTicks = -1;

		// Token: 0x040043F7 RID: 17399
		private bool everGeneratedStock;

		// Token: 0x040043F8 RID: 17400
		private const float DefaultTradePriceImprovement = 0.02f;

		// Token: 0x040043F9 RID: 17401
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
