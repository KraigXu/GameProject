using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		
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

		
		// (get) Token: 0x06005C60 RID: 23648 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06005C61 RID: 23649 RVA: 0x001FEF3B File Offset: 0x001FD13B
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		
		// (get) Token: 0x06005C62 RID: 23650 RVA: 0x001FEF43 File Offset: 0x001FD143
		private List<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		
		// (get) Token: 0x06005C63 RID: 23651 RVA: 0x001F7762 File Offset: 0x001F5962
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		
		// (get) Token: 0x06005C64 RID: 23652 RVA: 0x001D4C0A File Offset: 0x001D2E0A
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		
		// (get) Token: 0x06005C65 RID: 23653 RVA: 0x001FEF4F File Offset: 0x001FD14F
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		
		// (get) Token: 0x06005C66 RID: 23654 RVA: 0x001FEF5B File Offset: 0x001FD15B
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		
		// (get) Token: 0x06005C67 RID: 23655 RVA: 0x001FEF6F File Offset: 0x001FD16F
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		
		// (get) Token: 0x06005C68 RID: 23656 RVA: 0x001FEF7A File Offset: 0x001FD17A
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		
		// (get) Token: 0x06005C69 RID: 23657 RVA: 0x001FEFA9 File Offset: 0x001FD1A9
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.NumSelected == 1;
			}
		}

		
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

		
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.soundClose = SoundDefOf.TabClose;
		}

		
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			if (this.AnythingSelected && Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
			}
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
		}

		
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		
		public void DrawInspectGizmos()
		{
			InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected, out this.mouseoverGizmo);
		}

		
		public void DoPaneContents(Rect rect)
		{
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		
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

		
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		
		public void Reset()
		{
			this.openTabType = null;
		}

		
		private Type openTabType;

		
		private float recentHeight;

		
		private static IntVec3 lastSelectCell;

		
		private Gizmo mouseoverGizmo;
	}
}
