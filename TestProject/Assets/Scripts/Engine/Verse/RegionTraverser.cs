using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001C6 RID: 454
	public static class RegionTraverser
	{
		// Token: 0x06000CAB RID: 3243 RVA: 0x000486DC File Offset: 0x000468DC
		public static Room FloodAndSetRooms(Region root, Map map, Room existingRoom)
		{
			Room floodingRoom;
			if (existingRoom == null)
			{
				floodingRoom = Room.MakeNew(map);
			}
			else
			{
				floodingRoom = existingRoom;
			}
			root.Room = floodingRoom;
			if (!root.type.AllowsMultipleRegionsPerRoom())
			{
				return floodingRoom;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.Room != floodingRoom;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.Room = floodingRoom;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			return floodingRoom;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0004876C File Offset: 0x0004696C
		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (!root.type.AllowsMultipleRegionsPerRoom())
			{
				return;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.newRegionGroupIndex < 0;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.newRegionGroupIndex = newRegionGroupIndex;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000487E0 File Offset: 0x000469E0
		public static bool WithinRegions(this IntVec3 A, IntVec3 B, Map map, int regionLookCount, TraverseParms traverseParams, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = A.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return false;
			}
			Region regB = B.GetRegion(map, traversableRegionTypes);
			if (regB == null)
			{
				return false;
			}
			if (region == regB)
			{
				return true;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
			bool found = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r == regB)
				{
					found = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, regionLookCount, traversableRegionTypes);
			return found;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00048860 File Offset: 0x00046A60
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00048890 File Offset: 0x00046A90
		public static bool ShouldCountRegion(Region r)
		{
			return !r.IsDoorway;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0004889B File Offset: 0x00046A9B
		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x000488C8 File Offset: 0x00046AC8
		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00048900 File Offset: 0x00046B00
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00048928 File Offset: 0x00046B28
		public static void BreadthFirstTraverse(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (RegionTraverser.freeWorkers.Count == 0)
			{
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers + ", or a bug has put this system in an inconsistent state. Resetting.", false);
				return;
			}
			if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.", false);
				return;
			}
			RegionTraverser.BFSWorker bfsworker = RegionTraverser.freeWorkers.Dequeue();
			try
			{
				bfsworker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
			catch (Exception ex)
			{
				Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString(), false);
			}
			finally
			{
				bfsworker.Clear();
				RegionTraverser.freeWorkers.Enqueue(bfsworker);
			}
		}

		// Token: 0x040009F2 RID: 2546
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x040009F3 RID: 2547
		public static int NumWorkers = 8;

		// Token: 0x040009F4 RID: 2548
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x020013DC RID: 5084
		private class BFSWorker
		{
			// Token: 0x060077FF RID: 30719 RVA: 0x00292424 File Offset: 0x00290624
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x06007800 RID: 30720 RVA: 0x00292445 File Offset: 0x00290645
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x06007801 RID: 30721 RVA: 0x00292454 File Offset: 0x00290654
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			// Token: 0x06007802 RID: 30722 RVA: 0x00002681 File Offset: 0x00000881
			private void FinalizeSearch()
			{
			}

			// Token: 0x06007803 RID: 30723 RVA: 0x002924AC File Offset: 0x002906AC
			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) == RegionType.None)
				{
					return;
				}
				this.closedIndex += 1u;
				this.open.Clear();
				this.numRegionsProcessed = 0;
				this.QueueNewOpenRegion(root);
				while (this.open.Count > 0)
				{
					Region region = this.open.Dequeue();
					if (DebugViewSettings.drawRegionTraversal)
					{
						region.Debug_Notify_Traversed();
					}
					if (regionProcessor != null && regionProcessor(region))
					{
						this.FinalizeSearch();
						return;
					}
					if (RegionTraverser.ShouldCountRegion(region))
					{
						this.numRegionsProcessed++;
					}
					if (this.numRegionsProcessed >= maxRegions)
					{
						this.FinalizeSearch();
						return;
					}
					for (int i = 0; i < region.links.Count; i++)
					{
						RegionLink regionLink = region.links[i];
						for (int j = 0; j < 2; j++)
						{
							Region region2 = regionLink.regions[j];
							if (region2 != null && region2.closedIndex[this.closedArrayPos] != this.closedIndex && (region2.type & traversableRegionTypes) != RegionType.None && (entryCondition == null || entryCondition(region, region2)))
							{
								this.QueueNewOpenRegion(region2);
							}
						}
					}
				}
				this.FinalizeSearch();
			}

			// Token: 0x04004B84 RID: 19332
			private Queue<Region> open = new Queue<Region>();

			// Token: 0x04004B85 RID: 19333
			private int numRegionsProcessed;

			// Token: 0x04004B86 RID: 19334
			private uint closedIndex = 1u;

			// Token: 0x04004B87 RID: 19335
			private int closedArrayPos;

			// Token: 0x04004B88 RID: 19336
			private const int skippableRegionSize = 4;
		}
	}
}
