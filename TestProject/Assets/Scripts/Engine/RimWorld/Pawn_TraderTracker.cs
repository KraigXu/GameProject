﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class Pawn_TraderTracker : IExposable
	{
		
		// (get) Token: 0x060046C9 RID: 18121 RVA: 0x0017F039 File Offset: 0x0017D239
		public IEnumerable<Thing> Goods
		{
			get
			{
				Lord lord = this.pawn.GetLord();
				if (lord == null || !(lord.LordJob is LordJob_TradeWithColony))
				{
					int num;
					for (int i = 0; i < this.pawn.inventory.innerContainer.Count; i = num + 1)
					{
						Thing thing = this.pawn.inventory.innerContainer[i];
						if (!this.pawn.inventory.NotForSale(thing))
						{
							yield return thing;
						}
						num = i;
					}
				}
				if (lord != null)
				{
					int num;
					for (int i = 0; i < lord.ownedPawns.Count; i = num + 1)
					{
						Pawn p = lord.ownedPawns[i];
						TraderCaravanRole traderCaravanRole = p.GetTraderCaravanRole();
						if (traderCaravanRole == TraderCaravanRole.Carrier)
						{
							for (int j = 0; j < p.inventory.innerContainer.Count; j = num + 1)
							{
								yield return p.inventory.innerContainer[j];
								num = j;
							}
						}
						else if (traderCaravanRole == TraderCaravanRole.Chattel && !this.soldPrisoners.Contains(p))
						{
							yield return p;
						}
						p = null;
						num = i;
					}
				}
				yield break;
			}
		}

		
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x0017F049 File Offset: 0x0017D249
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.pawn.thingIDNumber, 1149275593);
			}
		}

		
		// (get) Token: 0x060046CB RID: 18123 RVA: 0x0017F060 File Offset: 0x0017D260
		public string TraderName
		{
			get
			{
				return this.pawn.LabelShort;
			}
		}

		
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x0017F070 File Offset: 0x0017D270
		public bool CanTradeNow
		{
			get
			{
				return !this.pawn.Dead && this.pawn.Spawned && this.pawn.mindState.wantsToTradeWithColony && this.pawn.CanCasuallyInteractNow(false) && !this.pawn.Downed && !this.pawn.IsPrisoner && this.pawn.Faction != Faction.OfPlayer && (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer)) && (this.Goods.Any((Thing x) => this.traderKind.WillTrade(x.def)) || this.traderKind.tradeCurrency == TradeCurrency.Favor);
			}
		}

		
		public Pawn_TraderTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> enumerable = from x in this.pawn.Map.listerThings.AllThings
			where x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x, this.pawn) && !x.Position.Fogged(x.Map) && (this.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && this.ReachableForTrade(x)
			select x;
			foreach (Thing thing in enumerable)
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.pawn.GetLord() != null)
			{
				foreach (Pawn pawn in from x in TradeUtility.AllSellableColonyPawns(this.pawn.Map)
				where !x.Downed && this.ReachableForTrade(x)
				select x)
				{
					yield return pawn;
				}
				IEnumerator<Pawn> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.", false);
				return;
			}
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
				this.AddPawnToStock(pawn);
				return;
			}
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this.pawn, thing);
			if (thing2 != null)
			{
				if (!thing2.TryAbsorbStack(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				this.AddThingToRandomInventory(thing);
			}
		}

		
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.Undefined, null);
				}
				if (this.soldPrisoners.Contains(pawn))
				{
					this.soldPrisoners.Remove(pawn);
					return;
				}
			}
			else
			{
				IntVec3 positionHeld = toGive.PositionHeld;
				Map mapHeld = toGive.MapHeld;
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null, null, default(Rot4)))
				{
					Lord lord2 = this.pawn.GetLord();
					if (lord2 != null)
					{
						lord2.extraForbiddenThings.Add(thing);
						return;
					}
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not place bought thing ",
						thing,
						" at ",
						positionHeld
					}), false);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		
		private void AddPawnToStock(Pawn newPawn)
		{
			if (!newPawn.Spawned)
			{
				GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map, WipeMode.Vanish);
			}
			if (newPawn.Faction != this.pawn.Faction)
			{
				newPawn.SetFaction(this.pawn.Faction, null);
			}
			if (newPawn.RaceProps.Humanlike)
			{
				newPawn.kindDef = PawnKindDefOf.Slave;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				newPawn.Destroy(DestroyMode.Vanish);
				Log.Error(string.Concat(new object[]
				{
					"Tried to sell pawn ",
					newPawn,
					" to ",
					this.pawn,
					", but ",
					this.pawn,
					" has no lord. Traders without lord can't buy pawns."
				}), false);
				return;
			}
			if (newPawn.RaceProps.Humanlike)
			{
				this.soldPrisoners.Add(newPawn);
			}
			lord.AddPawn(newPawn);
		}

		
		private void AddThingToRandomInventory(Thing thing)
		{
			Lord lord = this.pawn.GetLord();
			IEnumerable<Pawn> source = Enumerable.Empty<Pawn>();
			if (lord != null)
			{
				source = from x in lord.ownedPawns
				where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
				select x;
			}
			if (source.Any<Pawn>())
			{
				if (!source.RandomElement<Pawn>().inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		
		private bool ReachableForTrade(Thing thing)
		{
			return this.pawn.Map == thing.Map && this.pawn.Map.reachability.CanReach(this.pawn.Position, thing, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
		}

		
		private Pawn pawn;

		
		public TraderKindDef traderKind;

		
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
