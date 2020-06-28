using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC5 RID: 3781
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06005C5C RID: 23644 RVA: 0x001FEF19 File Offset: 0x001FD119
		// (set) Token: 0x06005C5D RID: 23645 RVA: 0x001FEF21 File Offset: 0x001FD121
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06005C5E RID: 23646 RVA: 0x001FEF2A File Offset: 0x001FD12A
		// (set) Token: 0x06005C5F RID: 23647 RVA: 0x001FEF32 File Offset: 0x001FD132
		public float RecentHeight
		{
			get
			{
				return this.recentHeight;
			}
			set
			{
				this.recentHeight = value;
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06005C60 RID: 23648 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06005C61 RID: 23649 RVA: 0x001FEF3B File Offset: 0x001FD13B
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06005C62 RID: 23650 RVA: 0x001FEF43 File Offset: 0x001FD143
		private List<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06005C63 RID: 23651 RVA: 0x001F7762 File Offset: 0x001F5962
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06005C64 RID: 23652 RVA: 0x001D4C0A File Offset: 0x001D2E0A
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06005C65 RID: 23653 RVA: 0x001FEF4F File Offset: 0x001FD14F
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x06005C66 RID: 23654 RVA: 0x001FEF5B File Offset: 0x001FD15B
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06005C67 RID: 23655 RVA: 0x001FEF6F File Offset: 0x001FD16F
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06005C68 RID: 23656 RVA: 0x001FEF7A File Offset: 0x001FD17A
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06005C69 RID: 23657 RVA: 0x001FEFA9 File Offset: 0x001FD1A9
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.NumSelected == 1;
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06005C6A RID: 23658 RVA: 0x001FEFB4 File Offset: 0x001FD1B4
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelected == 1)
				{
					if (this.SelThing != null && this.SelThing.def.inspectorTabsResolved != null)
					{
						return this.SelThing.GetInspectTabs();
					}
					if (this.SelZone != null)
					{
						return this.SelZone.GetInspectTabs();
					}
				}
				return null;
			}
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x001FF005 File Offset: 0x001FD205
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.soundClose = SoundDefOf.TabClose;
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x001FF026 File Offset: 0x001FD226
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			if (this.AnythingSelected && Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
			}
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x001FF062 File Offset: 0x001FD262
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x001FF072 File Offset: 0x001FD272
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		// Token: 0x06005C6F RID: 23663 RVA: 0x001FF080 File Offset: 0x001FD280
		public void DrawInspectGizmos()
		{
			InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected, out this.mouseoverGizmo);
		}

		// Token: 0x06005C70 RID: 23664 RVA: 0x001FF093 File Offset: 0x001FD293
		public void DoPaneContents(Rect rect)
		{
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x001FF0AC File Offset: 0x001FD2AC
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			if (this.NumSelected == 1)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing != null)
				{
					Widgets.InfoCardButton(rect.width - 48f, 0f, Find.Selector.SingleSelectedThing);
					lineEndWidth += 24f;
					Pawn pawn = singleSelectedThing as Pawn;
					if (pawn != null && pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
					{
						HostilityResponseModeUtility.DrawResponseButton(new Rect(rect.width - 72f, 0f, 24f, 24f), pawn, false);
						lineEndWidth += 24f;
					}
				}
			}
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x001FF150 File Offset: 0x001FD350
		public void SelectNextInCell()
		{
			if (this.NumSelected != 1)
			{
				return;
			}
			Selector selector = Find.Selector;
			if (selector.SelectedZone == null || selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell))
			{
				if (selector.SelectedZone == null)
				{
					MainTabWindow_Inspect.lastSelectCell = selector.SingleSelectedThing.Position;
				}
				Map map;
				if (selector.SingleSelectedThing != null)
				{
					map = selector.SingleSelectedThing.Map;
				}
				else
				{
					map = selector.SelectedZone.Map;
				}
				selector.SelectNextAt(MainTabWindow_Inspect.lastSelectCell, map);
			}
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x001FF1CD File Offset: 0x001FD3CD
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x001FF1EE File Offset: 0x001FD3EE
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x06005C75 RID: 23669 RVA: 0x001FF1EE File Offset: 0x001FD3EE
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x04003265 RID: 12901
		private Type openTabType;

		// Token: 0x04003266 RID: 12902
		private float recentHeight;

		// Token: 0x04003267 RID: 12903
		private static IntVec3 lastSelectCell;

		// Token: 0x04003268 RID: 12904
		private Gizmo mouseoverGizmo;
	}
}
