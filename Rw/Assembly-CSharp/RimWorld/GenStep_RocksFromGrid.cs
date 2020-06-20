using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A55 RID: 2645
	public class GenStep_RocksFromGrid : GenStep
	{
		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003E8D RID: 16013 RVA: 0x0014B947 File Offset: 0x00149B47
		public override int SeedPart
		{
			get
			{
				return 1182952823;
			}
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x0014B950 File Offset: 0x00149B50
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

		// Token: 0x06003E8F RID: 16015 RVA: 0x0014B9D0 File Offset: 0x00149BD0
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
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				if (!visited[intVec2] && this.IsNaturalRoofAt(intVec2, map))
				{
					toRemove.Clear();
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec2;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => this.IsNaturalRoofAt(x, map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							visited[x] = true;
							toRemove.Add(x);
						});
					}
					floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
					if (toRemove.Count < 20)
					{
						for (int j = 0; j < toRemove.Count; j++)
						{
							map.roofGrid.SetRoof(toRemove[j], null);
						}
					}
				}
			}
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

		// Token: 0x06003E90 RID: 16016 RVA: 0x0014BD2C File Offset: 0x00149F2C
		private bool IsNaturalRoofAt(IntVec3 c, Map map)
		{
			return c.Roofed(map) && c.GetRoof(map).isNatural;
		}

		// Token: 0x04002470 RID: 9328
		private float maxMineableValue = float.MaxValue;

		// Token: 0x04002471 RID: 9329
		private const int MinRoofedCellsPerGroup = 20;

		// Token: 0x02001A4E RID: 6734
		private class RoofThreshold
		{
			// Token: 0x040063F8 RID: 25592
			public RoofDef roofDef;

			// Token: 0x040063F9 RID: 25593
			public float minGridVal;
		}
	}
}
