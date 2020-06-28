using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C1 RID: 4545
	public abstract class FeatureWorker_Cluster : FeatureWorker
	{
		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x06006919 RID: 26905 RVA: 0x0024B811 File Offset: 0x00249A11
		protected virtual int MinRootGroupsInCluster
		{
			get
			{
				return this.def.minRootGroupsInCluster;
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x0600691A RID: 26906 RVA: 0x0024B81E File Offset: 0x00249A1E
		protected virtual int MinRootGroupSize
		{
			get
			{
				return this.def.minRootGroupSize;
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x0600691B RID: 26907 RVA: 0x0024B82B File Offset: 0x00249A2B
		protected virtual int MaxRootGroupSize
		{
			get
			{
				return this.def.maxRootGroupSize;
			}
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x0600691C RID: 26908 RVA: 0x0024B838 File Offset: 0x00249A38
		protected virtual int MinOverallSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x0600691D RID: 26909 RVA: 0x0024B845 File Offset: 0x00249A45
		protected virtual int MaxOverallSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x0600691E RID: 26910 RVA: 0x0024B852 File Offset: 0x00249A52
		protected virtual int MaxSpaceBetweenRootGroups
		{
			get
			{
				return this.def.maxSpaceBetweenRootGroups;
			}
		}

		// Token: 0x0600691F RID: 26911
		protected abstract bool IsRoot(int tile);

		// Token: 0x06006920 RID: 26912 RVA: 0x0024B85F File Offset: 0x00249A5F
		protected virtual bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return true;
		}

		// Token: 0x06006921 RID: 26913 RVA: 0x0024B865 File Offset: 0x00249A65
		protected virtual bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06006922 RID: 26914 RVA: 0x0024B87D File Offset: 0x00249A7D
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootTiles();
			this.CalculateRootsWithAreaInBetween();
			this.CalculateContiguousGroups();
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x0024B894 File Offset: 0x00249A94
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

		// Token: 0x06006924 RID: 26916 RVA: 0x0024B8F4 File Offset: 0x00249AF4
		private void CalculateRootsWithAreaInBetween()
		{
			this.rootsWithAreaInBetween.Clear();
			this.rootsWithAreaInBetween.AddRange(this.roots);
			GenPlanetMorphology.Close(this.rootsWithAreaInBetween, this.MaxSpaceBetweenRootGroups);
			this.rootsWithAreaInBetweenSet.Clear();
			this.rootsWithAreaInBetweenSet.AddRange(this.rootsWithAreaInBetween);
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x0024B94C File Offset: 0x00249B4C
		private void CalculateContiguousGroups()
		{
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			WorldGrid worldGrid = Find.WorldGrid;
			int minRootGroupSize = this.MinRootGroupSize;
			int maxRootGroupSize = this.MaxRootGroupSize;
			int minOverallSize = this.MinOverallSize;
			int maxOverallSize = this.MaxOverallSize;
			int minRootGroupsInCluster = this.MinRootGroupsInCluster;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			FeatureWorker.ClearGroupIDs();
			Predicate<int> <>9__0;
			for (int i = 0; i < this.roots.Count; i++)
			{
				int num = this.roots[i];
				if (!FeatureWorker.visited[num])
				{
					bool anyMember = false;
					FeatureWorker_Cluster.tmpGroup.Clear();
					WorldFloodFiller worldFloodFiller2 = worldFloodFiller;
					int rootTile = num;
					Predicate<int> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((int x) => this.rootsSet.Contains(x)));
					}
					worldFloodFiller2.FloodFill(rootTile, passCheck, delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						FeatureWorker_Cluster.tmpGroup.Add(x);
						bool flag2;
						if (!anyMember && this.IsMember(x, out flag2))
						{
							anyMember = true;
						}
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Cluster.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Cluster.tmpGroup[j]] = FeatureWorker_Cluster.tmpGroup.Count;
						if (anyMember)
						{
							FeatureWorker.groupID[FeatureWorker_Cluster.tmpGroup[j]] = i + 1;
						}
					}
				}
			}
			FeatureWorker.ClearVisited();
			Predicate<int> <>9__2;
			Action<int> <>9__3;
			Predicate<int> <>9__4;
			Predicate<int> <>9__5;
			Predicate<int> <>9__6;
			for (int k = 0; k < this.roots.Count; k++)
			{
				int num2 = this.roots[k];
				if (!FeatureWorker.visited[num2] && FeatureWorker.groupSize[num2] >= minRootGroupSize && FeatureWorker.groupSize[num2] <= maxRootGroupSize && FeatureWorker.groupSize[num2] <= maxOverallSize)
				{
					this.currentGroup.Clear();
					this.visitedValidGroupIDs.Clear();
					WorldFloodFiller worldFloodFiller3 = worldFloodFiller;
					int rootTile2 = num2;
					Predicate<int> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = delegate(int x)
						{
							bool flag2;
							return this.rootsWithAreaInBetweenSet.Contains(x) && this.CanTraverse(x, out flag2) && (!flag2 || !this.rootsSet.Contains(x) || (FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize));
						});
					}
					Action<int> processor;
					if ((processor = <>9__3) == null)
					{
						processor = (<>9__3 = delegate(int x)
						{
							FeatureWorker.visited[x] = true;
							this.currentGroup.Add(x);
							if (FeatureWorker.groupID[x] != 0 && FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize)
							{
								this.visitedValidGroupIDs.Add(FeatureWorker.groupID[x]);
							}
						});
					}
					worldFloodFiller3.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
					if (this.currentGroup.Count >= minOverallSize && this.currentGroup.Count <= maxOverallSize && this.visitedValidGroupIDs.Count >= minRootGroupsInCluster)
					{
						if (!this.def.canTouchWorldEdge)
						{
							List<int> list = this.currentGroup;
							Predicate<int> predicate;
							if ((predicate = <>9__4) == null)
							{
								predicate = (<>9__4 = ((int x) => worldGrid.IsOnEdge(x)));
							}
							if (list.Any(predicate))
							{
								goto IL_395;
							}
						}
						this.currentGroupMembers.Clear();
						for (int l = 0; l < this.currentGroup.Count; l++)
						{
							int num3 = this.currentGroup[l];
							bool flag;
							if (this.IsMember(num3, out flag) && (!flag || !this.rootsSet.Contains(num3) || (FeatureWorker.groupSize[num3] >= minRootGroupSize && FeatureWorker.groupSize[num3] <= maxRootGroupSize)))
							{
								this.currentGroupMembers.Add(this.currentGroup[l]);
							}
						}
						if (this.currentGroupMembers.Count >= minOverallSize)
						{
							List<int> list2 = this.currentGroup;
							Predicate<int> predicate2;
							if ((predicate2 = <>9__5) == null)
							{
								predicate2 = (<>9__5 = ((int x) => worldGrid[x].feature == null));
							}
							if (list2.Any(predicate2))
							{
								List<int> list3 = this.currentGroup;
								Predicate<int> match;
								if ((match = <>9__6) == null)
								{
									match = (<>9__6 = ((int x) => worldGrid[x].feature != null));
								}
								list3.RemoveAll(match);
							}
							base.AddFeature(this.currentGroupMembers, this.currentGroup);
						}
					}
				}
				IL_395:;
			}
		}

		// Token: 0x0400415A RID: 16730
		private List<int> roots = new List<int>();

		// Token: 0x0400415B RID: 16731
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x0400415C RID: 16732
		private List<int> rootsWithAreaInBetween = new List<int>();

		// Token: 0x0400415D RID: 16733
		private HashSet<int> rootsWithAreaInBetweenSet = new HashSet<int>();

		// Token: 0x0400415E RID: 16734
		private List<int> currentGroup = new List<int>();

		// Token: 0x0400415F RID: 16735
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04004160 RID: 16736
		private HashSet<int> visitedValidGroupIDs = new HashSet<int>();

		// Token: 0x04004161 RID: 16737
		private static List<int> tmpGroup = new List<int>();
	}
}
