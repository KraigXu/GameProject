﻿using System;
using System.Collections.Generic;

namespace Verse
{
	
	public static class RegionTraverser
	{
		
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

		
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		
		public static bool ShouldCountRegion(Region r)
		{
			return !r.IsDoorway;
		}

		
		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		
		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
		}

		
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

		
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		
		public static int NumWorkers = 8;

		
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		
		private class BFSWorker
		{
			
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			
			public void Clear()
			{
				this.open.Clear();
			}

			
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			
			private void FinalizeSearch()
			{
			}

			
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

			
			private Queue<Region> open = new Queue<Region>();

			
			private int numRegionsProcessed;

			
			private uint closedIndex = 1u;

			
			private int closedArrayPos;

			
			private const int skippableRegionSize = 4;
		}
	}
}
