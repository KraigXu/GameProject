using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C2 RID: 2242
	public abstract class FeatureWorker
	{
		// Token: 0x06003601 RID: 13825
		public abstract void GenerateWhereAppropriate();

		// Token: 0x06003602 RID: 13826 RVA: 0x0012546C File Offset: 0x0012366C
		protected void AddFeature(List<int> members, List<int> tilesForTextDrawPosCalculation)
		{
			WorldFeature worldFeature = new WorldFeature();
			worldFeature.uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID();
			worldFeature.def = this.def;
			worldFeature.name = NameGenerator.GenerateName(this.def.nameMaker, from x in Find.WorldFeatures.features
			select x.name, false, "r_name");
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < members.Count; i++)
			{
				worldGrid[members[i]].feature = worldFeature;
			}
			this.AssignBestDrawPos(worldFeature, tilesForTextDrawPosCalculation);
			Find.WorldFeatures.features.Add(worldFeature);
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x00125528 File Offset: 0x00123728
		private void AssignBestDrawPos(WorldFeature newFeature, List<int> tilesForTextDrawPosCalculation)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			FeatureWorker.tmpEdgeTiles.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.AddRange(tilesForTextDrawPosCalculation);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < tilesForTextDrawPosCalculation.Count; i++)
			{
				int num = tilesForTextDrawPosCalculation[i];
				vector += worldGrid.GetTileCenter(num);
				bool flag = worldGrid.IsOnEdge(num);
				if (!flag)
				{
					worldGrid.GetTileNeighbors(num, FeatureWorker.tmpNeighbors);
					for (int j = 0; j < FeatureWorker.tmpNeighbors.Count; j++)
					{
						if (!FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(FeatureWorker.tmpNeighbors[j]))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					FeatureWorker.tmpEdgeTiles.Add(num);
				}
			}
			vector /= (float)tilesForTextDrawPosCalculation.Count;
			if (!FeatureWorker.tmpEdgeTiles.Any<int>())
			{
				FeatureWorker.tmpEdgeTiles.Add(tilesForTextDrawPosCalculation.RandomElement<int>());
			}
			int bestTileDist = 0;
			FeatureWorker.tmpTraversedTiles.Clear();
			Find.WorldFloodFiller.FloodFill(-1, (int x) => FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(x), delegate(int tile, int traversalDist)
			{
				FeatureWorker.tmpTraversedTiles.Add(new Pair<int, int>(tile, traversalDist));
				bestTileDist = traversalDist;
				return false;
			}, int.MaxValue, FeatureWorker.tmpEdgeTiles);
			int num2 = -1;
			float num3 = -1f;
			for (int k = 0; k < FeatureWorker.tmpTraversedTiles.Count; k++)
			{
				if (FeatureWorker.tmpTraversedTiles[k].Second == bestTileDist)
				{
					float sqrMagnitude = (worldGrid.GetTileCenter(FeatureWorker.tmpTraversedTiles[k].First) - vector).sqrMagnitude;
					if (num2 == -1 || sqrMagnitude < num3)
					{
						num2 = FeatureWorker.tmpTraversedTiles[k].First;
						num3 = sqrMagnitude;
					}
				}
			}
			float maxDrawSizeInTiles = (float)bestTileDist * 2f * 1.2f;
			newFeature.drawCenter = worldGrid.GetTileCenter(num2);
			newFeature.maxDrawSizeInTiles = maxDrawSizeInTiles;
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x00125732 File Offset: 0x00123932
		protected static void ClearVisited()
		{
			FeatureWorker.ClearOrCreate<bool>(ref FeatureWorker.visited);
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x0012573E File Offset: 0x0012393E
		protected static void ClearGroupSizes()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupSize);
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x0012574A File Offset: 0x0012394A
		protected static void ClearGroupIDs()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupID);
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x00125758 File Offset: 0x00123958
		private static void ClearOrCreate<T>(ref T[] array)
		{
			int tilesCount = Find.WorldGrid.TilesCount;
			if (array == null || array.Length != tilesCount)
			{
				array = new T[tilesCount];
				return;
			}
			Array.Clear(array, 0, array.Length);
		}

		// Token: 0x04001E32 RID: 7730
		public FeatureDef def;

		// Token: 0x04001E33 RID: 7731
		protected static bool[] visited;

		// Token: 0x04001E34 RID: 7732
		protected static int[] groupSize;

		// Token: 0x04001E35 RID: 7733
		protected static int[] groupID;

		// Token: 0x04001E36 RID: 7734
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04001E37 RID: 7735
		private static HashSet<int> tmpTilesForTextDrawPosCalculationSet = new HashSet<int>();

		// Token: 0x04001E38 RID: 7736
		private static List<int> tmpEdgeTiles = new List<int>();

		// Token: 0x04001E39 RID: 7737
		private static List<Pair<int, int>> tmpTraversedTiles = new List<Pair<int, int>>();
	}
}
