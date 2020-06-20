using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001293 RID: 4755
	[StaticConstructorOnStartup]
	public class WITab_Caravan_Health : WITab
	{
		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06006FF0 RID: 28656 RVA: 0x0026F60A File Offset: 0x0026D80A
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06006FF1 RID: 28657 RVA: 0x002706D8 File Offset: 0x0026E8D8
		private List<PawnCapacityDef> CapacitiesToDisplay
		{
			get
			{
				WITab_Caravan_Health.capacitiesToDisplay.Clear();
				List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].showOnCaravanHealthTab)
					{
						WITab_Caravan_Health.capacitiesToDisplay.Add(allDefsListForReading[i]);
					}
				}
				WITab_Caravan_Health.capacitiesToDisplay.SortBy((PawnCapacityDef x) => x.listOrder);
				return WITab_Caravan_Health.capacitiesToDisplay;
			}
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06006FF2 RID: 28658 RVA: 0x00270753 File Offset: 0x0026E953
		private float SpecificHealthTabWidth
		{
			get
			{
				this.EnsureSpecificHealthTabForPawnValid();
				if (this.specificHealthTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 630f;
			}
		}

		// Token: 0x06006FF3 RID: 28659 RVA: 0x00270773 File Offset: 0x0026E973
		public WITab_Caravan_Health()
		{
			this.labelKey = "TabCaravanHealth";
		}

		// Token: 0x06006FF4 RID: 28660 RVA: 0x00270788 File Offset: 0x0026E988
		protected override void FillTab()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoColumnHeaders(ref num);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06006FF5 RID: 28661 RVA: 0x0027083C File Offset: 0x0026EA3C
		protected override void UpdateSize()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.UpdateSize();
			this.size = this.GetRawSize(false);
			if (this.size.x + this.SpecificHealthTabWidth > (float)UI.screenWidth)
			{
				this.compactMode = true;
				this.size = this.GetRawSize(true);
				return;
			}
			this.compactMode = false;
		}

		// Token: 0x06006FF6 RID: 28662 RVA: 0x00270898 File Offset: 0x0026EA98
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificHealthTabForPawn = this.specificHealthTabForPawn;
			if (localSpecificHealthTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificHealthTabWidth = this.SpecificHealthTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificHealthTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificHealthTabForPawn.DestroyedOrNull())
					{
						return;
					}
					HealthCardUtility.DrawPawnHealthCard(new Rect(0f, 20f, rect.width, rect.height - 20f), localSpecificHealthTabForPawn, false, true, localSpecificHealthTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificHealthTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f);
			}
		}

		// Token: 0x06006FF7 RID: 28663 RVA: 0x00270940 File Offset: 0x0026EB40
		private void DoColumnHeaders(ref float curY)
		{
			if (!this.compactMode)
			{
				float num = 135f;
				Text.Anchor = TextAnchor.UpperCenter;
				GUI.color = Widgets.SeparatorLabelColor;
				Widgets.Label(new Rect(num, 3f, 100f, 100f), "Pain".Translate());
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Widgets.Label(new Rect(num, 3f, 100f, 100f), list[i].LabelCap.Truncate(100f, null));
					num += 100f;
				}
				Rect rect = new Rect(num + 8f, 0f, 24f, 24f);
				GUI.DrawTexture(rect, WITab_Caravan_Health.BeCarriedIfSickIcon);
				TooltipHandler.TipRegionByKey(rect, "BeCarriedIfSickTip");
				num += 40f;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x06006FF8 RID: 28664 RVA: 0x00270A34 File Offset: 0x0026EC34
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			if (this.specificHealthTabForPawn != null && !pawns.Contains(this.specificHealthTabForPawn))
			{
				this.specificHealthTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (!pawn2.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x06006FF9 RID: 28665 RVA: 0x00270B0C File Offset: 0x0026ED0C
		private Vector2 GetRawSize(bool compactMode)
		{
			float num = 100f;
			if (!compactMode)
			{
				num += 100f;
				num += (float)this.CapacitiesToDisplay.Count * 100f;
				num += 40f;
			}
			Vector2 result;
			result.x = 127f + num + 16f;
			result.y = Mathf.Min(550f, this.PaneTopY - 30f);
			return result;
		}

		// Token: 0x06006FFA RID: 28666 RVA: 0x00270B7C File Offset: 0x0026ED7C
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 50f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 50f), p);
			}
			curY += 50f;
		}

		// Token: 0x06006FFB RID: 28667 RVA: 0x00270BE4 File Offset: 0x0026EDE4
		private void DoRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificHealthTabForPawn);
			rect2.width -= 24f;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
			Rect bgRect = new Rect(rect3.xMax + 4f, 16f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float num = bgRect.xMax;
			if (!this.compactMode)
			{
				if (p.RaceProps.IsFlesh)
				{
					Rect rect4 = new Rect(num, 0f, 100f, 50f);
					this.DoPain(rect4, p);
				}
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Rect rect5 = new Rect(num, 0f, 100f, 50f);
					if ((p.RaceProps.Humanlike && !list[i].showOnHumanlikes) || (p.RaceProps.Animal && !list[i].showOnAnimals) || (p.RaceProps.IsMechanoid && !list[i].showOnMechanoids) || !PawnCapacityUtility.BodyCanEverDoCapacity(p.RaceProps.body, list[i]))
					{
						num += 100f;
					}
					else
					{
						this.DoCapacity(rect5, p, list[i]);
						num += 100f;
					}
				}
			}
			if (!this.compactMode)
			{
				Vector2 vector = new Vector2(num + 8f, 13f);
				Widgets.Checkbox(vector, ref p.health.beCarriedByCaravanIfSick, 24f, false, true, null, null);
				TooltipHandler.TipRegionByKey(new Rect(vector, new Vector2(24f, 24f)), "BeCarriedIfSickTip");
				num += 40f;
			}
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x06006FFC RID: 28668 RVA: 0x00270EC0 File Offset: 0x0026F0C0
		private void DoPain(Rect rect, Pawn pawn)
		{
			Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel(pawn);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = painLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, painLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string painTip = HealthCardUtility.GetPainTip(pawn);
				TooltipHandler.TipRegion(rect, painTip);
			}
		}

		// Token: 0x06006FFD RID: 28669 RVA: 0x00270F2C File Offset: 0x0026F12C
		private void DoCapacity(Rect rect, Pawn pawn, PawnCapacityDef capacity)
		{
			Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, capacity);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = efficiencyLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, efficiencyLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string pawnCapacityTip = HealthCardUtility.GetPawnCapacityTip(pawn, capacity);
				TooltipHandler.TipRegion(rect, pawnCapacityTip);
			}
		}

		// Token: 0x06006FFE RID: 28670 RVA: 0x00270F9A File Offset: 0x0026F19A
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificHealthTabForPawn = null;
		}

		// Token: 0x06006FFF RID: 28671 RVA: 0x00270FA9 File Offset: 0x0026F1A9
		private void EnsureSpecificHealthTabForPawnValid()
		{
			if (this.specificHealthTabForPawn != null && (this.specificHealthTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificHealthTabForPawn)))
			{
				this.specificHealthTabForPawn = null;
			}
		}

		// Token: 0x040044DD RID: 17629
		private Vector2 scrollPosition;

		// Token: 0x040044DE RID: 17630
		private float scrollViewHeight;

		// Token: 0x040044DF RID: 17631
		private Pawn specificHealthTabForPawn;

		// Token: 0x040044E0 RID: 17632
		private bool compactMode;

		// Token: 0x040044E1 RID: 17633
		private static List<PawnCapacityDef> capacitiesToDisplay = new List<PawnCapacityDef>();

		// Token: 0x040044E2 RID: 17634
		private const float RowHeight = 50f;

		// Token: 0x040044E3 RID: 17635
		private const float PawnLabelHeight = 18f;

		// Token: 0x040044E4 RID: 17636
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x040044E5 RID: 17637
		private const float SpaceAroundIcon = 4f;

		// Token: 0x040044E6 RID: 17638
		private const float PawnCapacityColumnWidth = 100f;

		// Token: 0x040044E7 RID: 17639
		private const float BeCarriedIfSickColumnWidth = 40f;

		// Token: 0x040044E8 RID: 17640
		private const float BeCarriedIfSickIconSize = 24f;

		// Token: 0x040044E9 RID: 17641
		private static readonly Texture2D BeCarriedIfSickIcon = ContentFinder<Texture2D>.Get("UI/Icons/CarrySick", true);
	}
}
