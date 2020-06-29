﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class FeatureWorker_Cluster : FeatureWorker
	{
		
		
		protected virtual int MinRootGroupsInCluster
		{
			get
			{
				return this.def.minRootGroupsInCluster;
			}
		}

		
		
		protected virtual int MinRootGroupSize
		{
			get
			{
				return this.def.minRootGroupSize;
			}
		}

		
		
		protected virtual int MaxRootGroupSize
		{
			get
			{
				return this.def.maxRootGroupSize;
			}
		}

		
		
		protected virtual int MinOverallSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		
		
		protected virtual int MaxOverallSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		
		
		protected virtual int MaxSpaceBetweenRootGroups
		{
			get
			{
				return this.def.maxSpaceBetweenRootGroups;
			}
		}

		
		protected abstract bool IsRoot(int tile);

		
		protected virtual bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return true;
		}

		
		protected virtual bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].feature == null;
		}

		
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootTiles();
			this.CalculateRootsWithAreaInBetween();
			this.CalculateContiguousGroups();
		}

		
		private void CalculateRootTiles()
		{
			this.roots.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.roots.Add(i);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
		}

		
		private void CalculateRootsWithAreaInBetween()
		{
			this.rootsWithAreaInBetween.Clear();
			this.rootsWithAreaInBetween.AddRange(this.roots);
			GenPlanetMorphology.Close(this.rootsWithAreaInBetween, this.MaxSpaceBetweenRootGroups);
			this.rootsWithAreaInBetweenSet.Clear();
			this.rootsWithAreaInBetweenSet.AddRange(this.rootsWithAreaInBetween);
		}

		
		private void CalculateContiguousGroups()
		{
			//WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			//WorldGrid worldGrid = Find.WorldGrid;
			//int minRootGroupSize = this.MinRootGroupSize;
			//int maxRootGroupSize = this.MaxRootGroupSize;
			//int minOverallSize = this.MinOverallSize;
			//int maxOverallSize = this.MaxOverallSize;
			//int minRootGroupsInCluster = this.MinRootGroupsInCluster;
			//FeatureWorker.ClearVisited();
			//FeatureWorker.ClearGroupSizes();
			//FeatureWorker.ClearGroupIDs();

			//for (int i = 0; i < this.roots.Count; i++)
			//{
			//	int num = this.roots[i];
			//	if (!FeatureWorker.visited[num])
			//	{
			//		bool anyMember = false;
			//		FeatureWorker_Cluster.tmpGroup.Clear();
			//		WorldFloodFiller worldFloodFiller2 = worldFloodFiller;
			//		int rootTile = num;
			//		Predicate<int> passCheck;
			//		if ((passCheck ) == null)
			//		{
			//			passCheck = ( ((int x) => this.rootsSet.Contains(x)));
			//		}
			//		worldFloodFiller2.FloodFill(rootTile, passCheck, delegate(int x)
			//		{
			//			FeatureWorker.visited[x] = true;
			//			FeatureWorker_Cluster.tmpGroup.Add(x);
			//			bool flag2;
			//			if (!anyMember && this.IsMember(x, out flag2))
			//			{
			//				anyMember = true;
			//			}
			//		}, int.MaxValue, null);
			//		for (int j = 0; j < FeatureWorker_Cluster.tmpGroup.Count; j++)
			//		{
			//			FeatureWorker.groupSize[FeatureWorker_Cluster.tmpGroup[j]] = FeatureWorker_Cluster.tmpGroup.Count;
			//			if (anyMember)
			//			{
			//				FeatureWorker.groupID[FeatureWorker_Cluster.tmpGroup[j]] = i + 1;
			//			}
			//		}
			//	}
			//}
			//FeatureWorker.ClearVisited();

			//



			//for (int k = 0; k < this.roots.Count; k++)
			//{
			//	int num2 = this.roots[k];
			//	if (!FeatureWorker.visited[num2] && FeatureWorker.groupSize[num2] >= minRootGroupSize && FeatureWorker.groupSize[num2] <= maxRootGroupSize && FeatureWorker.groupSize[num2] <= maxOverallSize)
			//	{
			//		this.currentGroup.Clear();
			//		this.visitedValidGroupIDs.Clear();
			//		WorldFloodFiller worldFloodFiller3 = worldFloodFiller;
			//		int rootTile2 = num2;
			//		Predicate<int> passCheck2;
			//		if ((passCheck2 ) == null)
			//		{
			//			passCheck2 = ( delegate(int x)
			//			{
			//				bool flag2;
			//				return this.rootsWithAreaInBetweenSet.Contains(x) && this.CanTraverse(x, out flag2) && (!flag2 || !this.rootsSet.Contains(x) || (FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize));
			//			});
			//		}
			//		Action<int> processor;
			//		if ((processor ) == null)
			//		{
			//			processor = ( delegate(int x)
			//			{
			//				FeatureWorker.visited[x] = true;
			//				this.currentGroup.Add(x);
			//				if (FeatureWorker.groupID[x] != 0 && FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize)
			//				{
			//					this.visitedValidGroupIDs.Add(FeatureWorker.groupID[x]);
			//				}
			//			});
			//		}
			//		worldFloodFiller3.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
			//		if (this.currentGroup.Count >= minOverallSize && this.currentGroup.Count <= maxOverallSize && this.visitedValidGroupIDs.Count >= minRootGroupsInCluster)
			//		{
			//			if (!this.def.canTouchWorldEdge)
			//			{
			//				List<int> list = this.currentGroup;
			//				Predicate<int> predicate;
			//				if ((predicate=default ) == null)
			//				{
			//					predicate = ( ((int x) => worldGrid.IsOnEdge(x)));
			//				}
			//				if (list.Any(predicate))
			//				{
			//					goto IL_395;
			//				}
			//			}
			//			this.currentGroupMembers.Clear();
			//			for (int l = 0; l < this.currentGroup.Count; l++)
			//			{
			//				int num3 = this.currentGroup[l];
			//				bool flag;
			//				if (this.IsMember(num3, out flag) && (!flag || !this.rootsSet.Contains(num3) || (FeatureWorker.groupSize[num3] >= minRootGroupSize && FeatureWorker.groupSize[num3] <= maxRootGroupSize)))
			//				{
			//					this.currentGroupMembers.Add(this.currentGroup[l]);
			//				}
			//			}
			//			if (this.currentGroupMembers.Count >= minOverallSize)
			//			{
			//				List<int> list2 = this.currentGroup;
			//				Predicate<int> predicate2;
			//				if ((predicate2=default ) == null)
			//				{
			//					predicate2 = ( ((int x) => worldGrid[x].feature == null));
			//				}
			//				if (list2.Any(predicate2))
			//				{
			//					List<int> list3 = this.currentGroup;
			//					Predicate<int> match;
			//					if ((match ) == null)
			//					{
			//						match = ( ((int x) => worldGrid[x].feature != null));
			//					}
			//					list3.RemoveAll(match);
			//				}
			//				base.AddFeature(this.currentGroupMembers, this.currentGroup);
			//			}
			//		}
			//	}
			//	IL_395:;
			//}
		}

		
		private List<int> roots = new List<int>();

		
		private HashSet<int> rootsSet = new HashSet<int>();

		
		private List<int> rootsWithAreaInBetween = new List<int>();

		
		private HashSet<int> rootsWithAreaInBetweenSet = new HashSet<int>();

		
		private List<int> currentGroup = new List<int>();

		
		private List<int> currentGroupMembers = new List<int>();

		
		private HashSet<int> visitedValidGroupIDs = new HashSet<int>();

		
		private static List<int> tmpGroup = new List<int>();
	}
}
