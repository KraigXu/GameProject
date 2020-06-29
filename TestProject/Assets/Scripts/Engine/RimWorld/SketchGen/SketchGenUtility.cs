using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	
	public static class SketchGenUtility
	{
		
		public static bool IsStuffAllowed(ThingDef stuff, bool allowWood, Map useOnlyStonesAvailableOnMap, bool allowFlammableWalls, ThingDef stuffFor)
		{
			return (allowWood || stuff != ThingDefOf.WoodLog) && (allowFlammableWalls || stuffFor != ThingDefOf.Wall || StatDefOf.Flammability.Worker.GetValueAbstract(stuffFor, stuff) <= 0f) && (useOnlyStonesAvailableOnMap == null || stuff.stuffProps.SourceNaturalRock == null || !stuff.stuffProps.SourceNaturalRock.IsNonResourceNaturalRock || Find.World.NaturalRockTypesIn(useOnlyStonesAvailableOnMap.Tile).Contains(stuff.stuffProps.SourceNaturalRock));
		}

		
		[Obsolete("Only used for mod compatibility")]
		public static bool IsFloorAllowed(TerrainDef floor, bool allowWoodenFloor, bool allowConcrete, Map useOnlyStonesAvailableOnMap, bool onlyBuildableByPlayer)
		{
			return SketchGenUtility.IsFloorAllowed_NewTmp(floor, allowWoodenFloor, allowConcrete, useOnlyStonesAvailableOnMap, onlyBuildableByPlayer, true);
		}

		
		public static bool IsFloorAllowed_NewTmp(TerrainDef floor, bool allowWoodenFloor, bool allowConcrete, Map useOnlyStonesAvailableOnMap, bool onlyBuildableByPlayer, bool onlyStoneFloor)
		{
			if (!allowWoodenFloor && floor == TerrainDefOf.WoodPlankFloor)
			{
				return false;
			}
			if (!allowConcrete && floor == TerrainDefOf.Concrete)
			{
				return false;
			}
			if (onlyStoneFloor)
			{
				List<ThingDefCountClass> list = floor.CostListAdjusted(null, true);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].thingDef.stuffProps != null && !list[i].thingDef.stuffProps.categories.Contains(StuffCategoryDefOf.Stony))
					{
						return false;
					}
				}
			}
			if (useOnlyStonesAvailableOnMap != null)
			{
				bool flag = false;
				bool flag2 = true;
				List<ThingDefCountClass> list2 = floor.CostListAdjusted(null, true);
				for (int j = 0; j < list2.Count; j++)
				{
					if (list2[j].thingDef.stuffProps != null && list2[j].thingDef.stuffProps.SourceNaturalRock != null && list2[j].thingDef.stuffProps.SourceNaturalRock.IsNonResourceNaturalRock)
					{
						flag = true;
						flag2 = (flag2 && Find.World.NaturalRockTypesIn(useOnlyStonesAvailableOnMap.Tile).Contains(list2[j].thingDef.stuffProps.SourceNaturalRock));
					}
				}
				if (flag && !flag2)
				{
					return false;
				}
			}
			return !onlyBuildableByPlayer || SketchGenUtility.PlayerCanBuildNow(floor);
		}

		
		public static CellRect FindBiggestRectAt(IntVec3 c, CellRect outerRect, Sketch sketch, HashSet<IntVec3> processed, Predicate<IntVec3> canTraverse)
		{
			if (processed.Contains(c) || !canTraverse(c))
			{
				return CellRect.Empty;
			}
			CellRect cellRect = CellRect.SingleCell(c);
			bool flag;
			do
			{
				flag = false;
				if (cellRect.maxX < outerRect.maxX)
				{
					bool flag2 = false;
					foreach (IntVec3 intVec in cellRect.GetEdgeCells(Rot4.East))
					{
						//intVec.x++;
						if (processed.Contains(intVec) || !canTraverse(intVec))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						cellRect.maxX++;
						flag = true;
					}
				}
				if (cellRect.minX > outerRect.minX)
				{
					bool flag3 = false;
					foreach (IntVec3 intVec2 in cellRect.GetEdgeCells(Rot4.West))
					{
					//	intVec2.x--;
						if (processed.Contains(intVec2) || !canTraverse(intVec2))
						{
							flag3 = true;
							break;
						}
					}
					if (!flag3)
					{
						cellRect.minX--;
						flag = true;
					}
				}
				if (cellRect.maxZ < outerRect.maxZ)
				{
					bool flag4 = false;
					foreach (IntVec3 intVec3 in cellRect.GetEdgeCells(Rot4.North))
					{
					//	intVec3.z++;
						if (processed.Contains(intVec3) || !canTraverse(intVec3))
						{
							flag4 = true;
							break;
						}
					}
					if (!flag4)
					{
						cellRect.maxZ++;
						flag = true;
					}
				}
				if (cellRect.minZ > outerRect.minZ)
				{
					bool flag5 = false;
					foreach (IntVec3 intVec4 in cellRect.GetEdgeCells(Rot4.South))
					{
						//intVec4.z--;
						if (processed.Contains(intVec4) || !canTraverse(intVec4))
						{
							flag5 = true;
							break;
						}
					}
					if (!flag5)
					{
						cellRect.minZ--;
						flag = true;
					}
				}
			}
			while (flag);
			foreach (IntVec3 item in cellRect)
			{
				processed.Add(item);
			}
			return cellRect;
		}

		
		public static CellRect FindBiggestRect(Sketch sketch, Predicate<IntVec3> canTraverse)
		{
			CellRect result;
			try
			{
				CellRect cellRect = CellRect.Empty;
				for (int i = 0; i < 3; i++)
				{
					SketchGenUtility.tmpProcessed.Clear();
					foreach (IntVec3 c in sketch.OccupiedRect.InRandomOrder(null))
					{
						CellRect cellRect2 = SketchGenUtility.FindBiggestRectAt(c, sketch.OccupiedRect, sketch, SketchGenUtility.tmpProcessed, canTraverse);
						if (cellRect2.Area > cellRect.Area)
						{
							cellRect = cellRect2;
						}
					}
				}
				result = cellRect;
			}
			finally
			{
				SketchGenUtility.tmpProcessed.Clear();
			}
			return result;
		}

		
		public static bool PlayerCanBuildNow(BuildableDef buildable)
		{
			return buildable.BuildableByPlayer && buildable.IsResearchFinished;
		}

		
		private static HashSet<IntVec3> tmpProcessed = new HashSet<IntVec3>();
	}
}
