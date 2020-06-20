using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011C5 RID: 4549
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06006932 RID: 26930 RVA: 0x0024B838 File Offset: 0x00249A38
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06006933 RID: 26931 RVA: 0x0024B845 File Offset: 0x00249A45
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x06006934 RID: 26932 RVA: 0x0024BF7E File Offset: 0x0024A17E
		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x06006935 RID: 26933 RVA: 0x0024BF8B File Offset: 0x0024A18B
		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		// Token: 0x06006936 RID: 26934
		protected abstract bool IsRoot(int tile);

		// Token: 0x06006937 RID: 26935 RVA: 0x00010306 File Offset: 0x0000E506
		protected virtual bool IsPossiblyAllowed(int tile)
		{
			return false;
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x0024BF98 File Offset: 0x0024A198
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x0024BFAD File Offset: 0x0024A1AD
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootsAndPossiblyAllowedTiles();
			this.CalculateContiguousGroups();
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x0024BFBC File Offset: 0x0024A1BC
		private void CalculateRootsAndPossiblyAllowedTiles()
		{
			this.roots.Clear();
			this.possiblyAllowed.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.roots.Add(i);
				}
				if (this.IsPossiblyAllowed(i))
				{
					this.possiblyAllowed.Add(i);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
			this.possiblyAllowedSet.Clear();
			this.possiblyAllowedSet.AddRange(this.possiblyAllowed);
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x0024C058 File Offset: 0x0024A258
		private void CalculateContiguousGroups()
		{
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			int maxPossiblyAllowedSizeToTake = this.MaxPossiblyAllowedSizeToTake;
			float maxPossiblyAllowedSizePctOfMeToTake = this.MaxPossiblyAllowedSizePctOfMeToTake;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			Predicate<int> <>9__0;
			for (int i = 0; i < this.possiblyAllowed.Count; i++)
			{
				int num = this.possiblyAllowed[i];
				if (!FeatureWorker.visited[num] && !this.rootsSet.Contains(num))
				{
					FeatureWorker_FloodFill.tmpGroup.Clear();
					WorldFloodFiller worldFloodFiller2 = worldFloodFiller;
					int rootTile = num;
					Predicate<int> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((int x) => this.possiblyAllowedSet.Contains(x) && !this.rootsSet.Contains(x)));
					}
					worldFloodFiller2.FloodFill(rootTile, passCheck, delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						FeatureWorker_FloodFill.tmpGroup.Add(x);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_FloodFill.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_FloodFill.tmpGroup[j]] = FeatureWorker_FloodFill.tmpGroup.Count;
					}
				}
			}
			Predicate<int> <>9__2;
			Predicate<int> <>9__4;
			Predicate<int> <>9__8;
			Predicate<int> <>9__9;
			Predicate<int> <>9__10;
			for (int k = 0; k < this.roots.Count; k++)
			{
				int num2 = this.roots[k];
				if (!FeatureWorker.visited[num2])
				{
					int initialMembersCountClamped = 0;
					WorldFloodFiller worldFloodFiller3 = worldFloodFiller;
					int rootTile2 = num2;
					Predicate<int> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = ((int x) => (this.rootsSet.Contains(x) || this.possiblyAllowedSet.Contains(x)) && this.IsMember(x)));
					}
					worldFloodFiller3.FloodFill(rootTile2, passCheck2, delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						int initialMembersCountClamped = initialMembersCountClamped;
						initialMembersCountClamped++;
						return initialMembersCountClamped >= minSize;
					}, int.MaxValue, null);
					if (initialMembersCountClamped >= minSize)
					{
						int initialRootsCount = 0;
						WorldFloodFiller worldFloodFiller4 = worldFloodFiller;
						int rootTile3 = num2;
						Predicate<int> passCheck3;
						if ((passCheck3 = <>9__4) == null)
						{
							passCheck3 = (<>9__4 = ((int x) => this.rootsSet.Contains(x)));
						}
						worldFloodFiller4.FloodFill(rootTile3, passCheck3, delegate(int x)
						{
							FeatureWorker.visited[x] = true;
							int initialRootsCount = initialRootsCount;
							initialRootsCount++;
						}, int.MaxValue, null);
						if (initialRootsCount >= minSize && initialRootsCount <= maxSize)
						{
							int traversedRootsCount = 0;
							this.currentGroup.Clear();
							worldFloodFiller.FloodFill(num2, (int x) => this.rootsSet.Contains(x) || (this.possiblyAllowedSet.Contains(x) && FeatureWorker.groupSize[x] <= maxPossiblyAllowedSizeToTake && (float)FeatureWorker.groupSize[x] <= maxPossiblyAllowedSizePctOfMeToTake * (float)Mathf.Max(traversedRootsCount, initialRootsCount) && FeatureWorker.groupSize[x] < maxSize), delegate(int x)
							{
								FeatureWorker.visited[x] = true;
								if (this.rootsSet.Contains(x))
								{
									int traversedRootsCount = traversedRootsCount;
									traversedRootsCount++;
								}
								this.currentGroup.Add(x);
							}, int.MaxValue, null);
							if (this.currentGroup.Count >= minSize && this.currentGroup.Count <= maxSize)
							{
								if (!this.def.canTouchWorldEdge)
								{
									List<int> list = this.currentGroup;
									Predicate<int> predicate;
									if ((predicate = <>9__8) == null)
									{
										predicate = (<>9__8 = ((int x) => worldGrid.IsOnEdge(x)));
									}
									if (list.Any(predicate))
									{
										goto IL_41F;
									}
								}
								this.currentGroupMembers.Clear();
								for (int l = 0; l < this.currentGroup.Count; l++)
								{
									if (this.IsMember(this.currentGroup[l]))
									{
										this.currentGroupMembers.Add(this.currentGroup[l]);
									}
								}
								if (this.currentGroupMembers.Count >= minSize)
								{
									List<int> list2 = this.currentGroup;
									Predicate<int> predicate2;
									if ((predicate2 = <>9__9) == null)
									{
										predicate2 = (<>9__9 = ((int x) => worldGrid[x].feature == null));
									}
									if (list2.Any(predicate2))
									{
										List<int> list3 = this.currentGroup;
										Predicate<int> match;
										if ((match = <>9__10) == null)
										{
											match = (<>9__10 = ((int x) => worldGrid[x].feature != null));
										}
										list3.RemoveAll(match);
									}
									base.AddFeature(this.currentGroupMembers, this.currentGroup);
								}
							}
						}
					}
				}
				IL_41F:;
			}
		}

		// Token: 0x04004164 RID: 16740
		private List<int> roots = new List<int>();

		// Token: 0x04004165 RID: 16741
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x04004166 RID: 16742
		private List<int> possiblyAllowed = new List<int>();

		// Token: 0x04004167 RID: 16743
		private HashSet<int> possiblyAllowedSet = new HashSet<int>();

		// Token: 0x04004168 RID: 16744
		private List<int> currentGroup = new List<int>();

		// Token: 0x04004169 RID: 16745
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x0400416A RID: 16746
		private static List<int> tmpGroup = new List<int>();
	}
}
