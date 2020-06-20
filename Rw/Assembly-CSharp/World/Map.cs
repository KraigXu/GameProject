using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{
    public sealed class Map
    {
        public MapInfo info = new MapInfo();

        public Map() { }

        public void InitContent()
        {


        }

		public void MapUpdate()
		{
			//bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			//this.skyManager.SkyManagerUpdate();
			//this.powerNetManager.UpdatePowerNetsAndConnections_First();
			//this.regionGrid.UpdateClean();
			//this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			//this.glowGrid.GlowGridUpdate_First();
			//this.lordManager.LordManagerUpdate();
			//if (!worldRenderedNow && Find.CurrentMap == this)
			//{
			//	if (Map.AlwaysRedrawShadows)
			//	{
			//		this.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
			//	}
			//	PlantFallColors.SetFallShaderGlobals(this);
			//	this.waterInfo.SetTextures();
			//	this.avoidGrid.DebugDrawOnMap();
			//	this.mapDrawer.MapMeshDrawerUpdate_First();
			//	this.powerNetGrid.DrawDebugPowerNetGrid();
			//	DoorsDebugDrawer.DrawDebug();
			//	this.mapDrawer.DrawMapMesh();
			//	this.dynamicDrawManager.DrawDynamicThings();
			//	this.gameConditionManager.GameConditionManagerDraw(this);
			//	MapEdgeClipDrawer.DrawClippers(this);
			//	this.designationManager.DrawDesignations();
			//	this.overlayDrawer.DrawAllOverlays();
			//}
			//try
			//{
			//	this.areaManager.AreaManagerUpdate();
			//}
			//catch (Exception ex)
			//{
			//	Log.Error(ex.ToString(), false);
			//}
			//this.weatherManager.WeatherManagerUpdate();
			//MapComponentUtility.MapComponentUpdate(this);
		}
	}
}
