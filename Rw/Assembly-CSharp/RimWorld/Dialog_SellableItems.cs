using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFE RID: 3838
	public class Dialog_SellableItems : Window
	{
		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x06005E18 RID: 24088 RVA: 0x00208920 File Offset: 0x00206B20
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(650f, (float)Mathf.Min(UI.screenHeight, 1000));
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06005E19 RID: 24089 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x0020893C File Offset: 0x00206B3C
		public Dialog_SellableItems(ITrader trader)
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.trader = trader;
			this.CalculateSellableItems(trader.TraderKind);
			this.CalculateTabs();
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x002089AC File Offset: 0x00206BAC
		public override void DoWindowContents(Rect inRect)
		{
			float num = 40f;
			Rect rect = new Rect(0f, 0f, inRect.width, 40f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SellableItemsTitle".Translate().CapitalizeFirst());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			ITraderRestockingInfoProvider traderRestockingInfoProvider = this.trader as ITraderRestockingInfoProvider;
			if (traderRestockingInfoProvider != null)
			{
				int nextRestockTick = traderRestockingInfoProvider.NextRestockTick;
				if (nextRestockTick != -1)
				{
					float num2 = (nextRestockTick - Find.TickManager.TicksGame).TicksToDays();
					Widgets.Label(new Rect(0f, num, inRect.width, 20f), "NextTraderRestock".Translate(num2.ToString("0.0")));
					num += 20f;
				}
				else if (!traderRestockingInfoProvider.EverVisited)
				{
					Widgets.Label(new Rect(0f, num, inRect.width, 20f), "TraderNotVisitedYet".Translate());
					num += 20f;
				}
				else if (traderRestockingInfoProvider.RestockedSinceLastVisit)
				{
					Widgets.Label(new Rect(0f, num, inRect.width, 20f), "TraderRestockedSinceLastVisit".Translate());
					num += 20f;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			inRect.yMin += 64f + num;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, this.tabs, 2);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect outRect = rect2;
			outRect.yMax -= 65f;
			List<ThingDef> sellableItemsInCategory = this.GetSellableItemsInCategory(this.currentCategory, this.pawnsTabOpen);
			if (sellableItemsInCategory.Any<ThingDef>())
			{
				float height = (float)sellableItemsInCategory.Count * 24f;
				num = 0f;
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
				Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
				float num3 = this.scrollPosition.y - 24f;
				float num4 = this.scrollPosition.y + outRect.height;
				for (int i = 0; i < sellableItemsInCategory.Count; i++)
				{
					if (num > num3 && num < num4)
					{
						Widgets.DefLabelWithIcon(new Rect(0f, num, viewRect.width, 24f), sellableItemsInCategory[i], 2f, 6f);
					}
					num += 24f;
				}
				Widgets.EndScrollView();
			}
			else
			{
				Widgets.NoneLabel(0f, outRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x00208C54 File Offset: 0x00206E54
		private void DoBottomButtons(Rect rect)
		{
			if (Widgets.ButtonText(new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y), "CloseButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x00208CC8 File Offset: 0x00206EC8
		private void CalculateSellableItems(TraderKindDef trader)
		{
			this.sellableItems.Clear();
			this.cachedSellableItemsByCategory.Clear();
			this.cachedSellablePawns = null;
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].PlayerAcquirable && !allDefsListForReading[i].IsCorpse && !typeof(MinifiedThing).IsAssignableFrom(allDefsListForReading[i].thingClass) && trader.WillTrade(allDefsListForReading[i]) && TradeUtility.EverPlayerSellable(allDefsListForReading[i]))
				{
					this.sellableItems.Add(allDefsListForReading[i]);
				}
			}
			this.sellableItems.SortBy((ThingDef x) => x.label);
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x00208D9C File Offset: 0x00206F9C
		private void CalculateTabs()
		{
			this.tabs.Clear();
			List<ThingCategoryDef> allDefsListForReading = DefDatabase<ThingCategoryDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingCategoryDef category = allDefsListForReading[i];
				if (category.parent == ThingCategoryDefOf.Root && this.AnyTraderWillEverTrade(category))
				{
					if (this.currentCategory == null)
					{
						this.currentCategory = category;
					}
					this.tabs.Add(new TabRecord(category.LabelCap, delegate
					{
						this.currentCategory = category;
						this.pawnsTabOpen = false;
					}, () => this.currentCategory == category));
				}
			}
			this.tabs.Add(new TabRecord("PawnsTabShort".Translate(), delegate
			{
				this.currentCategory = null;
				this.pawnsTabOpen = true;
			}, () => this.pawnsTabOpen));
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x00208E90 File Offset: 0x00207090
		private List<ThingDef> GetSellableItemsInCategory(ThingCategoryDef category, bool pawns)
		{
			if (pawns)
			{
				if (this.cachedSellablePawns == null)
				{
					this.cachedSellablePawns = new List<ThingDef>();
					for (int i = 0; i < this.sellableItems.Count; i++)
					{
						if (this.sellableItems[i].category == ThingCategory.Pawn)
						{
							this.cachedSellablePawns.Add(this.sellableItems[i]);
						}
					}
				}
				return this.cachedSellablePawns;
			}
			List<ThingDef> list;
			if (this.cachedSellableItemsByCategory.TryGetValue(category, out list))
			{
				return list;
			}
			list = new List<ThingDef>();
			for (int j = 0; j < this.sellableItems.Count; j++)
			{
				if (this.sellableItems[j].IsWithinCategory(category))
				{
					list.Add(this.sellableItems[j]);
				}
			}
			this.cachedSellableItemsByCategory.Add(category, list);
			return list;
		}

		// Token: 0x06005E20 RID: 24096 RVA: 0x00208F60 File Offset: 0x00207160
		private bool AnyTraderWillEverTrade(ThingCategoryDef thingCategory)
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].IsWithinCategory(thingCategory))
				{
					List<TraderKindDef> allDefsListForReading2 = DefDatabase<TraderKindDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading2.Count; j++)
					{
						if (allDefsListForReading2[j].WillTrade(allDefsListForReading[i]))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04003317 RID: 13079
		private ThingCategoryDef currentCategory;

		// Token: 0x04003318 RID: 13080
		private bool pawnsTabOpen;

		// Token: 0x04003319 RID: 13081
		private List<ThingDef> sellableItems = new List<ThingDef>();

		// Token: 0x0400331A RID: 13082
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400331B RID: 13083
		private Vector2 scrollPosition;

		// Token: 0x0400331C RID: 13084
		private ITrader trader;

		// Token: 0x0400331D RID: 13085
		private List<ThingDef> cachedSellablePawns;

		// Token: 0x0400331E RID: 13086
		private Dictionary<ThingCategoryDef, List<ThingDef>> cachedSellableItemsByCategory = new Dictionary<ThingCategoryDef, List<ThingDef>>();

		// Token: 0x0400331F RID: 13087
		private const float RowHeight = 24f;

		// Token: 0x04003320 RID: 13088
		private const float TitleRectHeight = 40f;

		// Token: 0x04003321 RID: 13089
		private const float RestockTextHeight = 20f;

		// Token: 0x04003322 RID: 13090
		private const float BottomAreaHeight = 55f;

		// Token: 0x04003323 RID: 13091
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);
	}
}
