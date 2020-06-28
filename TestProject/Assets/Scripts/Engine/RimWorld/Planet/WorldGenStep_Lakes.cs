using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001215 RID: 4629
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06006B09 RID: 27401 RVA: 0x00255768 File Offset: 0x00253968
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x0025576F File Offset: 0x0025396F
		public override void GenerateFresh(string seed)
		{
			this.GenerateLakes();
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x00255778 File Offset: 0x00253978
		private void GenerateLakes()
		{
			WorldGrid grid = Find.WorldGrid;
			bool[] touched = new bool[grid.TilesCount];
			List<int> oceanChunk = new List<int>();
			Predicate<int> <>9__0;
			Action<int> <>9__1;
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (!touched[i] && grid[i].biome == BiomeDefOf.Ocean)
				{
					WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
					int rootTile = i;
					Predicate<int> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((int tid) => grid[tid].biome == BiomeDefOf.Ocean));
					}
					Action<int> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(int tid)
						{
							oceanChunk.Add(tid);
							touched[tid] = true;
						});
					}
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

		// Token: 0x040042DC RID: 17116
		private const int LakeMaxSize = 15;
	}
}
