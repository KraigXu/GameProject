using System;
using UnityEngine;

namespace Verse
{
	
	public class GenStep_TerrainPatches : GenStep
	{
		
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00042EEB File Offset: 0x000410EB
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = Mathf.RoundToInt((float)map.Area / 10000f * this.patchesPer10kCellsRange.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				float randomInRange = this.patchSizeRange.RandomInRange;
				IntVec3 a = CellFinder.RandomCell(map);
				foreach (IntVec3 b in GenRadial.RadialPatternInRadius(randomInRange / 2f))
				{
					IntVec3 c = a + b;
					if (c.InBounds(map))
					{
						map.terrainGrid.SetTerrain(c, this.terrainDef);
					}
				}
			}
		}

		
		public TerrainDef terrainDef;

		
		public FloatRange patchesPer10kCellsRange;

		
		public FloatRange patchSizeRange;
	}
}
