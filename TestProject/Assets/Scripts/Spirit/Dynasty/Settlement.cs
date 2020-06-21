﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001257 RID: 4695
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader, ITraderRestockingInfoProvider
	{
		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06006D98 RID: 28056 RVA: 0x002653FB File Offset: 0x002635FB
		// (set) Token: 0x06006D99 RID: 28057 RVA: 0x00265403 File Offset: 0x00263603
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

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06006D9A RID: 28058 RVA: 0x0026540C File Offset: 0x0026360C
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06006D9B RID: 28059 RVA: 0x0026541E File Offset: 0x0026361E
		public override string Label
		{
			get
			{
				if (this.nameInt == null)
				{
					return base.Label;
				}
				return this.nameInt;
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06006D9C RID: 28060 RVA: 0x00265435 File Offset: 0x00263635
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06006D9D RID: 28061 RVA: 0x00265445 File Offset: 0x00263645
		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06006D9E RID: 28062 RVA: 0x00265450 File Offset: 0x00263650
		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer));
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06006D9F RID: 28063 RVA: 0x0026547E File Offset: 0x0026367E
		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06006DA0 RID: 28064 RVA: 0x0026547E File Offset: 0x0026367E
		public override bool ShowRelatedQuests
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06006DA1 RID: 28065 RVA: 0x00265490 File Offset: 0x00263690
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					this.cachedMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06006DA2 RID: 28066 RVA: 0x002654E1 File Offset: 0x002636E1
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				if (base.Faction == Faction.OfPlayer)
				{
					return MapGeneratorDefOf.Base_Player;
				}
				return MapGeneratorDefOf.Base_Faction;
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06006DA3 RID: 28067 RVA: 0x002654FB File Offset: 0x002636FB
		public TraderKindDef TraderKind
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.TraderKind;
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06006DA4 RID: 28068 RVA: 0x00265512 File Offset: 0x00263712
		public IEnumerable<Thing> Goods
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.StockListForReading;
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06006DA5 RID: 28069 RVA: 0x00265529 File Offset: 0x00263729
		public int RandomPriceFactorSeed
		{
			get
			{
				if (this.trader == null)
				{
					return 0;
				}
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06006DA6 RID: 28070 RVA: 0x00265540 File Offset: 0x00263740
		public string TraderName
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.TraderName;
			}
		}

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06006DA7 RID: 28071 RVA: 0x00265557 File Offset: 0x00263757
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06006DA8 RID: 28072 RVA: 0x0026556E File Offset: 0x0026376E
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				if (this.trader == null)
				{
					return 0f;
				}
				return this.trader.TradePriceImprovementOffsetForPlayer;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06006DA9 RID: 28073 RVA: 0x00265589 File Offset: 0x00263789
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x00265596 File Offset: 0x00263796
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			if (this.trader == null)
			{
				return null;
			}
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x002655AE File Offset: 0x002637AE
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06006DAC RID: 28076 RVA: 0x002655BE File Offset: 0x002637BE
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06006DAD RID: 28077 RVA: 0x002655CE File Offset: 0x002637CE
		public bool EverVisited
		{
			get
			{
				return this.trader.EverVisited;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06006DAE RID: 28078 RVA: 0x002655DB File Offset: 0x002637DB
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.trader.RestockedSinceLastVisit;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06006DAF RID: 28079 RVA: 0x002655E8 File Offset: 0x002637E8
		public int NextRestockTick
		{
			get
			{
				return this.trader.NextRestockTick;
			}
		}

		// Token: 0x06006DB0 RID: 28080 RVA: 0x002655F5 File Offset: 0x002637F5
		public Settlement()
		{
			this.trader = new Settlement_TraderTracker(this);
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x00265614 File Offset: 0x00263814
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef incidentTargetTagDef in this.<>n__0())
			{
				yield return incidentTargetTagDef;
			}
			IEnumerator<IncidentTargetTagDef> enumerator = null;
			if (base.Faction == Faction.OfPlayer)
			{
				yield return IncidentTargetTagDefOf.Map_PlayerHome;
			}
			else
			{
				yield return IncidentTargetTagDefOf.Map_Misc;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x00265624 File Offset: 0x00263824
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x002656BE File Offset: 0x002638BE
		public override void Tick()
		{
			base.Tick();
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
			SettlementDefeatUtility.CheckDefeated(this);
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x002656E0 File Offset: 0x002638E0
		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int i = this.previouslyGeneratedInhabitants.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[i];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(i);
				}
			}
		}

		// Token: 0x06006DB5 RID: 28085 RVA: 0x00265735 File Offset: 0x00263935
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		// Token: 0x06006DB6 RID: 28086 RVA: 0x0026575C File Offset: 0x0026395C
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		// Token: 0x06006DB7 RID: 28087 RVA: 0x00265778 File Offset: 0x00263978
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += base.Faction.PlayerRelationKind.GetLabel();
				if (!base.Faction.def.hidden)
				{
					text = text + " (" + base.Faction.PlayerGoodwill.ToStringWithSign() + ")";
				}
				TraderKindDef traderKind = this.TraderKind;
				RoyalTitleDef royalTitleDef = (traderKind != null) ? traderKind.TitleRequiredToTrade : null;
				if (royalTitleDef != null)
				{
					text += "\n" + "RequiresTradePermission".Translate(royalTitleDef.GetLabelCapForBothGenders());
				}
			}
			return text;
		}

		// Token: 0x06006DB8 RID: 28088 RVA: 0x0026583C File Offset: 0x00263A3C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__1())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.TraderKind != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowSellableItems".Translate(),
					defaultDesc = "CommandShowSellableItemsDesc".Translate(),
					icon = Settlement.ShowSellableItemsCommand,
					action = delegate
					{
						Find.WindowStack.Add(new Dialog_SellableItems(this));
						RoyalTitleDef titleRequiredToTrade = this.TraderKind.TitleRequiredToTrade;
						if (titleRequiredToTrade != null)
						{
							TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradingRequiresPermit, new string[]
							{
								titleRequiredToTrade.GetLabelCapForBothGenders()
							});
						}
					}
				};
			}
			if (base.Faction != Faction.OfPlayer && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan))
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandFormCaravan".Translate();
				command_Action.defaultDesc = "CommandFormCaravanDesc".Translate();
				command_Action.icon = Settlement.FormCaravanCommand;
				command_Action.action = delegate
				{
					Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
					Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, false);
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006DB9 RID: 28089 RVA: 0x0026584C File Offset: 0x00263A4C
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.CanTradeNow && CaravanVisitUtility.SettlementVisitedNow(caravan) == this)
			{
				yield return CaravanVisitUtility.TradeCommand(caravan, base.Faction, this.TraderKind);
			}
			if (CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this))
			{
				yield return FactionGiftUtility.OfferGiftsCommand(caravan, this);
			}
			foreach (Gizmo gizmo in this.<>n__2(caravan))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.Attackable)
			{
				yield return new Command_Action
				{
					icon = Settlement.AttackCommand,
					defaultLabel = "CommandAttackSettlement".Translate(),
					defaultDesc = "CommandAttackSettlementDesc".Translate(),
					action = delegate
					{
						SettlementUtility.Attack(caravan, this);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06006DBA RID: 28090 RVA: 0x00265863 File Offset: 0x00263A63
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in this.<>n__3(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (CaravanVisitUtility.SettlementVisitedNow(caravan) != this)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitSettlement.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			foreach (FloatMenuOption floatMenuOption3 in CaravanArrivalAction_Trade.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption3;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption4 in CaravanArrivalAction_OfferGifts.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption4;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption5 in CaravanArrivalAction_AttackSettlement.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption5;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006DBB RID: 28091 RVA: 0x0026587A File Offset: 0x00263A7A
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in this.<>n__4(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalAction_VisitSettlement.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption3 in TransportPodsArrivalAction_GiveGift.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption3;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption4 in TransportPodsArrivalAction_AttackSettlement.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption4;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006DBC RID: 28092 RVA: 0x00265898 File Offset: 0x00263A98
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		// Token: 0x040043EC RID: 17388
		public Settlement_TraderTracker trader;

		// Token: 0x040043ED RID: 17389
		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		// Token: 0x040043EE RID: 17390
		private string nameInt;

		// Token: 0x040043EF RID: 17391
		public bool namedByPlayer;

		// Token: 0x040043F0 RID: 17392
		private Material cachedMat;

		// Token: 0x040043F1 RID: 17393
		public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x040043F2 RID: 17394
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x040043F3 RID: 17395
		public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);
	}
}
