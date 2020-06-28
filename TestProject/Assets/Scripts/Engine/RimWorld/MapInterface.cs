using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ECD RID: 3789
	public class MapInterface
	{
		// Token: 0x06005CDF RID: 23775 RVA: 0x00203BB0 File Offset: 0x00201DB0
		public void MapInterfaceOnGUI_BeforeMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				this.thingOverlays.ThingOverlaysOnGUI();
				MapComponentUtility.MapComponentOnGUI(Find.CurrentMap);
				BeautyDrawer.BeautyDrawerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.colonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.designatorManager.DesignationManagerOnGUI();
				this.targeter.TargeterOnGUI();
				Find.CurrentMap.tooltipGiverList.DispenseAllThingTooltips();
				if (DebugViewSettings.drawFoodSearchFromMouse)
				{
					FoodUtility.DebugFoodSearchFromMouse_OnGUI();
				}
				if (DebugViewSettings.drawAttackTargetScores)
				{
					AttackTargetFinder.DebugDrawAttackTargetScores_OnGUI();
				}
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.mouseoverReadout.MouseoverReadoutOnGUI();
					this.globalControls.GlobalControlsOnGUI();
					this.resourceReadout.ResourceReadoutOnGUI();
					return;
				}
			}
			else
			{
				this.targeter.StopTargeting();
			}
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x00203C87 File Offset: 0x00201E87
		public void MapInterfaceOnGUI_AfterMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow && !Find.UIRoot.screenshotMode.FiltersCurrentEvent)
			{
				EnvironmentStatsDrawer.EnvironmentStatsOnGUI();
				Find.CurrentMap.debugDrawer.DebugDrawerOnGUI();
			}
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x00203CBD File Offset: 0x00201EBD
		public void HandleMapClicks()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				this.designatorManager.ProcessInputEvents();
				this.targeter.ProcessInputEvents();
			}
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x00203CE4 File Offset: 0x00201EE4
		public void HandleLowPriorityInput()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				this.selector.SelectorOnGUI();
				Find.CurrentMap.lordManager.LordManagerOnGUI();
			}
		}

		// Token: 0x06005CE3 RID: 23779 RVA: 0x00203D10 File Offset: 0x00201F10
		public void MapInterfaceUpdate()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				this.targeter.TargeterUpdate();
				SelectionDrawer.DrawSelectionOverlays();
				EnvironmentStatsDrawer.DrawRoomOverlays();
				this.designatorManager.DesignatorManagerUpdate();
				Find.CurrentMap.roofGrid.RoofGridUpdate();
				Find.CurrentMap.fertilityGrid.FertilityGridUpdate();
				Find.CurrentMap.terrainGrid.TerrainGridUpdate();
				Find.CurrentMap.exitMapGrid.ExitMapGridUpdate();
				Find.CurrentMap.deepResourceGrid.DeepResourceGridUpdate();
				if (DebugViewSettings.drawPawnDebug)
				{
					Find.CurrentMap.pawnDestinationReservationManager.DebugDrawDestinations();
					Find.CurrentMap.reservationManager.DebugDrawReservations();
				}
				if (DebugViewSettings.drawDestReservations)
				{
					Find.CurrentMap.pawnDestinationReservationManager.DebugDrawReservations();
				}
				if (DebugViewSettings.drawFoodSearchFromMouse)
				{
					FoodUtility.DebugFoodSearchFromMouse_Update();
				}
				if (DebugViewSettings.drawPreyInfo)
				{
					FoodUtility.DebugDrawPredatorFoodSource();
				}
				if (DebugViewSettings.drawAttackTargetScores)
				{
					AttackTargetFinder.DebugDrawAttackTargetScores_Update();
				}
				MiscDebugDrawer.DebugDrawInteractionCells();
				Find.CurrentMap.debugDrawer.DebugDrawerUpdate();
				Find.CurrentMap.regionGrid.DebugDraw();
				InfestationCellFinder.DebugDraw();
				StealAIDebugDrawer.DebugDraw();
				if (DebugViewSettings.drawRiverDebug)
				{
					Find.CurrentMap.waterInfo.DebugDrawRiver();
				}
				BuildingsDamageSectionLayerUtility.DebugDraw();
			}
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x00203E44 File Offset: 0x00202044
		public void Notify_SwitchedMap()
		{
			this.designatorManager.Deselect();
			this.reverseDesignatorDatabase.Reinit();
			this.selector.ClearSelection();
			this.selector.dragBox.active = false;
			this.targeter.StopTargeting();
			MainButtonDef openTab = Find.MainTabsRoot.OpenTab;
			List<MainButtonDef> allDefsListForReading = DefDatabase<MainButtonDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				allDefsListForReading[i].Notify_SwitchedMap();
			}
			if (openTab != null && openTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.SetCurrentTab(openTab, false);
			}
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
			}
		}

		// Token: 0x040032B2 RID: 12978
		public ThingOverlays thingOverlays = new ThingOverlays();

		// Token: 0x040032B3 RID: 12979
		public Selector selector = new Selector();

		// Token: 0x040032B4 RID: 12980
		public Targeter targeter = new Targeter();

		// Token: 0x040032B5 RID: 12981
		public DesignatorManager designatorManager = new DesignatorManager();

		// Token: 0x040032B6 RID: 12982
		public ReverseDesignatorDatabase reverseDesignatorDatabase = new ReverseDesignatorDatabase();

		// Token: 0x040032B7 RID: 12983
		private MouseoverReadout mouseoverReadout = new MouseoverReadout();

		// Token: 0x040032B8 RID: 12984
		public GlobalControls globalControls = new GlobalControls();

		// Token: 0x040032B9 RID: 12985
		protected ResourceReadout resourceReadout = new ResourceReadout();

		// Token: 0x040032BA RID: 12986
		public ColonistBar colonistBar = new ColonistBar();
	}
}
