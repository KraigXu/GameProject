using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAE RID: 3246
	public static class PlantPosIndices
	{
		// Token: 0x06004EC2 RID: 20162 RVA: 0x001A8088 File Offset: 0x001A6288
		static PlantPosIndices()
		{
			for (int i = 0; i < 25; i++)
			{
				PlantPosIndices.rootList[i] = new int[8][];
				for (int j = 0; j < 8; j++)
				{
					int[] array = new int[i + 1];
					for (int k = 0; k < i; k++)
					{
						array[k] = k;
					}
					array.Shuffle<int>();
					PlantPosIndices.rootList[i][j] = array;
				}
			}
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x001A80F0 File Offset: 0x001A62F0
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}

		// Token: 0x04002C2F RID: 11311
		private static int[][][] rootList = new int[25][][];

		// Token: 0x04002C30 RID: 11312
		private const int ListCount = 8;
	}
}
