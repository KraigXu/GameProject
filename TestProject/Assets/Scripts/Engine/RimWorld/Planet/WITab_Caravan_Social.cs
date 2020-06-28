using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001296 RID: 4758
	public class WITab_Caravan_Social : WITab
	{
		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06007010 RID: 28688 RVA: 0x0026F60A File Offset: 0x0026D80A
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06007011 RID: 28689 RVA: 0x0027153D File Offset: 0x0026F73D
		private float SpecificSocialTabWidth
		{
			get
			{
				this.EnsureSpecificSocialTabForPawnValid();
				if (this.specificSocialTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 540f;
			}
		}

		// Token: 0x06007012 RID: 28690 RVA: 0x0027155D File Offset: 0x0026F75D
		public WITab_Caravan_Social()
		{
			this.labelKey = "TabCaravanSocial";
		}

		// Token: 0x06007013 RID: 28691 RVA: 0x00271570 File Offset: 0x0026F770
		protected override void FillTab()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 15f, this.size.x, this.size.y - 15f).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06007014 RID: 28692 RVA: 0x00271620 File Offset: 0x0026F820
		protected override void UpdateSize()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.UpdateSize();
			this.size.x = 243f;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x06007015 RID: 28693 RVA: 0x00271660 File Offset: 0x0026F860
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificSocialTabForPawn = this.specificSocialTabForPawn;
			if (localSpecificSocialTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificSocialTabWidth = this.SpecificSocialTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificSocialTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificSocialTabForPawn.DestroyedOrNull())
					{
						return;
					}
					SocialCardUtility.DrawSocialCard(rect.AtZero(), localSpecificSocialTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificSocialTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f);
			}
		}

		// Token: 0x06007016 RID: 28694 RVA: 0x00271708 File Offset: 0x0026F908
		public override void OnOpen()
		{
			base.OnOpen();
			if ((this.specificSocialTabForPawn == null || !this.Pawns.Contains(this.specificSocialTabForPawn)) && this.Pawns.Any<Pawn>())
			{
				this.specificSocialTabForPawn = this.Pawns[0];
			}
		}

		// Token: 0x06007017 RID: 28695 RVA: 0x00271758 File Offset: 0x0026F958
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(base.SelCaravan, null, null);
			GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
			Widgets.Label(new Rect(0f, curY, scrollViewRect.width, 24f), string.Format("{0}: {1}", "Negotiator".TranslateSimple(), (pawn != null) ? pawn.LabelShort : "NoneCapable".Translate().ToString()));
			curY += 24f;
			if (this.specificSocialTabForPawn != null && !pawns.Contains(this.specificSocialTabForPawn))
			{
				this.specificSocialTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn2 = pawns[i];
				if (pawn2.RaceProps.IsFlesh && pawn2.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn3 = pawns[j];
				if (pawn3.RaceProps.IsFlesh && !pawn3.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn3);
				}
			}
		}

		// Token: 0x06007018 RID: 28696 RVA: 0x002718DC File Offset: 0x0026FADC
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 34f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 34f), p);
			}
			curY += 34f;
		}

		// Token: 0x06007019 RID: 28697 RVA: 0x00271944 File Offset: 0x0026FB44
		private void DoRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificSocialTabForPawn);
			rect2.width -= 24f;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
			Rect bgRect = new Rect(rect3.xMax + 4f, 8f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x0600701A RID: 28698 RVA: 0x00271AA6 File Offset: 0x0026FCA6
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificSocialTabForPawn = null;
		}

		// Token: 0x0600701B RID: 28699 RVA: 0x00271AB5 File Offset: 0x0026FCB5
		private void EnsureSpecificSocialTabForPawnValid()
		{
			if (this.specificSocialTabForPawn != null && (this.specificSocialTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificSocialTabForPawn)))
			{
				this.specificSocialTabForPawn = null;
			}
		}

		// Token: 0x040044F8 RID: 17656
		private Vector2 scrollPosition;

		// Token: 0x040044F9 RID: 17657
		private float scrollViewHeight;

		// Token: 0x040044FA RID: 17658
		private Pawn specificSocialTabForPawn;

		// Token: 0x040044FB RID: 17659
		private const float RowHeight = 34f;

		// Token: 0x040044FC RID: 17660
		private const float ScrollViewTopMargin = 15f;

		// Token: 0x040044FD RID: 17661
		private const float PawnLabelHeight = 18f;

		// Token: 0x040044FE RID: 17662
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x040044FF RID: 17663
		private const float SpaceAroundIcon = 4f;
	}
}
