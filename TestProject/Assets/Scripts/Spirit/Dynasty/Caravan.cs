﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001226 RID: 4646
	[StaticConstructorOnStartup]
	public class Caravan : WorldObject, IThingHolder, IIncidentTarget, ILoadReferenceable, ITrader
	{
		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06006BCB RID: 27595 RVA: 0x00259A45 File Offset: 0x00257C45
		public List<Pawn> PawnsListForReading
		{
			get
			{
				return this.pawns.InnerListForReading;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06006BCC RID: 27596 RVA: 0x00259A54 File Offset: 0x00257C54
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction == null)
					{
						color = Color.white;
					}
					else if (base.Faction.IsPlayer)
					{
						color = Caravan.PlayerCaravanColor;
					}
					else
					{
						color = base.Faction.Color;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.DynamicObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06006BCD RID: 27597 RVA: 0x00259AC7 File Offset: 0x00257CC7
		// (set) Token: 0x06006BCE RID: 27598 RVA: 0x00259ACF File Offset: 0x00257CCF
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06006BCF RID: 27599 RVA: 0x00259AD8 File Offset: 0x00257CD8
		public override Vector3 DrawPos
		{
			get
			{
				return this.tweener.TweenedPos;
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x00259AE5 File Offset: 0x00257CE5
		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06006BD1 RID: 27601 RVA: 0x00259AF4 File Offset: 0x00257CF4
		public bool ImmobilizedByMass
		{
			get
			{
				if (Find.TickManager.TicksGame - this.cachedImmobilizedForTicks < 60)
				{
					return this.cachedImmobilized;
				}
				this.cachedImmobilized = (this.MassUsage > this.MassCapacity);
				this.cachedImmobilizedForTicks = Find.TickManager.TicksGame;
				return this.cachedImmobilized;
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06006BD2 RID: 27602 RVA: 0x00259B48 File Offset: 0x00257D48
		public Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (Find.TickManager.TicksGame - this.cachedDaysWorthOfFoodForTicks < 3000)
				{
					return this.cachedDaysWorthOfFood;
				}
				this.cachedDaysWorthOfFood = new Pair<float, float>(DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this), DaysUntilRotCalculator.ApproxDaysUntilRot(this));
				this.cachedDaysWorthOfFoodForTicks = Find.TickManager.TicksGame;
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06006BD3 RID: 27603 RVA: 0x00259BA1 File Offset: 0x00257DA1
		public bool CantMove
		{
			get
			{
				return this.NightResting || this.AllOwnersHaveMentalBreak || this.AllOwnersDowned || this.ImmobilizedByMass;
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06006BD4 RID: 27604 RVA: 0x00259BC3 File Offset: 0x00257DC3
		public float MassCapacity
		{
			get
			{
				return CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, null);
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06006BD5 RID: 27605 RVA: 0x00259BD4 File Offset: 0x00257DD4
		public string MassCapacityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06006BD6 RID: 27606 RVA: 0x00259BFA File Offset: 0x00257DFA
		public float MassUsage
		{
			get
			{
				return CollectionsMassCalculator.MassUsage<Pawn>(this.PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore, false, false);
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06006BD7 RID: 27607 RVA: 0x00259C0C File Offset: 0x00257E0C
		public bool AllOwnersDowned
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].Downed)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06006BD8 RID: 27608 RVA: 0x00259C5C File Offset: 0x00257E5C
		public bool AllOwnersHaveMentalBreak
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].InMentalState)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06006BD9 RID: 27609 RVA: 0x00259CAC File Offset: 0x00257EAC
		public bool NightResting
		{
			get
			{
				return base.Spawned && (!this.pather.Moving || this.pather.nextTile != this.pather.Destination || !Caravan_PathFollower.IsValidFinalPushDestination(this.pather.Destination) || Mathf.CeilToInt(this.pather.nextTileCostLeft / 1f) > 10000) && CaravanNightRestUtility.RestingNowAt(base.Tile);
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06006BDA RID: 27610 RVA: 0x00259D24 File Offset: 0x00257F24
		public int LeftRestTicks
		{
			get
			{
				if (!this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftRestTicksAt(base.Tile);
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x06006BDB RID: 27611 RVA: 0x00259D3B File Offset: 0x00257F3B
		public int LeftNonRestTicks
		{
			get
			{
				if (this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftNonRestTicksAt(base.Tile);
			}
		}

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06006BDC RID: 27612 RVA: 0x00259D52 File Offset: 0x00257F52
		public override string Label
		{
			get
			{
				if (this.nameInt != null)
				{
					return this.nameInt;
				}
				return base.Label;
			}
		}

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06006BDD RID: 27613 RVA: 0x00259D69 File Offset: 0x00257F69
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06006BDE RID: 27614 RVA: 0x00259D79 File Offset: 0x00257F79
		public int TicksPerMove
		{
			get
			{
				return CaravanTicksPerMoveUtility.GetTicksPerMove(this, null);
			}
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06006BDF RID: 27615 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AppendFactionToInspectString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06006BE0 RID: 27616 RVA: 0x00259D82 File Offset: 0x00257F82
		public float Visibility
		{
			get
			{
				return CaravanVisibilityCalculator.Visibility(this, null);
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06006BE1 RID: 27617 RVA: 0x00259D8C File Offset: 0x00257F8C
		public string VisibilityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanVisibilityCalculator.Visibility(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06006BE2 RID: 27618 RVA: 0x00259DB0 File Offset: 0x00257FB0
		public string TicksPerMoveExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanTicksPerMoveUtility.GetTicksPerMove(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06006BE3 RID: 27619 RVA: 0x00259DD1 File Offset: 0x00257FD1
		public IEnumerable<Thing> AllThings
		{
			get
			{
				return CaravanInventoryUtility.AllInventoryItems(this).Concat(this.pawns);
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06006BE4 RID: 27620 RVA: 0x00259DE4 File Offset: 0x00257FE4
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueId ^ 728241121;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06006BE5 RID: 27621 RVA: 0x00259DF2 File Offset: 0x00257FF2
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06006BE6 RID: 27622 RVA: 0x00259DFA File Offset: 0x00257FFA
		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050, false);
				return null;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06006BE7 RID: 27623 RVA: 0x00259E10 File Offset: 0x00258010
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return 0f;
				}
				float num = 0f;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(this.pawns[i]);
					if (this.pawns[i].Faction == Faction.OfPlayer)
					{
						num += this.pawns[i].MarketValue;
					}
				}
				return num * 0.7f;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06006BE8 RID: 27624 RVA: 0x00259E8E File Offset: 0x0025808E
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from x in this.PawnsListForReading
				where x.Faction == Faction.OfPlayer
				select x;
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06006BE9 RID: 27625 RVA: 0x00259EC8 File Offset: 0x002580C8
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.CaravanPointsRandomFactorRange;
			}
		}

		// Token: 0x06006BEA RID: 27626 RVA: 0x00259ED0 File Offset: 0x002580D0
		public void SetUniqueId(int newId)
		{
			if (this.uniqueId != -1 || newId < 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to set caravan with uniqueId ",
					this.uniqueId,
					" to have uniqueId ",
					newId
				}), false);
			}
			this.uniqueId = newId;
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06006BEB RID: 27627 RVA: 0x00259F29 File Offset: 0x00258129
		public TraderKindDef TraderKind
		{
			get
			{
				return this.trader.TraderKind;
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06006BEC RID: 27628 RVA: 0x00259F36 File Offset: 0x00258136
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06006BED RID: 27629 RVA: 0x00259F43 File Offset: 0x00258143
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06006BEE RID: 27630 RVA: 0x00259F50 File Offset: 0x00258150
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06006BEF RID: 27631 RVA: 0x00259F5D File Offset: 0x0025815D
		public bool CanTradeNow
		{
			get
			{
				return this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06006BF0 RID: 27632 RVA: 0x0005AC15 File Offset: 0x00058E15
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06006BF1 RID: 27633 RVA: 0x00259F6A File Offset: 0x0025816A
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x06006BF2 RID: 27634 RVA: 0x00259F77 File Offset: 0x00258177
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x00259F85 File Offset: 0x00258185
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x00259F95 File Offset: 0x00258195
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06006BF5 RID: 27637 RVA: 0x00259FA8 File Offset: 0x002581A8
		public Caravan()
		{
			this.pawns = new ThingOwner<Pawn>(this, false, LookMode.Reference);
			this.pather = new Caravan_PathFollower(this);
			this.gotoMote = new Caravan_GotoMoteRenderer();
			this.tweener = new Caravan_Tweener(this);
			this.trader = new Caravan_TraderTracker(this);
			this.forage = new Caravan_ForageTracker(this);
			this.needs = new Caravan_NeedsTracker(this);
			this.carryTracker = new Caravan_CarryTracker(this);
			this.beds = new Caravan_BedsTracker(this);
			this.storyState = new StoryState(this);
		}

		// Token: 0x06006BF6 RID: 27638 RVA: 0x0025A054 File Offset: 0x00258254
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawns, "pawns", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.autoJoinable, "autoJoinable", false, false);
			Scribe_Deep.Look<Caravan_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_ForageTracker>(ref this.forage, "forage", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_BedsTracker>(ref this.beds, "beds", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06006BF7 RID: 27639 RVA: 0x0025A1A6 File Offset: 0x002583A6
		public override void PostAdd()
		{
			base.PostAdd();
			this.carryTracker.Notify_CaravanSpawned();
			this.beds.Notify_CaravanSpawned();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06006BF8 RID: 27640 RVA: 0x0025A1CE File Offset: 0x002583CE
		public override void PostRemove()
		{
			base.PostRemove();
			this.pather.StopDead();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06006BF9 RID: 27641 RVA: 0x0025A1EC File Offset: 0x002583EC
		public override void Tick()
		{
			base.Tick();
			this.CheckAnyNonWorldPawns();
			this.pather.PatherTick();
			this.tweener.TweenerTick();
			this.forage.ForageTrackerTick();
			this.carryTracker.CarryTrackerTick();
			this.beds.BedsTrackerTick();
			this.needs.NeedsTrackerTick();
			CaravanDrugPolicyUtility.CheckTakeScheduledDrugs(this);
			CaravanTendUtility.CheckTend(this);
		}

		// Token: 0x06006BFA RID: 27642 RVA: 0x0025A253 File Offset: 0x00258453
		public override void SpawnSetup()
		{
			base.SpawnSetup();
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x06006BFB RID: 27643 RVA: 0x0025A266 File Offset: 0x00258466
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled && this.pather.curPath != null)
			{
				this.pather.curPath.DrawPath(this);
			}
			this.gotoMote.RenderMote();
		}

		// Token: 0x06006BFC RID: 27644 RVA: 0x0025A2A0 File Offset: 0x002584A0
		public void AddPawn(Pawn p, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (p == null)
			{
				Log.Warning("Tried to add a null pawn to " + this, false);
				return;
			}
			if (p.Dead)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add ",
					p,
					" to ",
					this,
					", but this pawn is dead."
				}), false);
				return;
			}
			Pawn pawn = p.carryTracker.CarriedThing as Pawn;
			if (pawn != null)
			{
				p.carryTracker.innerContainer.Remove(pawn);
			}
			if (p.Spawned)
			{
				p.DeSpawn(DestroyMode.Vanish);
			}
			if (this.pawns.TryAdd(p, true))
			{
				if (this.ShouldAutoCapture(p))
				{
					p.guest.CapturedBy(base.Faction, null);
				}
				if (pawn != null)
				{
					if (this.ShouldAutoCapture(pawn))
					{
						pawn.guest.CapturedBy(base.Faction, p);
					}
					this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
					if (addCarriedPawnToWorldPawnsIfAny)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return;
					}
				}
			}
			else
			{
				Log.Error("Couldn't add pawn " + p + " to caravan.", false);
			}
		}

		// Token: 0x06006BFD RID: 27645 RVA: 0x0025A3A8 File Offset: 0x002585A8
		public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (thing == null)
			{
				Log.Warning("Tried to add a null thing to " + this, false);
				return;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
				return;
			}
			CaravanInventoryUtility.GiveThing(this, thing);
		}

		// Token: 0x06006BFE RID: 27646 RVA: 0x0025A3E4 File Offset: 0x002585E4
		public bool ContainsPawn(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006BFF RID: 27647 RVA: 0x0025A3F2 File Offset: 0x002585F2
		public void RemovePawn(Pawn p)
		{
			this.pawns.Remove(p);
		}

		// Token: 0x06006C00 RID: 27648 RVA: 0x0025A401 File Offset: 0x00258601
		public void RemoveAllPawns()
		{
			this.pawns.Clear();
		}

		// Token: 0x06006C01 RID: 27649 RVA: 0x0025A40E File Offset: 0x0025860E
		public bool IsOwner(Pawn p)
		{
			return this.pawns.Contains(p) && CaravanUtility.IsOwner(p, base.Faction);
		}

		// Token: 0x06006C02 RID: 27650 RVA: 0x0025A42C File Offset: 0x0025862C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].IsColonist)
				{
					num++;
				}
				else if (this.pawns[i].RaceProps.Animal)
				{
					num2++;
				}
				else if (this.pawns[i].IsPrisoner)
				{
					num3++;
				}
				if (this.pawns[i].Downed)
				{
					num4++;
				}
				if (this.pawns[i].InMentalState)
				{
					num5++;
				}
			}
			stringBuilder.Append("CaravanColonistsCount".Translate(num, (num == 1) ? Faction.OfPlayer.def.pawnSingular : Faction.OfPlayer.def.pawnsPlural));
			if (num2 == 1)
			{
				stringBuilder.Append(", " + "CaravanAnimal".Translate());
			}
			else if (num2 > 1)
			{
				stringBuilder.Append(", " + "CaravanAnimalsCount".Translate(num2));
			}
			if (num3 == 1)
			{
				stringBuilder.Append(", " + "CaravanPrisoner".Translate());
			}
			else if (num3 > 1)
			{
				stringBuilder.Append(", " + "CaravanPrisonersCount".Translate(num3));
			}
			stringBuilder.AppendLine();
			if (num5 > 0)
			{
				stringBuilder.Append("CaravanPawnsInMentalState".Translate(num5));
			}
			if (num4 > 0)
			{
				if (num5 > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("CaravanPawnsDowned".Translate(num4));
			}
			if (num5 > 0 || num4 > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.pather.Moving)
			{
				if (this.pather.ArrivalAction != null)
				{
					stringBuilder.Append(this.pather.ArrivalAction.ReportString);
				}
				else
				{
					stringBuilder.Append("CaravanTraveling".Translate());
				}
			}
			else
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(this);
				if (settlement != null)
				{
					stringBuilder.Append("CaravanVisiting".Translate(settlement.Label));
				}
				else
				{
					stringBuilder.Append("CaravanWaiting".Translate());
				}
			}
			if (this.pather.Moving)
			{
				float num6 = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this, true) / 60000f;
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanEstimatedTimeToDestination".Translate(num6.ToString("0.#")));
			}
			if (this.AllOwnersDowned)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersDowned".Translate());
			}
			else if (this.AllOwnersHaveMentalBreak)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersMentalBreak".Translate());
			}
			else if (this.ImmobilizedByMass)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanImmobilizedByMass".Translate());
			}
			string text;
			if (this.needs.AnyPawnOutOfFood(out text))
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanOutOfFood".Translate());
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(text);
					stringBuilder.Append(".");
				}
			}
			if (!this.pather.MovingNow)
			{
				int usedBedCount = this.beds.GetUsedBedCount();
				stringBuilder.AppendLine();
				stringBuilder.Append(CaravanBedUtility.AppendUsingBedsLabel("CaravanResting".Translate(), usedBedCount));
			}
			else
			{
				string inspectStringLine = this.carryTracker.GetInspectStringLine();
				if (!inspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inspectStringLine);
				}
				string inBedForMedicalReasonsInspectStringLine = this.beds.GetInBedForMedicalReasonsInspectStringLine();
				if (!inBedForMedicalReasonsInspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inBedForMedicalReasonsInspectStringLine);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x0025A895 File Offset: 0x00258A95
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return new Gizmo_CaravanInfo(this);
			}
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.IsPlayerControlled)
			{
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					yield return SettleInEmptyTileUtility.SettleCommand(this);
				}
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					if (this.PawnsListForReading.Count((Pawn x) => x.IsColonist) >= 2)
					{
						yield return new Command_Action
						{
							defaultLabel = "CommandSplitCaravan".Translate(),
							defaultDesc = "CommandSplitCaravanDesc".Translate(),
							icon = Caravan.SplitCommand,
							hotKey = KeyBindingDefOf.Misc5,
							action = delegate
							{
								Find.WindowStack.Add(new Dialog_SplitCaravan(this));
							}
						};
					}
				}
				if (this.pather.Moving)
				{
					yield return new Command_Toggle
					{
						hotKey = KeyBindingDefOf.Misc1,
						isActive = (() => this.pather.Paused),
						toggleAction = delegate
						{
							if (!this.pather.Moving)
							{
								return;
							}
							this.pather.Paused = !this.pather.Paused;
						},
						defaultDesc = "CommandToggleCaravanPauseDesc".Translate(2f.ToString("0.#"), 0.3f.ToStringPercent()),
						icon = TexCommand.PauseCaravan,
						defaultLabel = "CommandPauseCaravan".Translate()
					};
				}
				if (CaravanMergeUtility.ShouldShowMergeCommand)
				{
					yield return CaravanMergeUtility.MergeCommand(this);
				}
				foreach (Gizmo gizmo2 in this.forage.GetGizmos())
				{
					yield return gizmo2;
				}
				enumerator = null;
				foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(base.Tile))
				{
					foreach (Gizmo gizmo3 in worldObject.GetCaravanGizmos(this))
					{
						yield return gizmo3;
					}
					enumerator = null;
				}
				IEnumerator<WorldObject> enumerator2 = null;
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Mental break",
					action = delegate
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where x.RaceProps.Humanlike && !x.InMentalState
						select x).TryRandomElement(out pawn))
						{
							pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, null, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Make random pawn hungry",
					action = delegate
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where x.needs.food != null
						select x).TryRandomElement(out pawn))
						{
							pawn.needs.food.CurLevelPercentage = 0f;
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Kill random pawn",
					action = delegate
					{
						Pawn pawn;
						if (this.PawnsListForReading.TryRandomElement(out pawn))
						{
							pawn.Kill(null, null);
							Messages.Message("Dev: Killed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Harm random pawn",
					action = delegate
					{
						Pawn pawn;
						if (this.PawnsListForReading.TryRandomElement(out pawn))
						{
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
							pawn.TakeDamage(dinfo);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Down random pawn",
					action = delegate
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where !x.Downed
						select x).TryRandomElement(out pawn))
						{
							HealthUtility.DamageUntilDowned(pawn, true);
							Messages.Message("Dev: Downed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Plague on random pawn",
					action = delegate
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where !x.Downed
						select x).TryRandomElement(out pawn))
						{
							Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Plague, pawn, null);
							hediff.Severity = HediffDefOf.Plague.stages[1].minSeverity - 0.001f;
							pawn.health.AddHediff(hediff, null, null, null);
							Messages.Message("Dev: Gave advanced plague to " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Teleport to destination",
					action = delegate
					{
						base.Tile = this.pather.Destination;
						this.pather.StopDead();
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x0025A8A5 File Offset: 0x00258AA5
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in this.<>n__1(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalAction_GiveToCaravan.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x0025A8C3 File Offset: 0x00258AC3
		public void RecacheImmobilizedNow()
		{
			this.cachedImmobilizedForTicks = -99999;
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x0025A8D0 File Offset: 0x00258AD0
		public void RecacheDaysWorthOfFood()
		{
			this.cachedDaysWorthOfFoodForTicks = -99999;
		}

		// Token: 0x06006C07 RID: 27655 RVA: 0x0025A8E0 File Offset: 0x00258AE0
		public virtual void Notify_MemberDied(Pawn member)
		{
			if (!base.Spawned)
			{
				Log.Error("Caravan member died in an unspawned caravan. Unspawned caravans shouldn't be kept for more than a single frame.", false);
			}
			if (!this.PawnsListForReading.Any((Pawn x) => x != member && this.IsOwner(x)))
			{
				this.RemovePawn(member);
				if (base.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null);
				}
				this.pawns.Clear();
				this.Destroy();
				return;
			}
			member.Strip();
			this.RemovePawn(member);
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x0025A9B9 File Offset: 0x00258BB9
		public virtual void Notify_Merged(List<Caravan> group)
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x06006C09 RID: 27657 RVA: 0x0025A9B9 File Offset: 0x00258BB9
		public virtual void Notify_StartedTrading()
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x0025A9C4 File Offset: 0x00258BC4
		private void CheckAnyNonWorldPawns()
		{
			for (int i = this.pawns.Count - 1; i >= 0; i--)
			{
				if (!this.pawns[i].IsWorldPawn())
				{
					Log.Error("Caravan member " + this.pawns[i] + " is not a world pawn. Removing...", false);
					this.pawns.Remove(this.pawns[i]);
				}
			}
		}

		// Token: 0x06006C0B RID: 27659 RVA: 0x0025AA35 File Offset: 0x00258C35
		private bool ShouldAutoCapture(Pawn p)
		{
			return CaravanUtility.ShouldAutoCapture(p, base.Faction);
		}

		// Token: 0x06006C0C RID: 27660 RVA: 0x0025AA43 File Offset: 0x00258C43
		public void Notify_PawnRemoved(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
			this.carryTracker.Notify_PawnRemoved();
			this.beds.Notify_PawnRemoved();
		}

		// Token: 0x06006C0D RID: 27661 RVA: 0x0025AA71 File Offset: 0x00258C71
		public void Notify_PawnAdded(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
		}

		// Token: 0x06006C0E RID: 27662 RVA: 0x0025AA89 File Offset: 0x00258C89
		public void Notify_DestinationOrPauseStatusChanged()
		{
			this.RecacheDaysWorthOfFood();
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x0025AA91 File Offset: 0x00258C91
		public void Notify_Teleported()
		{
			this.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
		}

		// Token: 0x06006C10 RID: 27664 RVA: 0x0025AAA9 File Offset: 0x00258CA9
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawns;
		}

		// Token: 0x06006C11 RID: 27665 RVA: 0x0025AAB1 File Offset: 0x00258CB1
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0400433D RID: 17213
		private int uniqueId = -1;

		// Token: 0x0400433E RID: 17214
		private string nameInt;

		// Token: 0x0400433F RID: 17215
		public ThingOwner<Pawn> pawns;

		// Token: 0x04004340 RID: 17216
		public bool autoJoinable;

		// Token: 0x04004341 RID: 17217
		public Caravan_PathFollower pather;

		// Token: 0x04004342 RID: 17218
		public Caravan_GotoMoteRenderer gotoMote;

		// Token: 0x04004343 RID: 17219
		public Caravan_Tweener tweener;

		// Token: 0x04004344 RID: 17220
		public Caravan_TraderTracker trader;

		// Token: 0x04004345 RID: 17221
		public Caravan_ForageTracker forage;

		// Token: 0x04004346 RID: 17222
		public Caravan_NeedsTracker needs;

		// Token: 0x04004347 RID: 17223
		public Caravan_CarryTracker carryTracker;

		// Token: 0x04004348 RID: 17224
		public Caravan_BedsTracker beds;

		// Token: 0x04004349 RID: 17225
		public StoryState storyState;

		// Token: 0x0400434A RID: 17226
		private Material cachedMat;

		// Token: 0x0400434B RID: 17227
		private bool cachedImmobilized;

		// Token: 0x0400434C RID: 17228
		private int cachedImmobilizedForTicks = -99999;

		// Token: 0x0400434D RID: 17229
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x0400434E RID: 17230
		private int cachedDaysWorthOfFoodForTicks = -99999;

		// Token: 0x0400434F RID: 17231
		public bool notifiedOutOfFood;

		// Token: 0x04004350 RID: 17232
		private const int ImmobilizedCacheDuration = 60;

		// Token: 0x04004351 RID: 17233
		private const int DaysWorthOfFoodCacheDuration = 3000;

		// Token: 0x04004352 RID: 17234
		private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan", true);

		// Token: 0x04004353 RID: 17235
		private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);
	}
}
