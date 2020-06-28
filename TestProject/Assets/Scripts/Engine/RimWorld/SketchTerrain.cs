using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA6 RID: 2726
	public class SketchTerrain : SketchBuildable
	{
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x0600406F RID: 16495 RVA: 0x0015847B File Offset: 0x0015667B
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x00158483 File Offset: 0x00156683
		public override ThingDef Stuff
		{
			get
			{
				return this.stuffForComparingSimilar;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x0015848B File Offset: 0x0015668B
		public override CellRect OccupiedRect
		{
			get
			{
				return CellRect.SingleCell(this.pos);
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public override float SpawnOrder
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004073 RID: 16499 RVA: 0x00158498 File Offset: 0x00156698
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

		// Token: 0x06004074 RID: 16500 RVA: 0x001584F0 File Offset: 0x001566F0
		public override void DrawGhost(IntVec3 at, Color color)
		{
			ThingDef blueprintDef = this.def.blueprintDef;
			GraphicDatabase.Get(blueprintDef.graphic.GetType(), blueprintDef.graphic.path, blueprintDef.graphic.Shader, blueprintDef.graphic.drawSize, color, Color.white, blueprintDef.graphicData, null).DrawFromDef(at.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), Rot4.North, this.def.blueprintDef, 0f);
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0015856A File Offset: 0x0015676A
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return at.InBounds(map) && this.IsSameOrSimilar(at.GetTerrain(map));
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x00158584 File Offset: 0x00156784
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

		// Token: 0x06004077 RID: 16503 RVA: 0x001585FC File Offset: 0x001567FC
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

		// Token: 0x06004078 RID: 16504 RVA: 0x00158664 File Offset: 0x00156864
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !at.InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, Rot4.North, map, wipeIfCollides, thingToIgnore, null, null).Accepted;
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x001586B0 File Offset: 0x001568B0
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

		// Token: 0x0600407A RID: 16506 RVA: 0x00158788 File Offset: 0x00156988
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, Rot4.North, null, null);
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x001587A0 File Offset: 0x001569A0
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

		// Token: 0x0600407C RID: 16508 RVA: 0x00158810 File Offset: 0x00156A10
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

		// Token: 0x0600407D RID: 16509 RVA: 0x001588A0 File Offset: 0x00156AA0
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchTerrain sketchTerrain = other as SketchTerrain;
			return sketchTerrain != null && (sketchTerrain == this || (this.IsSameOrSimilar(sketchTerrain.Buildable) && this.pos == sketchTerrain.pos));
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x001588E0 File Offset: 0x00156AE0
		public override SketchEntity DeepCopy()
		{
			SketchTerrain sketchTerrain = (SketchTerrain)base.DeepCopy();
			sketchTerrain.def = this.def;
			sketchTerrain.stuffForComparingSimilar = this.stuffForComparingSimilar;
			return sketchTerrain;
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x00158905 File Offset: 0x00156B05
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuffForComparingSimilar, "stuff");
			Scribe_Values.Look<bool>(ref this.treatSimilarAsSame, "treatSimilarAsSame", false, false);
		}

		// Token: 0x04002569 RID: 9577
		public TerrainDef def;

		// Token: 0x0400256A RID: 9578
		public ThingDef stuffForComparingSimilar;

		// Token: 0x0400256B RID: 9579
		public bool treatSimilarAsSame;
	}
}
