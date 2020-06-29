using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public sealed class TerrainGrid : IExposable
	{
		
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x000383E4 File Offset: 0x000365E4
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3600, 0.33f);
				}
				return this.drawerInt;
			}
		}

		
		public TerrainGrid(Map map)
		{
			this.map = map;
			this.ResetGrids();
		}

		
		public void ResetGrids()
		{
			this.topGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.underGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
		}

		
		public TerrainDef TerrainAt(int ind)
		{
			return this.topGrid[ind];
		}

		
		public TerrainDef TerrainAt(IntVec3 c)
		{
			return this.topGrid[this.map.cellIndices.CellToIndex(c)];
		}

		
		public TerrainDef UnderTerrainAt(int ind)
		{
			return this.underGrid[ind];
		}

		
		public TerrainDef UnderTerrainAt(IntVec3 c)
		{
			return this.underGrid[this.map.cellIndices.CellToIndex(c)];
		}

		
		public void SetTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				Log.Error("Tried to set terrain at " + c + " to null.", false);
				return;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
				if (designation != null)
				{
					designation.Delete();
				}
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newTerr.layerable)
			{
				if (this.underGrid[num] == null)
				{
					if (this.topGrid[num].passability != Traversability.Impassable)
					{
						this.underGrid[num] = this.topGrid[num];
					}
					else
					{
						this.underGrid[num] = TerrainDefOf.Sand;
					}
				}
			}
			else
			{
				this.underGrid[num] = null;
			}
			this.topGrid[num] = newTerr;
			this.DoTerrainChangedEffects(c);
		}

		
		public void SetUnderTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to set terrain out of bounds at " + c, false);
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			this.underGrid[num] = newTerr;
		}

		
		public void RemoveTopLayer(IntVec3 c, bool doLeavings = true)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.topGrid[num], c, this.map);
			}
			if (this.underGrid[num] != null)
			{
				this.topGrid[num] = this.underGrid[num];
				this.underGrid[num] = null;
				this.DoTerrainChangedEffects(c);
			}
		}

		
		public bool CanRemoveTopLayerAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			return this.topGrid[num].Removable && this.underGrid[num] != null;
		}

		
		private void DoTerrainChangedEffects(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Terrain, true, false);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (thingList[i].def.category == ThingCategory.Plant && this.map.fertilityGrid.FertilityAt(c) < thingList[i].def.plant.fertilityMin)
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				else if (thingList[i].def.category == ThingCategory.Filth && !FilthMaker.TerrainAcceptsFilth(this.TerrainAt(c), thingList[i].def, FilthSourceFlags.None))
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				else if ((thingList[i].def.IsBlueprint || thingList[i].def.IsFrame) && !GenConstruct.CanBuildOnTerrain(thingList[i].def.entityDefToBuild, thingList[i].Position, this.map, thingList[i].Rotation, null, ((IConstructible)thingList[i]).EntityToBuildStuff()))
				{
					thingList[i].Destroy(DestroyMode.Cancel);
				}
			}
			this.map.pathGrid.RecalculatePerceivedPathCostAt(c);
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.fertilityGrid.Drawer.SetDirty();
			Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c);
			if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.Room != null)
			{
				regionAt_NoRebuild_InvalidAllowed.Room.Notify_TerrainChanged();
			}
		}

		
		public void ExposeData()
		{
			this.ExposeTerrainGrid(this.topGrid, "topGrid");
			this.ExposeTerrainGrid(this.underGrid, "underGrid");
		}

		
		public void Notify_TerrainBurned(IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(this.map);
			this.Notify_TerrainDestroyed(c);
			if (terrain.burnedDef != null)
			{
				this.SetTerrain(c, terrain.burnedDef);
			}
		}

		
		public void Notify_TerrainDestroyed(IntVec3 c)
		{
			if (!this.CanRemoveTopLayerAt(c))
			{
				return;
			}
			TerrainDef terrainDef = this.TerrainAt(c);
			this.RemoveTopLayer(c, false);
			if (terrainDef.destroyBuildingsOnDestroyed)
			{
				Building firstBuilding = c.GetFirstBuilding(this.map);
				if (firstBuilding != null)
				{
					firstBuilding.Kill(null, null);
				}
			}
			if (terrainDef.destroyEffectWater != null && this.TerrainAt(c) != null && this.TerrainAt(c).IsWater)
			{
				Effecter effecter = terrainDef.destroyEffectWater.Spawn();
				effecter.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false));
				effecter.Cleanup();
				return;
			}
			if (terrainDef.destroyEffect != null)
			{
				Effecter effecter2 = terrainDef.destroyEffect.Spawn();
				effecter2.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false));
				effecter2.Cleanup();
			}
		}

		
		private void ExposeTerrainGrid(TerrainDef[] grid, string label)
		{
			Dictionary<ushort, TerrainDef> terrainDefsByShortHash = new Dictionary<ushort, TerrainDef>();
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				terrainDefsByShortHash.Add(terrainDef.shortHash, terrainDef);
			}
			Func<IntVec3, ushort> shortReader = delegate(IntVec3 c)
			{
				TerrainDef terrainDef2 = grid[this.map.cellIndices.CellToIndex(c)];
				if (terrainDef2 == null)
				{
					return 0;
				}
				return terrainDef2.shortHash;
			};
			Action<IntVec3, ushort> shortWriter = delegate(IntVec3 c, ushort val)
			{
				TerrainDef terrainDef2 = terrainDefsByShortHash.TryGetValue(val, null);
				if (terrainDef2 == null && val != 0)
				{
					TerrainDef terrainDef3 = BackCompatibility.BackCompatibleTerrainWithShortHash(val);
					if (terrainDef3 == null)
					{
						Log.Error(string.Concat(new object[]
						{
							"Did not find terrain def with short hash ",
							val,
							" for cell ",
							c,
							"."
						}), false);
						terrainDef3 = TerrainDefOf.Soil;
					}
					terrainDef2 = terrainDef3;
					terrainDefsByShortHash.Add(val, terrainDef3);
				}
				grid[this.map.cellIndices.CellToIndex(c)] = terrainDef2;
			};
			MapExposeUtility.ExposeUshort(this.map, shortReader, shortWriter, label);
		}

		
		public string DebugStringAt(IntVec3 c)
		{
			if (c.InBounds(this.map))
			{
				TerrainDef terrain = c.GetTerrain(this.map);
				TerrainDef terrainDef = this.underGrid[this.map.cellIndices.CellToIndex(c)];
				return "top: " + ((terrain != null) ? terrain.defName : "null") + ", under=" + ((terrainDef != null) ? terrainDef.defName : "null");
			}
			return "out of bounds";
		}

		
		public void TerrainGridUpdate()
		{
			if (Find.PlaySettings.showTerrainAffordanceOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 c = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			TerrainAffordanceDef terrainAffordanceDef;
			return !c.Filled(this.map) && !c.Fogged(this.map) && this.TryGetAffordanceDefToDraw(this.TerrainAt(index), out terrainAffordanceDef);
		}

		
		private bool TryGetAffordanceDefToDraw(TerrainDef terrainDef, out TerrainAffordanceDef affordance)
		{
			if (terrainDef == null || terrainDef.affordances.NullOrEmpty<TerrainAffordanceDef>())
			{
				affordance = null;
				return true;
			}
			TerrainAffordanceDef terrainAffordanceDef = null;
			int num = int.MinValue;
			foreach (TerrainAffordanceDef terrainAffordanceDef2 in terrainDef.affordances)
			{
				if (terrainAffordanceDef2.visualizeOnAffordanceOverlay)
				{
					if (num < terrainAffordanceDef2.order)
					{
						num = terrainAffordanceDef2.order;
						terrainAffordanceDef = terrainAffordanceDef2;
					}
				}
				else if (terrainAffordanceDef2.blockAffordanceOverlay)
				{
					affordance = null;
					return false;
				}
			}
			affordance = terrainAffordanceDef;
			return true;
		}

		
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			TerrainAffordanceDef terrainAffordanceDef;
			this.TryGetAffordanceDefToDraw(this.TerrainAt(index), out terrainAffordanceDef);
			if (terrainAffordanceDef != null)
			{
				return terrainAffordanceDef.affordanceOverlayColor;
			}
			return TerrainGrid.NoAffordanceColor;
		}

		
		private Map map;

		
		public TerrainDef[] topGrid;

		
		private TerrainDef[] underGrid;

		
		private CellBoolDrawer drawerInt;

		
		private static readonly Color NoAffordanceColor = Color.red;
	}
}
