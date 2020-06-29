﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class Dialog_Trade : Window
	{
		
		// (get) Token: 0x06005E23 RID: 24099 RVA: 0x001D66F9 File Offset: 0x001D48F9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		
		// (get) Token: 0x06005E24 RID: 24100 RVA: 0x00208FDA File Offset: 0x002071DA
		private int Tile
		{
			get
			{
				return TradeSession.playerNegotiator.Tile;
			}
		}

		
		// (get) Token: 0x06005E25 RID: 24101 RVA: 0x00208FE6 File Offset: 0x002071E6
		private BiomeDef Biome
		{
			get
			{
				return Find.WorldGrid[this.Tile].biome;
			}
		}

		
		// (get) Token: 0x06005E26 RID: 24102 RVA: 0x00209000 File Offset: 0x00207200
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, IgnorePawnsInventoryMode.Ignore, false, false);
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassUsage;
			}
		}

		
		// (get) Token: 0x06005E27 RID: 24103 RVA: 0x00209074 File Offset: 0x00207274
		private float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassCapacity;
			}
		}

		
		// (get) Token: 0x06005E28 RID: 24104 RVA: 0x002090F8 File Offset: 0x002072F8
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.MassUsage, this.MassCapacity, this.Tile, (caravan != null && caravan.pather.Moving) ? caravan.pather.nextTile : -1, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		
		// (get) Token: 0x06005E29 RID: 24105 RVA: 0x0020918C File Offset: 0x0020738C
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore, Faction.OfPlayer);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		
		// (get) Token: 0x06005E2A RID: 24106 RVA: 0x002091FC File Offset: 0x002073FC
		private Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		
		// (get) Token: 0x06005E2B RID: 24107 RVA: 0x00209260 File Offset: 0x00207460
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		
		public Dialog_Trade(Pawn playerNegotiator, ITrader trader, bool giftsOnly = false)
		{
			this.giftsOnly = giftsOnly;
			TradeSession.SetupWith(trader, playerNegotiator, giftsOnly);
			this.SetupPlayerCaravanVariables();
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (trader is PassingShip)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		
		public override void PostOpen()
		{
			base.PostOpen();
			if (!this.giftsOnly && !this.playerIsCaravan)
			{
				Pawn playerNegotiator = TradeSession.playerNegotiator;
				float level = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Talking);
				float level2 = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Hearing);
				if (level < 0.95f || level2 < 0.95f)
				{
					TaggedString taggedString;
					if (level < 0.95f)
					{
						taggedString = "NegotiatorTalkingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					else
					{
						taggedString = "NegotiatorHearingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					taggedString += "\n\n" + "NegotiatorCapacityImpaired".Translate();
					Find.WindowStack.Add(new Dialog_MessageBox(taggedString, null, null, null, null, null, false, null, null));
				}
			}
			this.CacheTradeables();
		}

		
		private void CacheTradeables()
		{
			this.cachedCurrencyTradeable = TradeSession.deal.AllTradeables.FirstOrDefault((Tradeable x) => x.IsCurrency && (TradeSession.TradeCurrency != TradeCurrency.Favor || x.IsFavor));
			this.cachedTradeables = (from tr in TradeSession.deal.AllTradeables
			where !tr.IsCurrency && (tr.TraderWillTrade || !TradeSession.trader.TraderKind.hideThingsNotWillingToTrade)
			select tr).OrderByDescending(delegate(Tradeable tr)
			{
				if (!tr.TraderWillTrade)
				{
					return -1;
				}
				return 0;
			}).ThenBy((Tradeable tr) => tr, this.sorter1.Comparer).ThenBy((Tradeable tr) => tr, this.sorter2.Comparer).ThenBy((Tradeable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ThenBy((Tradeable tr) => tr.ThingDef.label).ThenBy(delegate(Tradeable tr)
			{
				QualityCategory result;
				if (tr.AnyThing.TryGetQuality(out result))
				{
					return (int)result;
				}
				return -1;
			}).ThenBy((Tradeable tr) => tr.AnyThing.HitPoints).ToList<Tradeable>();
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			if (this.playerIsCaravan)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.Tile, null, -9999f, new Rect(12f, 0f, inRect.width - 24f, 40f), true, null, false);
				inRect.yMin += 52f;
			}
			TradeSession.deal.UpdateCurrencyCount();
			GUI.BeginGroup(inRect);
			inRect = inRect.AtZero();
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTradeables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTradeables();
			});
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(0f, 27f, inRect.width / 2f, inRect.height / 2f), "NegotiatorTradeDialogInfo".Translate(TradeSession.playerNegotiator.Name.ToStringFull, TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true).ToStringPercent()));
			float num = inRect.width - 590f;
			Rect position = new Rect(num, 0f, inRect.width - num, 58f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(0f, 0f, position.width / 2f, position.height);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, Faction.OfPlayer.Name.Truncate(rect.width, null));
			Rect rect2 = new Rect(position.width / 2f, 0f, position.width / 2f, position.height);
			Text.Anchor = TextAnchor.UpperRight;
			string text = TradeSession.trader.TraderName;
			if (Text.CalcSize(text).x > rect2.width)
			{
				Text.Font = GameFont.Small;
				text = text.Truncate(rect2.width, null);
			}
			Widgets.Label(rect2, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(position.width / 2f, 27f, position.width / 2f, position.height / 2f), TradeSession.trader.TraderKind.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!TradeSession.giftMode)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.6f);
				Text.Font = GameFont.Tiny;
				Rect rect3 = new Rect(position.width / 2f - 100f - 30f, 0f, 200f, position.height);
				Text.Anchor = TextAnchor.LowerCenter;
				Widgets.Label(rect3, "PositiveBuysNegativeSells".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			GUI.EndGroup();
			float num2 = 0f;
			if (this.cachedCurrencyTradeable != null)
			{
				float num3 = inRect.width - 16f;
				TradeUI.DrawTradeableRow(new Rect(0f, 58f, num3, 30f), this.cachedCurrencyTradeable, 1);
				GUI.color = Color.gray;
				Widgets.DrawLineHorizontal(0f, 87f, num3);
				GUI.color = Color.white;
				num2 = 30f;
			}
			Rect mainRect = new Rect(0f, 58f + num2, inRect.width, inRect.height - 58f - 38f - num2 - 20f);
			this.FillMainRect(mainRect);
			Rect rect4 = new Rect(inRect.width / 2f - Dialog_Trade.AcceptButtonSize.x / 2f, inRect.height - 55f, Dialog_Trade.AcceptButtonSize.x, Dialog_Trade.AcceptButtonSize.y);
			if (Widgets.ButtonText(rect4, TradeSession.giftMode ? ("OfferGifts".Translate() + " (" + FactionGiftUtility.GetGoodwillChange(TradeSession.deal.AllTradeables, TradeSession.trader.Faction).ToStringWithSign() + ")") : "AcceptButton".Translate(), true, true, true))
			{
				Action action = delegate
				{
					bool flag;
					if (TradeSession.deal.TryExecute(out flag))
					{
						if (flag)
						{
							SoundDefOf.ExecuteTrade.PlayOneShotOnCamera(null);
							this.Close(false);
							return;
						}
						this.Close(true);
					}
				};
				if (TradeSession.deal.DoesTraderHaveEnoughSilver())
				{
					action();
				}
				else
				{
					this.FlashSilver();
					SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmTraderShortFunds".Translate(), action, false, null));
				}
				Event.current.Use();
			}
			if (Widgets.ButtonText(new Rect(rect4.x - 10f - Dialog_Trade.OtherBottomButtonSize.x, rect4.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "ResetButton".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				TradeSession.deal.Reset();
				this.CacheTradeables();
				this.CountToTransferChanged();
			}
			if (Widgets.ButtonText(new Rect(rect4.xMax + 10f, rect4.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
				Event.current.Use();
			}
			float y = Dialog_Trade.OtherBottomButtonSize.y;
			Rect rect5 = new Rect(inRect.width - y, rect4.y, y, y);
			if (Widgets.ButtonImageWithBG(rect5, Dialog_Trade.ShowSellableItemsIcon, new Vector2?(new Vector2(32f, 32f))))
			{
				Find.WindowStack.Add(new Dialog_SellableItems(TradeSession.trader));
			}
			TooltipHandler.TipRegionByKey(rect5, "CommandShowSellableItemsDesc");
			Faction faction = TradeSession.trader.Faction;
			if (faction != null && !this.giftsOnly && !faction.def.permanentEnemy)
			{
				Rect rect6 = new Rect(rect5.x - y - 4f, rect4.y, y, y);
				if (TradeSession.giftMode)
				{
					if (Widgets.ButtonImageWithBG(rect6, Dialog_Trade.TradeModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = false;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect6, "TradeModeTip");
				}
				else
				{
					if (Widgets.ButtonImageWithBG(rect6, Dialog_Trade.GiftModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = true;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect6, "GiftModeTip", faction.Name);
				}
			}
			GUI.EndGroup();
		}

		
		public override void Close(bool doCloseSound = true)
		{
			DragSliderManager.ForceStop();
			base.Close(doCloseSound);
		}

		
		private void FillMainRect(Rect mainRect)
		{
			Text.Font = GameFont.Small;
			float height = 6f + (float)this.cachedTradeables.Count * 30f;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, height);
			Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
			float num = 6f;
			float num2 = this.scrollPosition.y - 30f;
			float num3 = this.scrollPosition.y + mainRect.height;
			int num4 = 0;
			for (int i = 0; i < this.cachedTradeables.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					Rect rect = new Rect(0f, num, viewRect.width, 30f);
					int countToTransfer = this.cachedTradeables[i].CountToTransfer;
					TradeUI.DrawTradeableRow(rect, this.cachedTradeables[i], num4);
					if (countToTransfer != this.cachedTradeables[i].CountToTransfer)
					{
						this.CountToTransferChanged();
					}
				}
				num += 30f;
				num4++;
			}
			Widgets.EndScrollView();
		}

		
		public void FlashSilver()
		{
			Dialog_Trade.lastCurrencyFlashTime = Time.time;
		}

		
		public override bool CausesMessageBackground()
		{
			return true;
		}

		
		private void SetupPlayerCaravanVariables()
		{
			Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
			if (caravan != null)
			{
				this.playerIsCaravan = true;
				this.playerCaravanAllPawnsAndItems = new List<Thing>();
				List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					this.playerCaravanAllPawnsAndItems.Add(pawnsListForReading[i]);
				}
				this.playerCaravanAllPawnsAndItems.AddRange(CaravanInventoryUtility.AllInventoryItems(caravan));
				caravan.Notify_StartedTrading();
				return;
			}
			this.playerIsCaravan = false;
		}

		
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		
		private bool giftsOnly;

		
		private Vector2 scrollPosition = Vector2.zero;

		
		public static float lastCurrencyFlashTime = -100f;

		
		private List<Tradeable> cachedTradeables;

		
		private Tradeable cachedCurrencyTradeable;

		
		private TransferableSorterDef sorter1;

		
		private TransferableSorterDef sorter2;

		
		private bool playerIsCaravan;

		
		private List<Thing> playerCaravanAllPawnsAndItems;

		
		private bool massUsageDirty = true;

		
		private float cachedMassUsage;

		
		private bool massCapacityDirty = true;

		
		private float cachedMassCapacity;

		
		private string cachedMassCapacityExplanation;

		
		private bool tilesPerDayDirty = true;

		
		private float cachedTilesPerDay;

		
		private string cachedTilesPerDayExplanation;

		
		private bool daysWorthOfFoodDirty = true;

		
		private Pair<float, float> cachedDaysWorthOfFood;

		
		private bool foragedFoodPerDayDirty = true;

		
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		
		private string cachedForagedFoodPerDayExplanation;

		
		private bool visibilityDirty = true;

		
		private float cachedVisibility;

		
		private string cachedVisibilityExplanation;

		
		private const float TitleAreaHeight = 45f;

		
		private const float TopAreaHeight = 58f;

		
		private const float ColumnWidth = 120f;

		
		private const float FirstCommodityY = 6f;

		
		private const float RowInterval = 30f;

		
		private const float SpaceBetweenTraderNameAndTraderKind = 27f;

		
		private const float ShowSellableItemsIconSize = 32f;

		
		private const float GiftModeIconSize = 32f;

		
		private const float TradeModeIconSize = 32f;

		
		protected static readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		
		protected static readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		
		private static readonly Texture2D ShowSellableItemsIcon = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		
		private static readonly Texture2D GiftModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/GiftMode", true);

		
		private static readonly Texture2D TradeModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/TradeMode", true);
	}
}
