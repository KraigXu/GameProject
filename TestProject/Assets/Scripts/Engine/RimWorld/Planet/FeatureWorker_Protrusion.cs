using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class FeatureWorker_Protrusion : FeatureWorker
	{
		
		// (get) Token: 0x06006945 RID: 26949 RVA: 0x0024B838 File Offset: 0x00249A38
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		
		// (get) Token: 0x06006946 RID: 26950 RVA: 0x0024B845 File Offset: 0x00249A45
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		
		// (get) Token: 0x06006947 RID: 26951 RVA: 0x0024C5B7 File Offset: 0x0024A7B7
		protected virtual int MaxPassageWidth
		{
			get
			{
				return this.def.maxPassageWidth;
			}
		}

		
		// (get) Token: 0x06006948 RID: 26952 RVA: 0x0024C5C4 File Offset: 0x0024A7C4
		protected virtual float MaxPctOfWholeArea
		{
			get
			{
				return this.def.maxPctOfWholeArea;
			}
		}

		
		protected abstract bool IsRoot(int tile);

		
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRoots();
			this.CalculateRootsWithoutSmallPassages();
			this.CalculateContiguousGroups();
		}

		
		private void CalculateRoots()
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

		
		private void CalculateRootsWithoutSmallPassages()
		{
			this.rootsWithoutSmallPassages.Clear();
			this.rootsWithoutSmallPassages.AddRange(this.roots);
			GenPlanetMorphology.Open(this.rootsWithoutSmallPassages, this.MaxPassageWidth);
			this.rootsWithoutSmallPassagesSet.Clear();
			this.rootsWithoutSmallPassagesSet.AddRange(this.rootsWithoutSmallPassages);
		}

		
		private void CalculateContiguousGroups()
		{
			//WorldGrid worldGrid = Find.WorldGrid;
			//WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			//int minSize = this.MinSize;
			//int maxSize = this.MaxSize;
			//float maxPctOfWholeArea = this.MaxPctOfWholeArea;
			//int maxPassageWidth = this.MaxPassageWidth;
			//FeatureWorker.ClearVisited();
			//FeatureWorker.ClearGroupSizes();
			//Predicate<int> 9__0;
			//for (int i = 0; i < this.roots.Count; i++)
			//{
			//	int num = this.roots[i];
			//	if (!FeatureWorker.visited[num])
			//	{
			//		FeatureWorker_Protrusion.tmpGroup.Clear();
			//		WorldFloodFiller worldFloodFiller2 = worldFloodFiller;
			//		int rootTile = num;
			//		Predicate<int> passCheck;
			//		if ((passCheck ) == null)
			//		{
			//			passCheck = (9__0 = ((int x) => this.rootsSet.Contains(x)));
			//		}
			//		worldFloodFiller2.FloodFill(rootTile, passCheck, delegate(int x)
			//		{
			//			FeatureWorker.visited[x] = true;
			//			FeatureWorker_Protrusion.tmpGroup.Add(x);
			//		}, int.MaxValue, null);
			//		for (int j = 0; j < FeatureWorker_Protrusion.tmpGroup.Count; j++)
			//		{
			//			FeatureWorker.groupSize[FeatureWorker_Protrusion.tmpGroup[j]] = FeatureWorker_Protrusion.tmpGroup.Count;
			//		}
			//	}
			//}
			//FeatureWorker.ClearVisited();
			//Predicate<int> 9__2;
			//Action<int> 9__3;
			//Predicate<int> 9__4;
			//Predicate<int> 9__5;
			//Predicate<int> 9__6;
			//Predicate<int> 9__7;
			//for (int k = 0; k < this.rootsWithoutSmallPassages.Count; k++)
			//{
			//	int num2 = this.rootsWithoutSmallPassages[k];
			//	if (!FeatureWorker.visited[num2])
			//	{
			//		this.currentGroup.Clear();
			//		WorldFloodFiller worldFloodFiller3 = worldFloodFiller;
			//		int rootTile2 = num2;
			//		Predicate<int> passCheck2;
			//		if ((passCheck2 ) == null)
			//		{
			//			passCheck2 = (9__2 = ((int x) => this.rootsWithoutSmallPassagesSet.Contains(x)));
			//		}
			//		Action<int> processor;
			//		if ((processor ) == null)
			//		{
			//			processor = (9__3 = delegate(int x)
			//			{
			//				FeatureWorker.visited[x] = true;
			//				this.currentGroup.Add(x);
			//			});
			//		}
			//		worldFloodFiller3.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
			//		if (this.currentGroup.Count >= minSize)
			//		{
			//			List<int> tiles = this.currentGroup;
			//			int count = maxPassageWidth * 2;
			//			Predicate<int> extraPredicate;
			//			if ((extraPredicate ) == null)
			//			{
			//				extraPredicate = (9__4 = ((int x) => this.rootsSet.Contains(x)));
			//			}
			//			GenPlanetMorphology.Dilate(tiles, count, extraPredicate);
			//			if (this.currentGroup.Count <= maxSize && (float)this.currentGroup.Count / (float)FeatureWorker.groupSize[num2] <= maxPctOfWholeArea)
			//			{
			//				if (!this.def.canTouchWorldEdge)
			//				{
			//					List<int> list = this.currentGroup;
			//					Predicate<int> predicate;
			//					if ((predicate ) == null)
			//					{
			//						predicate = (9__5 = ((int x) => worldGrid.IsOnEdge(x)));
			//					}
			//					if (list.Any(predicate))
			//					{
			//						goto IL_30D;
			//					}
			//				}
			//				this.currentGroupMembers.Clear();
			//				for (int l = 0; l < this.currentGroup.Count; l++)
			//				{
			//					if (this.IsMember(this.currentGroup[l]))
			//					{
			//						this.currentGroupMembers.Add(this.currentGroup[l]);
			//					}
			//				}
			//				if (this.currentGroupMembers.Count >= minSize)
			//				{
			//					List<int> list2 = this.currentGroup;
			//					Predicate<int> predicate2;
			//					if ((predicate2 ) == null)
			//					{
			//						predicate2 = (9__6 = ((int x) => worldGrid[x].feature == null));
			//					}
			//					if (list2.Any(predicate2))
			//					{
			//						List<int> list3 = this.currentGroup;
			//						Predicate<int> match;
			//						if ((match ) == null)
			//						{
			//							match = (9__7 = ((int x) => worldGrid[x].feature != null));
			//						}
			//						list3.RemoveAll(match);
			//					}
			//					base.AddFeature(this.currentGroupMembers, this.currentGroup);
			//				}
			//			}
			//		}
			//	}
			//	IL_30D:;
			//}
		}

		
		private List<int> roots = new List<int>();

		
		private HashSet<int> rootsSet = new HashSet<int>();

		
		private List<int> rootsWithoutSmallPassages = new List<int>();

		
		private HashSet<int> rootsWithoutSmallPassagesSet = new HashSet<int>();

		
		private List<int> currentGroup = new List<int>();

		
		private List<int> currentGroupMembers = new List<int>();

		
		private static List<int> tmpGroup = new List<int>();
	}
}
