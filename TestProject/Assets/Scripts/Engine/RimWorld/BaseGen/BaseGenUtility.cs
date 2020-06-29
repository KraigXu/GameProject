using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	
	public static class BaseGenUtility
	{
		
		public static ThingDef RandomCheapWallStuff(Faction faction, bool notVeryFlammable = false)
		{
			return BaseGenUtility.RandomCheapWallStuff((faction == null) ? TechLevel.Spacer : faction.def.techLevel, notVeryFlammable);
		}

		
		public static ThingDef RandomCheapWallStuff(TechLevel techLevel, bool notVeryFlammable = false)
		{
			if (techLevel.IsNeolithicOrWorse())
			{
				return ThingDefOf.WoodLog;
			}
			return (from d in DefDatabase<ThingDef>.AllDefsListForReading
			where BaseGenUtility.IsCheapWallStuff(d) && (!notVeryFlammable || d.BaseFlammability < 0.5f)
			select d).RandomElement<ThingDef>();
		}

		
		public static bool IsCheapWallStuff(ThingDef d)
		{
			return d.IsStuff && d.stuffProps.CanMake(ThingDefOf.Wall) && d.BaseMarketValue / d.VolumePerUnit < 5f;
		}

		
		public static ThingDef RandomHightechWallStuff()
		{
			if (Rand.Value < 0.15f)
			{
				return ThingDefOf.Plasteel;
			}
			return ThingDefOf.Steel;
		}

		
		public static TerrainDef RandomHightechFloorDef()
		{
			return Rand.Element<TerrainDef>(TerrainDefOf.Concrete, TerrainDefOf.Concrete, TerrainDefOf.PavedTile, TerrainDefOf.PavedTile, TerrainDefOf.MetalTile);
		}

		
		public static TerrainDef RandomBasicFloorDef(Faction faction, bool allowCarpet = false)
		{
			if (allowCarpet && (faction == null || !faction.def.techLevel.IsNeolithicOrWorse()) && Rand.Chance(0.1f))
			{
				return (from x in DefDatabase<TerrainDef>.AllDefsListForReading
				where x.IsCarpet
				select x).RandomElement<TerrainDef>();
			}
			return Rand.Element<TerrainDef>(TerrainDefOf.MetalTile, TerrainDefOf.PavedTile, TerrainDefOf.WoodPlankFloor, TerrainDefOf.TileSandstone);
		}

		
		public static bool TryRandomInexpensiveFloor(out TerrainDef floor, Predicate<TerrainDef> validator = null)
		{
			Func<TerrainDef, float> costCalculator = delegate(TerrainDef x)
			{
				List<ThingDefCountClass> list = x.CostListAdjusted(null, true);
				float num2 = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					num2 += (float)list[i].count * list[i].thingDef.BaseMarketValue;
				}
				return num2;
			};
			IEnumerable<TerrainDef> enumerable = from x in DefDatabase<TerrainDef>.AllDefs
			where x.BuildableByPlayer && x.terrainAffordanceNeeded != TerrainAffordanceDefOf.Bridgeable && !x.IsCarpet && (validator == null || validator(x)) && costCalculator(x) > 0f
			select x;
			float cheapest = -1f;
			foreach (TerrainDef arg in enumerable)
			{
				float num = costCalculator(arg);
				if (cheapest == -1f || num < cheapest)
				{
					cheapest = num;
				}
			}
			return (from x in enumerable
			where costCalculator(x) <= cheapest * 4f
			select x).TryRandomElement(out floor);
		}

		
		public static TerrainDef CorrespondingTerrainDef(ThingDef stuffDef, bool beautiful)
		{
			TerrainDef terrainDef = null;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].costList != null)
				{
					for (int j = 0; j < allDefsListForReading[i].costList.Count; j++)
					{
						if (allDefsListForReading[i].costList[j].thingDef == stuffDef && (terrainDef == null || (beautiful ? (terrainDef.statBases.GetStatOffsetFromList(StatDefOf.Beauty) < allDefsListForReading[i].statBases.GetStatOffsetFromList(StatDefOf.Beauty)) : (terrainDef.statBases.GetStatOffsetFromList(StatDefOf.Beauty) > allDefsListForReading[i].statBases.GetStatOffsetFromList(StatDefOf.Beauty)))))
						{
							terrainDef = allDefsListForReading[i];
						}
					}
				}
			}
			if (terrainDef == null)
			{
				terrainDef = TerrainDefOf.Concrete;
			}
			return terrainDef;
		}

		
		public static TerrainDef RegionalRockTerrainDef(int tile, bool beautiful)
		{
			ThingDef thingDef = Find.World.NaturalRockTypesIn(tile).RandomElementWithFallback(null);
			ThingDef thingDef2 = (thingDef != null) ? thingDef.building.mineableThing : null;
			return BaseGenUtility.CorrespondingTerrainDef((thingDef2 != null && thingDef2.butcherProducts != null && thingDef2.butcherProducts.Count > 0) ? thingDef2.butcherProducts[0].thingDef : null, beautiful);
		}

		
		public static bool AnyDoorAdjacentCardinalTo(IntVec3 cell, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (c.InBounds(map) && c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		
		public static bool AnyDoorAdjacentCardinalTo(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.AdjacentCellsCardinal)
			{
				if (c.InBounds(map) && c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		
		public static ThingDef WallStuffAt(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def == ThingDefOf.Wall)
			{
				return edifice.Stuff;
			}
			return null;
		}

		
		public static void CheckSpawnBridgeUnder(ThingDef thingDef, IntVec3 c, Rot4 rot)
		{
			if (thingDef.category != ThingCategory.Building)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			CellRect cellRect = GenAdj.OccupiedRect(c, rot, thingDef.size);
			BaseGenUtility.bridgeCells.Clear();
			foreach (IntVec3 intVec in cellRect)
			{
				if (!intVec.SupportsStructureType(map, thingDef.terrainAffordanceNeeded) && GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, intVec, map, Rot4.North, null, null))
				{
					BaseGenUtility.bridgeCells.Add(intVec);
				}
			}
			if (!BaseGenUtility.bridgeCells.Any<IntVec3>())
			{
				return;
			}
			if (thingDef.size.x != 1 || thingDef.size.z != 1)
			{
				for (int i = BaseGenUtility.bridgeCells.Count - 1; i >= 0; i--)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 intVec2 = BaseGenUtility.bridgeCells[i] + GenAdj.AdjacentCells[j];
						if (!BaseGenUtility.bridgeCells.Contains(intVec2) && intVec2.InBounds(map) && !intVec2.SupportsStructureType(map, thingDef.terrainAffordanceNeeded) && GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, intVec2, map, Rot4.North, null, null))
						{
							BaseGenUtility.bridgeCells.Add(intVec2);
						}
					}
				}
			}
			for (int k = 0; k < BaseGenUtility.bridgeCells.Count; k++)
			{
				map.terrainGrid.SetTerrain(BaseGenUtility.bridgeCells[k], TerrainDefOf.Bridge);
			}
		}

		
		[DebugOutput]
		private static void WallStuffs()
		{
			IEnumerable<ThingDef> dataSources = GenStuff.AllowedStuffsFor(ThingDefOf.Wall, TechLevel.Undefined);
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("cheap", (ThingDef d) => BaseGenUtility.IsCheapWallStuff(d).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("floor", (ThingDef d) => BaseGenUtility.CorrespondingTerrainDef(d, false).defName);
			array[3] = new TableDataGetter<ThingDef>("floor (beautiful)", (ThingDef d) => BaseGenUtility.CorrespondingTerrainDef(d, true).defName);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		private static List<IntVec3> bridgeCells = new List<IntVec3>();
	}
}
