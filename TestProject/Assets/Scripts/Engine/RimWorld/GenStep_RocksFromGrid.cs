﻿using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class GenStep_RocksFromGrid : GenStep
	{
		
		
		public override int SeedPart
		{
			get
			{
				return 1182952823;
			}
		}

		
		public static ThingDef RockDefAt(IntVec3 c)
		{
			ThingDef thingDef = null;
			float num = -999999f;
			for (int i = 0; i < RockNoises.rockNoises.Count; i++)
			{
				float value = RockNoises.rockNoises[i].noise.GetValue(c);
				if (value > num)
				{
					thingDef = RockNoises.rockNoises[i].rockDef;
					num = value;
				}
			}
			if (thingDef == null)
			{
				Log.ErrorOnce("Did not get rock def to generate at " + c, 50812, false);
				thingDef = ThingDefOf.Sandstone;
			}
			return thingDef;
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.TileInfo.WaterCovered)
			{
				return;
			}
			map.regionAndRoomUpdater.Enabled = false;
			float num = 0.7f;
			List<GenStep_RocksFromGrid.RoofThreshold> list = new List<GenStep_RocksFromGrid.RoofThreshold>();
			list.Add(new GenStep_RocksFromGrid.RoofThreshold
			{
				roofDef = RoofDefOf.RoofRockThick,
				minGridVal = num * 1.14f
			});
			list.Add(new GenStep_RocksFromGrid.RoofThreshold
			{
				roofDef = RoofDefOf.RoofRockThin,
				minGridVal = num * 1.04f
			});
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float num2 = elevation[intVec];
				if (num2 > num)
				{
					if (caves[intVec] <= 0f)
					{
						GenSpawn.Spawn(GenStep_RocksFromGrid.RockDefAt(intVec), intVec, map, WipeMode.Vanish);
					}
					for (int i = 0; i < list.Count; i++)
					{
						if (num2 > list[i].minGridVal)
						{
							map.roofGrid.SetRoof(intVec, list[i].roofDef);
							break;
						}
					}
				}
			}
			BoolGrid visited = new BoolGrid(map);
			List<IntVec3> toRemove = new List<IntVec3>();

			//
			//foreach (IntVec3 intVec2 in map.AllCells)
			//{
			//	if (!visited[intVec2] && this.IsNaturalRoofAt(intVec2, map))
			//	{
			//		toRemove.Clear();
			//		FloodFiller floodFiller = map.floodFiller;
			//		IntVec3 root = intVec2;
			//		Predicate<IntVec3> passCheck;
			//		if ((passCheck ) == null)
			//		{
			//			passCheck = ( ((IntVec3 x) => this.IsNaturalRoofAt(x, map)));
			//		}
			//		Action<IntVec3> processor;
			//		if ((processor ) == null)
			//		{
			//			processor = ( delegate(IntVec3 x)
			//			{
			//				visited[x] = true;
			//				toRemove.Add(x);
			//			});
			//		}
			//		floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
			//		if (toRemove.Count < 20)
			//		{
			//			for (int j = 0; j < toRemove.Count; j++)
			//			{
			//				map.roofGrid.SetRoof(toRemove[j], null);
			//			}
			//		}
			//	}
			//}
			GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable();
			genStep_ScatterLumpsMineable.maxValue = this.maxMineableValue;
			float num3 = 10f;
			switch (Find.WorldGrid[map.Tile].hilliness)
			{
			case Hilliness.Flat:
				num3 = 4f;
				break;
			case Hilliness.SmallHills:
				num3 = 8f;
				break;
			case Hilliness.LargeHills:
				num3 = 11f;
				break;
			case Hilliness.Mountainous:
				num3 = 15f;
				break;
			case Hilliness.Impassable:
				num3 = 16f;
				break;
			}
			genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(num3, num3);
			genStep_ScatterLumpsMineable.Generate(map, parms);
			map.regionAndRoomUpdater.Enabled = true;
		}

		
		private bool IsNaturalRoofAt(IntVec3 c, Map map)
		{
			return c.Roofed(map) && c.GetRoof(map).isNatural;
		}

		
		private float maxMineableValue = float.MaxValue;

		
		private const int MinRoofedCellsPerGroup = 20;

		
		private class RoofThreshold
		{
			
			public RoofDef roofDef;

			
			public float minGridVal;
		}
	}
}
