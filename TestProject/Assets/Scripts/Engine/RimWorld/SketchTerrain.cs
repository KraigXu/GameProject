using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class SketchTerrain : SketchBuildable
	{
		
		
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		
		
		public override ThingDef Stuff
		{
			get
			{
				return this.stuffForComparingSimilar;
			}
		}

		
		
		public override CellRect OccupiedRect
		{
			get
			{
				return CellRect.SingleCell(this.pos);
			}
		}

		
		
		public override float SpawnOrder
		{
			get
			{
				return 1f;
			}
		}

		
		
		public override string Label
		{
			get
			{
				if (this.def.designatorDropdown == null || this.def.designatorDropdown.label.NullOrEmpty() || !this.treatSimilarAsSame)
				{
					return base.Label;
				}
				return this.def.designatorDropdown.LabelCap;
			}
		}

		
		public override void DrawGhost(IntVec3 at, Color color)
		{
			ThingDef blueprintDef = this.def.blueprintDef;
			GraphicDatabase.Get(blueprintDef.graphic.GetType(), blueprintDef.graphic.path, blueprintDef.graphic.Shader, blueprintDef.graphic.drawSize, color, Color.white, blueprintDef.graphicData, null).DrawFromDef(at.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), Rot4.North, this.def.blueprintDef, 0f);
		}

		
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return at.InBounds(map) && this.IsSameOrSimilar(at.GetTerrain(map));
		}

		
		public bool IsSameOrSimilar(BuildableDef other)
		{
			if (other == null)
			{
				return false;
			}
			if (!this.treatSimilarAsSame)
			{
				return other == this.def;
			}
			if (this.def.designatorDropdown == null && other.designatorDropdown == null && other.BuildableByPlayer)
			{
				return true;
			}
			if (this.def.designatorDropdown == null || other.designatorDropdown == null)
			{
				return other == this.def;
			}
			return other.designatorDropdown == this.def.designatorDropdown;
		}

		
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].Position == at && this.IsSameOrSimilar(thingList[i].def.entityDefToBuild))
				{
					return thingList[i];
				}
			}
			return null;
		}

		
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !at.InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, Rot4.North, map, wipeIfCollides, thingToIgnore, null, null).Accepted;
		}

		
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			if (!at.InBounds(map))
			{
				return true;
			}
			if (!this.CanBuildOnTerrain(at, map))
			{
				return true;
			}
			foreach (IntVec3 c in GenAdj.OccupiedRect(at, Rot4.North, this.def.Size))
			{
				if (!c.InBounds(map))
				{
					return true;
				}
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable && !GenConstruct.CanPlaceBlueprintOver(this.def, thingList[i].def))
					{
						return true;
					}
				}
			}
			return false;
		}

		
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, Rot4.North, null, null);
		}

		
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false)
		{
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			if (spawnMode != Sketch.SpawnMode.Blueprint)
			{
				if (spawnMode != Sketch.SpawnMode.Normal)
				{
					throw new NotImplementedException("Spawn mode " + spawnMode + " not implemented!");
				}
				map.terrainGrid.SetTerrain(at, this.GetDefFromStuff());
			}
			else
			{
				GenConstruct.PlaceBlueprintForBuild(this.GetDefFromStuff(), at, map, Rot4.North, faction, null);
			}
			return true;
		}

		
		private TerrainDef GetDefFromStuff()
		{
			if (this.stuffForComparingSimilar == null)
			{
				return this.def;
			}
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (this.IsSameOrSimilar(terrainDef) && !terrainDef.costList.NullOrEmpty<ThingDefCountClass>() && terrainDef.costList[0].thingDef == this.stuffForComparingSimilar)
				{
					return terrainDef;
				}
			}
			return this.def;
		}

		
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchTerrain sketchTerrain = other as SketchTerrain;
			return sketchTerrain != null && (sketchTerrain == this || (this.IsSameOrSimilar(sketchTerrain.Buildable) && this.pos == sketchTerrain.pos));
		}

		
		public override SketchEntity DeepCopy()
		{
			SketchTerrain sketchTerrain = (SketchTerrain)base.DeepCopy();
			sketchTerrain.def = this.def;
			sketchTerrain.stuffForComparingSimilar = this.stuffForComparingSimilar;
			return sketchTerrain;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuffForComparingSimilar, "stuff");
			Scribe_Values.Look<bool>(ref this.treatSimilarAsSame, "treatSimilarAsSame", false, false);
		}

		
		public TerrainDef def;

		
		public ThingDef stuffForComparingSimilar;

		
		public bool treatSimilarAsSame;
	}
}
