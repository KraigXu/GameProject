using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse
{
	
	public class WorldFloodFiller
	{
		
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile, traversalDistance);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		
		public void FloodFill(int rootTile, Predicate<int> passCheck, Predicate<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (int tile, int traversalDistance) => processor(tile), maxTilesToProcess, extraRootTiles);
		}

		
		public void FloodFill(int rootTile, Predicate<int> passCheck, Func<int, int, bool> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.", false);
			}
			this.working = true;
			this.ClearVisited();
			if (rootTile != -1 && extraRootTiles == null && !passCheck(rootTile))
			{
				this.working = false;
				return;
			}
			int tilesCount = Find.WorldGrid.TilesCount;
			int num = tilesCount;
			if (this.traversalDistance.Count != tilesCount)
			{
				this.traversalDistance.Clear();
				for (int i = 0; i < tilesCount; i++)
				{
					this.traversalDistance.Add(-1);
				}
			}
			WorldGrid worldGrid = Find.WorldGrid;
			List<int> tileIDToNeighbors_offsets = worldGrid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = worldGrid.tileIDToNeighbors_values;
			int num2 = 0;
			this.openSet.Clear();
			if (rootTile != -1)
			{
				this.visited.Add(rootTile);
				this.traversalDistance[rootTile] = 0;
				this.openSet.Enqueue(rootTile);
			}
			if (extraRootTiles == null)
			{
				goto IL_261;
			}
			this.visited.AddRange(extraRootTiles);
			IList<int> list = extraRootTiles as IList<int>;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					int num3 = list[j];
					this.traversalDistance[num3] = 0;
					this.openSet.Enqueue(num3);
				}
				goto IL_261;
			}
			using (IEnumerator<int> enumerator = extraRootTiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num4 = enumerator.Current;
					this.traversalDistance[num4] = 0;
					this.openSet.Enqueue(num4);
				}
				goto IL_261;
			}
			IL_16F:
			int num5 = this.openSet.Dequeue();
			int num6 = this.traversalDistance[num5];
			if (processor(num5, num6))
			{
				goto IL_272;
			}
			num2++;
			if (num2 == maxTilesToProcess)
			{
				goto IL_272;
			}
			int num7 = (num5 + 1 < tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_offsets[num5 + 1] : tileIDToNeighbors_values.Count;
			for (int k = tileIDToNeighbors_offsets[num5]; k < num7; k++)
			{
				int num8 = tileIDToNeighbors_values[k];
				if (this.traversalDistance[num8] == -1 && passCheck(num8))
				{
					this.visited.Add(num8);
					this.openSet.Enqueue(num8);
					this.traversalDistance[num8] = num6 + 1;
				}
			}
			if (this.openSet.Count > num)
			{
				Log.Error("Overflow on world flood fill (>" + num + " cells). Make sure we're not flooding over the same area after we check it.", false);
				this.working = false;
				return;
			}
			IL_261:
			if (this.openSet.Count > 0)
			{
				goto IL_16F;
			}
			IL_272:
			this.working = false;
		}

		
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				this.traversalDistance[this.visited[i]] = -1;
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}

		
		private bool working;

		
		private Queue<int> openSet = new Queue<int>();

		
		private List<int> traversalDistance = new List<int>();

		
		private List<int> visited = new List<int>();
	}
}
