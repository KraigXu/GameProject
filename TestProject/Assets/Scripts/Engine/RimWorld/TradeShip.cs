using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCC RID: 3532
	public class TradeShip : PassingShip, ITrader, IThingHolder
	{
		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x060055AD RID: 21933 RVA: 0x001C719C File Offset: 0x001C539C
		public override string FullTitle
		{
			get
			{
				return this.name + " (" + this.def.label + ")";
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x060055AE RID: 21934 RVA: 0x001C71BE File Offset: 0x001C53BE
		public int Silver
		{
			get
			{
				return this.CountHeldOf(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x060055AF RID: 21935 RVA: 0x001C71CC File Offset: 0x001C53CC
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.def.tradeCurrency;
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x060055B0 RID: 21936 RVA: 0x001C71D9 File Offset: 0x001C53D9
		public IThingHolder ParentHolder
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x060055B1 RID: 21937 RVA: 0x001C71E1 File Offset: 0x001C53E1
		public TraderKindDef TraderKind
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x060055B2 RID: 21938 RVA: 0x001C71E9 File Offset: 0x001C53E9
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.randomPriceFactorSeed;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x060055B3 RID: 21939 RVA: 0x001C49FB File Offset: 0x001C2BFB
		public string TraderName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x060055B4 RID: 21940 RVA: 0x001C71F1 File Offset: 0x001C53F1
		public bool CanTradeNow
		{
			get
			{
				return !base.Departed;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x060055B5 RID: 21941 RVA: 0x0005AC15 File Offset: 0x00058E15
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060055B6 RID: 21942 RVA: 0x001C71FC File Offset: 0x001C53FC
		public IEnumerable<Thing> Goods
		{
			get
			{
				int num;
				for (int i = 0; i < this.things.Count; i = num + 1)
				{
					Pawn pawn = this.things[i] as Pawn;
					if (pawn == null || !this.soldPrisoners.Contains(pawn))
					{
						yield return this.things[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x001C720C File Offset: 0x001C540C
		public TradeShip()
		{
		}

		// Token: 0x060055B8 RID: 21944 RVA: 0x001C7228 File Offset: 0x001C5428
		public TradeShip(TraderKindDef def, Faction faction = null) : base(faction)
		{
			this.def = def;
			this.things = new ThingOwner<Thing>(this);
			TradeShip.tmpExtantNames.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				TradeShip.tmpExtantNames.AddRange(from x in maps[i].passingShipManager.passingShips
				select x.name);
			}
			this.name = NameGenerator.GenerateName(RulePackDefOf.NamerTraderGeneral, TradeShip.tmpExtantNames, false, null);
			if (faction != null)
			{
				this.name = string.Format("{0} {1} {2}", this.name, "OfLower".Translate(), faction.Name);
			}
			this.randomPriceFactorSeed = Rand.RangeInclusive(1, 10000000);
			this.loadID = Find.UniqueIDsManager.GetNextPassingShipID();
		}

		// Token: 0x060055B9 RID: 21945 RVA: 0x001C7327 File Offset: 0x001C5527
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			foreach (Thing thing in TradeUtility.AllLaunchableThingsForTrade(base.Map, this))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			foreach (Pawn pawn in TradeUtility.AllSellableColonyPawns(base.Map))
			{
				yield return pawn;
			}
			IEnumerator<Pawn> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x060055BA RID: 21946 RVA: 0x001C7338 File Offset: 0x001C5538
		public void GenerateThings()
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = this.def;
			parms.tile = new int?(base.Map.Tile);
			this.things.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x001C7390 File Offset: 0x001C5590
		public override void PassingShipTick()
		{
			base.PassingShipTick();
			for (int i = this.things.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.things[i] as Pawn;
				if (pawn != null)
				{
					pawn.Tick();
					if (pawn.Dead)
					{
						this.things.Remove(pawn);
					}
				}
			}
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x001C73EC File Offset: 0x001C55EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraderKindDef>(ref this.def, "def");
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.randomPriceFactorSeed, "randomPriceFactorSeed", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060055BD RID: 21949 RVA: 0x001C7484 File Offset: 0x001C5684
		public override void TryOpenComms(Pawn negotiator)
		{
			if (!this.CanTradeNow)
			{
				return;
			}
			Find.WindowStack.Add(new Dialog_Trade(negotiator, this, false));
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.Critical);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(this.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradeShip".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, false, true);
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradeGoodsMustBeNearBeacon, Array.Empty<string>());
		}

		// Token: 0x060055BE RID: 21950 RVA: 0x001C7500 File Offset: 0x001C5700
		public override void Depart()
		{
			base.Depart();
			this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			this.soldPrisoners.Clear();
		}

		// Token: 0x060055BF RID: 21951 RVA: 0x001C719C File Offset: 0x001C539C
		public override string GetCallLabel()
		{
			return this.name + " (" + this.def.label + ")";
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x001C7520 File Offset: 0x001C5720
		protected override AcceptanceReport CanCommunicateWith_NewTemp(Pawn negotiator)
		{
			AcceptanceReport result = base.CanCommunicateWith_NewTemp(negotiator);
			if (!result.Accepted)
			{
				return result;
			}
			return negotiator.CanTradeWith_NewTemp(base.Faction, this.TraderKind);
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x001C7552 File Offset: 0x001C5752
		protected override bool CanCommunicateWith(Pawn negotiator)
		{
			return base.CanCommunicateWith(negotiator) && negotiator.CanTradeWith(base.Faction, this.TraderKind);
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x001C7574 File Offset: 0x001C5774
		public int CountHeldOf(ThingDef thingDef, ThingDef stuffDef = null)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing != null)
			{
				return thing.stackCount;
			}
			return 0;
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x001C7598 File Offset: 0x001C5798
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this, thing);
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
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(pawn);
				}
				this.things.TryAdd(thing, false);
			}
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x001C7604 File Offset: 0x001C5804
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.soldPrisoners.Remove(pawn);
			}
			TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(base.Map), base.Map, thing);
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x001C7650 File Offset: 0x001C5850
		private Thing HeldThingMatching(ThingDef thingDef, ThingDef stuffDef)
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].def == thingDef && this.things[i].Stuff == stuffDef)
				{
					return this.things[i];
				}
			}
			return null;
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x001C76A9 File Offset: 0x001C58A9
		public void ChangeCountHeldOf(ThingDef thingDef, ThingDef stuffDef, int count)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing == null)
			{
				Log.Error("Changing count of thing trader doesn't have: " + thingDef, false);
			}
			thing.stackCount += count;
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x001C4A03 File Offset: 0x001C2C03
		public override string ToString()
		{
			return this.FullTitle;
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x001C76D4 File Offset: 0x001C58D4
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x001C76DC File Offset: 0x001C58DC
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04002EDA RID: 11994
		public TraderKindDef def;

		// Token: 0x04002EDB RID: 11995
		private ThingOwner things;

		// Token: 0x04002EDC RID: 11996
		private List<Pawn> soldPrisoners = new List<Pawn>();

		// Token: 0x04002EDD RID: 11997
		private int randomPriceFactorSeed = -1;

		// Token: 0x04002EDE RID: 11998
		private static List<string> tmpExtantNames = new List<string>();
	}
}
