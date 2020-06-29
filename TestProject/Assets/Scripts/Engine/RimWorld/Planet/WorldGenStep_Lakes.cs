using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldGenStep_Lakes : WorldGenStep
	{
		
		// (get) Token: 0x06006B09 RID: 27401 RVA: 0x00255768 File Offset: 0x00253968
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		
		public override void GenerateFresh(string seed)
		{
			this.GenerateLakes();
		}

		
		private void GenerateLakes()
		{
			WorldGrid grid = Find.WorldGrid;
			bool[] touched = new bool[grid.TilesCount];
			List<int> oceanChunk = new List<int>();

			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (!touched[i] && grid[i].biome == BiomeDefOf.Ocean)
				{
					WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
					int rootTile = i;
					Predicate<int> passCheck = ((int tid) => grid[tid].biome == BiomeDefOf.Ocean);

					Action<int> processor = delegate (int tid)
					{
						oceanChunk.Add(tid);
						touched[tid] = true;
					};

					worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, null);
					if (oceanChunk.Count <= 15)
					{
						for (int j = 0; j < oceanChunk.Count; j++)
						{
							grid[oceanChunk[j]].biome = BiomeDefOf.Lake;
						}
					}
					oceanChunk.Clear();
				}
			}
		}

		
		private const int LakeMaxSize = 15;
	}
}
