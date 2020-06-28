using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001210 RID: 4624
	public class WorldGenStep_AncientRoads : WorldGenStep
	{
		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x06006AF4 RID: 27380 RVA: 0x002551D4 File Offset: 0x002533D4
		public override int SeedPart
		{
			get
			{
				return 773428712;
			}
		}

		// Token: 0x06006AF5 RID: 27381 RVA: 0x002551DB File Offset: 0x002533DB
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientRoads();
		}

		// Token: 0x06006AF6 RID: 27382 RVA: 0x002551E4 File Offset: 0x002533E4
		private void GenerateAncientRoads()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts(new int?(0));
			List<List<int>> list = this.GenerateProspectiveRoads();
			list.Sort((List<int> lhs, List<int> rhs) => -lhs.Count.CompareTo(rhs.Count));
			HashSet<int> used = new HashSet<int>();
			Predicate<int> <>9__1;
			for (int i = 0; i < list.Count; i++)
			{
				List<int> list2 = list[i];
				Predicate<int> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((int elem) => used.Contains(elem)));
				}
				if (!list2.Any(predicate))
				{
					if (list[i].Count < 4)
					{
						break;
					}
					foreach (int item in list[i])
					{
						used.Add(item);
					}
					for (int j = 0; j < list[i].Count - 1; j++)
					{
						float num = Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness;
						float costCutoff = num * 12000f;
						using (WorldPath worldPath = Find.WorldPathFinder.FindPath(list[i][j], list[i][j + 1], null, (float cost) => cost > costCutoff))
						{
							if (worldPath != null && worldPath != WorldPath.NotFound)
							{
								List<int> nodesReversed = worldPath.NodesReversed;
								if ((float)nodesReversed.Count <= Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness)
								{
									for (int k = 0; k < nodesReversed.Count - 1; k++)
									{
										if (Find.WorldGrid.GetRoadDef(nodesReversed[k], nodesReversed[k + 1], false) != null)
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltHighway);
										}
										else
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltRoad);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06006AF7 RID: 27383 RVA: 0x00255480 File Offset: 0x00253680
		private List<List<int>> GenerateProspectiveRoads()
		{
			List<int> ancientSites = Find.World.genData.ancientSites;
			List<List<int>> list = new List<List<int>>();
			for (int i = 0; i < ancientSites.Count; i++)
			{
				for (int j = 0; j < ancientSites.Count; j++)
				{
					List<int> list2 = new List<int>();
					list2.Add(ancientSites[i]);
					List<int> list3 = ancientSites;
					float ang = Find.World.grid.GetHeadingFromTo(i, j);
					int current = ancientSites[i];
					Func<int, bool> <>9__0;
					Func<int, float> <>9__1;
					for (;;)
					{
						IEnumerable<int> source = list3;
						Func<int, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((int idx) => idx != current && Math.Abs(Find.World.grid.GetHeadingFromTo(current, idx) - ang) < this.maximumSiteCurve));
						}
						list3 = source.Where(predicate).ToList<int>();
						if (list3.Count == 0)
						{
							break;
						}
						IEnumerable<int> source2 = list3;
						Func<int, float> selector;
						if ((selector = <>9__1) == null)
						{
							selector = (<>9__1 = ((int idx) => Find.World.grid.ApproxDistanceInTiles(current, idx)));
						}
						int num = source2.MinBy(selector);
						ang = Find.World.grid.GetHeadingFromTo(current, num);
						current = num;
						list2.Add(current);
					}
					list.Add(list2);
				}
			}
			return list;
		}

		// Token: 0x040042D8 RID: 17112
		public float maximumSiteCurve;

		// Token: 0x040042D9 RID: 17113
		public float minimumChain;

		// Token: 0x040042DA RID: 17114
		public float maximumSegmentCurviness;
	}
}
