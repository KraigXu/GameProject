using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EC1 RID: 3777
	public class MainTabWindow_Architect : MainTabWindow
	{
		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06005C44 RID: 23620 RVA: 0x001FDC18 File Offset: 0x001FBE18
		public float WinHeight
		{
			get
			{
				if (this.desPanelsCached == null)
				{
					this.CacheDesPanels();
				}
				return (float)Mathf.CeilToInt((float)this.desPanelsCached.Count / 2f) * 32f;
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x06005C45 RID: 23621 RVA: 0x001FDC46 File Offset: 0x001FBE46
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(200f, this.WinHeight);
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06005C46 RID: 23622 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x001FDC58 File Offset: 0x001FBE58
		public MainTabWindow_Architect()
		{
			this.CacheDesPanels();
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x001FDC66 File Offset: 0x001FBE66
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x001FDC7E File Offset: 0x001FBE7E
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.selectedDesPanel != null && this.selectedDesPanel.def.showPowerGrid)
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
			}
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x001FDCA5 File Offset: 0x001FBEA5
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.selectedDesPanel != null)
			{
				this.selectedDesPanel.DesignationTabOnGUI();
			}
		}

		// Token: 0x06005C4B RID: 23627 RVA: 0x001FDCC0 File Offset: 0x001FBEC0
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Text.Font = GameFont.Small;
			float num = inRect.width / 2f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.desPanelsCached.Count; i++)
			{
				Rect rect = new Rect(num2 * num, num3 * 32f, num, 32f);
				float height = rect.height;
				rect.height = height + 1f;
				if (num2 == 0f)
				{
					rect.width += 1f;
				}
				if (Widgets.ButtonTextSubtle(rect, this.desPanelsCached[i].def.LabelCap, 0f, 8f, SoundDefOf.Mouseover_Category, new Vector2(-1f, -1f)))
				{
					this.ClickedCategory(this.desPanelsCached[i]);
				}
				if (this.selectedDesPanel != this.desPanelsCached[i])
				{
					UIHighlighter.HighlightOpportunity(rect, this.desPanelsCached[i].def.cachedHighlightClosedTag);
				}
				num2 += 1f;
				if (num2 > 1f)
				{
					num2 = 0f;
					num3 += 1f;
				}
			}
		}

		// Token: 0x06005C4C RID: 23628 RVA: 0x001FDDFC File Offset: 0x001FBFFC
		private void CacheDesPanels()
		{
			this.desPanelsCached = new List<ArchitectCategoryTab>();
			foreach (DesignationCategoryDef def in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				this.desPanelsCached.Add(new ArchitectCategoryTab(def));
			}
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x001FDE84 File Offset: 0x001FC084
		protected void ClickedCategory(ArchitectCategoryTab Pan)
		{
			if (this.selectedDesPanel == Pan)
			{
				this.selectedDesPanel = null;
			}
			else
			{
				this.selectedDesPanel = Pan;
			}
			SoundDefOf.ArchitectCategorySelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x0400324B RID: 12875
		private List<ArchitectCategoryTab> desPanelsCached;

		// Token: 0x0400324C RID: 12876
		public ArchitectCategoryTab selectedDesPanel;

		// Token: 0x0400324D RID: 12877
		public const float WinWidth = 200f;

		// Token: 0x0400324E RID: 12878
		private const float ButHeight = 32f;
	}
}
