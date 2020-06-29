using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		
		public override void GenerateWhereAppropriate()
		{
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			this.edgeTiles.Clear();
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.edgeTiles.Add(i);
				}
			}
			if (!this.edgeTiles.Any<int>())
			{
				return;
			}
			this.group.Clear();
			Find.WorldFloodFiller.FloodFill(-1, (int x) => this.CanTraverse(x), delegate(int tile, int traversalDist)
			{
				this.group.Add(tile);
				return false;
			}, int.MaxValue, this.edgeTiles);
			this.group.RemoveAll((int x) => worldGrid[x].feature != null);
			if (this.group.Count < this.def.minSize || this.group.Count > this.def.maxSize)
			{
				return;
			}
			base.AddFeature(this.group, this.group);
		}

		
		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		
		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}

		
		private List<int> group = new List<int>();

		
		private List<int> edgeTiles = new List<int>();
	}
}
