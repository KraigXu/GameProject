using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001295 RID: 4757
	public class WITab_Caravan_Needs : WITab
	{
		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06007009 RID: 28681 RVA: 0x0027133F File Offset: 0x0026F53F
		private float SpecificNeedsTabWidth
		{
			get
			{
				if (this.specificNeedsTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return NeedsCardUtility.GetSize(this.specificNeedsTabForPawn).x;
			}
		}

		// Token: 0x0600700A RID: 28682 RVA: 0x00271364 File Offset: 0x0026F564
		public WITab_Caravan_Needs()
		{
			this.labelKey = "TabCaravanNeeds";
		}

		// Token: 0x0600700B RID: 28683 RVA: 0x00271377 File Offset: 0x0026F577
		protected override void FillTab()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			CaravanNeedsTabUtility.DoRows(this.size, base.SelCaravan.PawnsListForReading, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, ref this.specificNeedsTabForPawn, this.doNeeds);
		}

		// Token: 0x0600700C RID: 28684 RVA: 0x002713B4 File Offset: 0x0026F5B4
		protected override void UpdateSize()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			base.UpdateSize();
			this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, true);
			if (this.size.x + this.SpecificNeedsTabWidth > (float)UI.screenWidth)
			{
				this.doNeeds = false;
				this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, false);
			}
			else
			{
				this.doNeeds = true;
			}
			this.size.y = Mathf.Max(this.size.y, NeedsCardUtility.FullSize.y);
		}

		// Token: 0x0600700D RID: 28685 RVA: 0x00271458 File Offset: 0x0026F658
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificNeedsTabForPawn = this.specificNeedsTabForPawn;
			if (localSpecificNeedsTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificNeedsTabWidth = this.SpecificNeedsTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificNeedsTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificNeedsTabForPawn.DestroyedOrNull())
					{
						return;
					}
					NeedsCardUtility.DoNeedsMoodAndThoughts(rect.AtZero(), localSpecificNeedsTabForPawn, ref this.thoughtScrollPosition);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificNeedsTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f);
			}
		}

		// Token: 0x0600700E RID: 28686 RVA: 0x002714FD File Offset: 0x0026F6FD
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificNeedsTabForPawn = null;
		}

		// Token: 0x0600700F RID: 28687 RVA: 0x0027150C File Offset: 0x0026F70C
		private void EnsureSpecificNeedsTabForPawnValid()
		{
			if (this.specificNeedsTabForPawn != null && (this.specificNeedsTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificNeedsTabForPawn)))
			{
				this.specificNeedsTabForPawn = null;
			}
		}

		// Token: 0x040044F3 RID: 17651
		private Vector2 scrollPosition;

		// Token: 0x040044F4 RID: 17652
		private float scrollViewHeight;

		// Token: 0x040044F5 RID: 17653
		private Pawn specificNeedsTabForPawn;

		// Token: 0x040044F6 RID: 17654
		private Vector2 thoughtScrollPosition;

		// Token: 0x040044F7 RID: 17655
		private bool doNeeds;
	}
}
