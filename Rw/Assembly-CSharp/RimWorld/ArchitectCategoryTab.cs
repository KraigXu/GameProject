using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9B RID: 3739
	public class ArchitectCategoryTab
	{
		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06005B2C RID: 23340 RVA: 0x001F61F6 File Offset: 0x001F43F6
		public static Rect InfoRect
		{
			get
			{
				return new Rect(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f, 200f, 270f);
			}
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x001F6230 File Offset: 0x001F4430
		public ArchitectCategoryTab(DesignationCategoryDef def)
		{
			this.def = def;
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x001F6240 File Offset: 0x001F4440
		public void DesignationTabOnGUI()
		{
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f);
			}
			float startX = 210f;
			Gizmo selectedDesignator;
			GizmoGridDrawer.DrawGizmoGrid(this.def.ResolvedAllowedDesignators.Cast<Gizmo>(), startX, out selectedDesignator);
			if (selectedDesignator == null && Find.DesignatorManager.SelectedDesignator != null)
			{
				selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			}
			this.DoInfoBox(ArchitectCategoryTab.InfoRect, (Designator)selectedDesignator);
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x001F62DC File Offset: 0x001F44DC
		protected void DoInfoBox(Rect infoRect, Designator designator)
		{
			Find.WindowStack.ImmediateWindow(32520, infoRect, WindowLayer.GameUI, delegate
			{
				if (designator != null)
				{
					Rect position = infoRect.AtZero().ContractedBy(7f);
					GUI.BeginGroup(position);
					Rect rect = new Rect(0f, 0f, position.width - designator.PanelReadoutTitleExtraRightMargin, 999f);
					Text.Font = GameFont.Small;
					Widgets.Label(rect, designator.LabelCap);
					float num = Mathf.Max(24f, Text.CalcHeight(designator.LabelCap, rect.width));
					designator.DrawPanelReadout(ref num, position.width);
					Rect rect2 = new Rect(0f, num, position.width, position.height - num);
					string text = designator.Desc;
					GenText.SetTextSizeToFit(text, rect2);
					text = text.TruncateHeight(rect2.width, rect2.height, null);
					Widgets.Label(rect2, text);
					GUI.EndGroup();
				}
			}, true, false, 1f);
		}

		// Token: 0x040031C9 RID: 12745
		public DesignationCategoryDef def;

		// Token: 0x040031CA RID: 12746
		public const float InfoRectHeight = 270f;
	}
}
