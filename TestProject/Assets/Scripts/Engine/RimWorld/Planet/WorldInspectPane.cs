using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldInspectPane : Window, IInspectPane
	{
		
		// (get) Token: 0x06007033 RID: 28723 RVA: 0x0027256A File Offset: 0x0027076A
		// (set) Token: 0x06007034 RID: 28724 RVA: 0x00272572 File Offset: 0x00270772
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

		
		// (get) Token: 0x06007035 RID: 28725 RVA: 0x0027257B File Offset: 0x0027077B
		// (set) Token: 0x06007036 RID: 28726 RVA: 0x00272583 File Offset: 0x00270783
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

		
		// (get) Token: 0x06007037 RID: 28727 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06007038 RID: 28728 RVA: 0x001FEF3B File Offset: 0x001FD13B
		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		
		// (get) Token: 0x06007039 RID: 28729 RVA: 0x0027258C File Offset: 0x0027078C
		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		
		// (get) Token: 0x0600703A RID: 28730 RVA: 0x00272598 File Offset: 0x00270798
		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		
		// (get) Token: 0x0600703B RID: 28731 RVA: 0x002725A4 File Offset: 0x002707A4
		public float PaneTopY
		{
			get
			{
				float num = (float)UI.screenHeight - 165f;
				if (Current.ProgramState == ProgramState.Playing)
				{
					num -= 35f;
				}
				return num;
			}
		}

		
		// (get) Token: 0x0600703C RID: 28732 RVA: 0x002725CF File Offset: 0x002707CF
		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		
		// (get) Token: 0x0600703D RID: 28733 RVA: 0x00250E5E File Offset: 0x0024F05E
		private int SelectedTile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		
		// (get) Token: 0x0600703E RID: 28734 RVA: 0x002725DB File Offset: 0x002707DB
		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile >= 0);
			}
		}

		
		// (get) Token: 0x0600703F RID: 28735 RVA: 0x002725FE File Offset: 0x002707FE
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		
		// (get) Token: 0x06007040 RID: 28736 RVA: 0x002725FE File Offset: 0x002707FE
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		
		// (get) Token: 0x06007041 RID: 28737 RVA: 0x00272606 File Offset: 0x00270806
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelectedObjects == 1)
				{
					return Find.WorldSelector.SingleSelectedObject.GetInspectTabs();
				}
				if (this.NumSelectedObjects == 0 && this.SelectedTile >= 0)
				{
					return WorldInspectPane.TileTabs;
				}
				return null;
			}
		}

		
		// (get) Token: 0x06007042 RID: 28738 RVA: 0x0027263C File Offset: 0x0027083C
		private string TileInspectString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				Vector2 vector = Find.WorldGrid.LongLatOf(this.SelectedTile);
				stringBuilder.Append(vector.y.ToStringLatitude());
				stringBuilder.Append(" ");
				stringBuilder.Append(vector.x.ToStringLongitude());
				Tile tile = Find.WorldGrid[this.SelectedTile];
				if (!tile.biome.impassable)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(tile.hilliness.GetLabelCap());
				}
				if (tile.Roads != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append((from rl in tile.Roads
					select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
				}
				if (!Find.World.Impassable(this.SelectedTile))
				{
					string t = (WorldPathGrid.CalculatedMovementDifficultyAt(this.SelectedTile, false, null, null) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(this.SelectedTile, -1, null)).ToString("0.#");
					stringBuilder.AppendLine();
					stringBuilder.Append("MovementDifficulty".Translate() + ": " + t);
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("AvgTemp".Translate() + ": " + GenTemperature.GetAverageTemperatureLabel(this.SelectedTile));
				return stringBuilder.ToString();
			}
		}

		
		public WorldInspectPane()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
		}

		
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			this.windowRect.x = 0f;
			this.windowRect.y = this.PaneTopY;
		}

		
		public void DrawInspectGizmos()
		{
			WorldInspectPane.tmpObjectsList.Clear();
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			List<WorldObject> selected = this.Selected;
			for (int i = 0; i < selected.Count; i++)
			{
				if (!worldRoutePlanner.Active || selected[i] is RoutePlannerWaypoint)
				{
					WorldInspectPane.tmpObjectsList.Add(selected[i]);
				}
			}
			InspectGizmoGrid.DrawInspectGizmoGridFor(WorldInspectPane.tmpObjectsList, out this.mouseoverGizmo);
			WorldInspectPane.tmpObjectsList.Clear();
		}

		
		public string GetLabel(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				return WorldInspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
			}
			if (this.SelectedTile >= 0)
			{
				return Find.WorldGrid[this.SelectedTile].biome.LabelCap;
			}
			return "error";
		}

		
		public void SelectNextInCell()
		{
			if (!this.AnythingSelected)
			{
				return;
			}
			if (this.NumSelectedObjects > 0)
			{
				Find.WorldSelector.SelectFirstOrNextAt(this.Selected[0].Tile);
				return;
			}
			Find.WorldSelector.SelectFirstOrNextAt(this.SelectedTile);
		}

		
		public void DoPaneContents(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				InspectPaneFiller.DoPaneContentsFor(Find.WorldSelector.FirstSelectedObject, rect);
				return;
			}
			if (this.SelectedTile >= 0)
			{
				InspectPaneFiller.DrawInspectString(this.TileInspectString, rect);
			}
		}

		
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
			if (singleSelectedObject != null || this.SelectedTile >= 0)
			{
				float x = rect.width - 48f;
				if (singleSelectedObject != null)
				{
					Widgets.InfoCardButton(x, 0f, singleSelectedObject);
				}
				else
				{
					Widgets.InfoCardButton(x, 0f, Find.WorldGrid[this.SelectedTile].biome);
				}
				lineEndWidth += 24f;
			}
		}

		
		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
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

		
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		
		public void Reset()
		{
			this.openTabType = null;
		}

		
		private static readonly WITab[] TileTabs = new WITab[]
		{
			new WITab_Terrain(),
			new WITab_Planet()
		};

		
		private Type openTabType;

		
		private float recentHeight;

		
		public Gizmo mouseoverGizmo;

		
		private static List<object> tmpObjectsList = new List<object>();
	}
}
