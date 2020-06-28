using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F30 RID: 3888
	public class WorldInterface
	{
		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06005F39 RID: 24377 RVA: 0x0020E1E9 File Offset: 0x0020C3E9
		// (set) Token: 0x06005F3A RID: 24378 RVA: 0x0020E1F6 File Offset: 0x0020C3F6
		public int SelectedTile
		{
			get
			{
				return this.selector.selectedTile;
			}
			set
			{
				this.selector.selectedTile = value;
			}
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x0020E204 File Offset: 0x0020C404
		public void Reset()
		{
			this.everReset = true;
			this.inspectPane.Reset();
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Find.CurrentMap != null)
				{
					this.SelectedTile = Find.CurrentMap.Tile;
				}
				else
				{
					this.SelectedTile = -1;
				}
			}
			else if (Find.GameInitData != null)
			{
				if (Find.GameInitData.startingTile >= 0 && Find.World != null && !Find.WorldGrid.InBounds(Find.GameInitData.startingTile))
				{
					Log.Error("Map world tile was out of bounds.", false);
					Find.GameInitData.startingTile = -1;
				}
				this.SelectedTile = Find.GameInitData.startingTile;
				this.inspectPane.OpenTabType = typeof(WITab_Terrain);
			}
			else
			{
				this.SelectedTile = -1;
			}
			if (this.SelectedTile >= 0)
			{
				Find.WorldCameraDriver.JumpTo(this.SelectedTile);
			}
			else
			{
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.viewCenter);
			}
			Find.WorldCameraDriver.ResetAltitude();
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x0020E2FB File Offset: 0x0020C4FB
		public void WorldInterfaceUpdate()
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				this.targeter.TargeterUpdate();
				WorldSelectionDrawer.DrawSelectionOverlays();
				Find.WorldDebugDrawer.WorldDebugDrawerUpdate();
			}
			else
			{
				this.targeter.StopTargeting();
			}
			this.routePlanner.WorldRoutePlannerUpdate();
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x0020E338 File Offset: 0x0020C538
		public void WorldInterfaceOnGUI()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.CheckOpenOrCloseInspectPane();
			if (worldRenderedNow)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsOnGUI();
				WorldSelectionDrawer.SelectionOverlaysOnGUI();
				this.routePlanner.WorldRoutePlannerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.targeter.TargeterOnGUI();
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.globalControls.WorldGlobalControlsOnGUI();
				}
				Find.WorldDebugDrawer.WorldDebugDrawerOnGUI();
			}
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x0020E3C2 File Offset: 0x0020C5C2
		public void HandleLowPriorityInput()
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				this.targeter.ProcessInputEvents();
				this.selector.WorldSelectorOnGUI();
			}
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x0020E3E4 File Offset: 0x0020C5E4
		private void CheckOpenOrCloseInspectPane()
		{
			if (this.selector.AnyObjectOrTileSelected && WorldRendererUtility.WorldRenderedNow && (Current.ProgramState != ProgramState.Playing || Find.MainTabsRoot.OpenTab == null))
			{
				if (!Find.WindowStack.IsOpen<WorldInspectPane>())
				{
					Find.WindowStack.Add(this.inspectPane);
					return;
				}
			}
			else if (Find.WindowStack.IsOpen<WorldInspectPane>())
			{
				Find.WindowStack.TryRemove(this.inspectPane, false);
			}
		}

		// Token: 0x040033A8 RID: 13224
		public WorldSelector selector = new WorldSelector();

		// Token: 0x040033A9 RID: 13225
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x040033AA RID: 13226
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x040033AB RID: 13227
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x040033AC RID: 13228
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x040033AD RID: 13229
		public bool everReset;
	}
}
