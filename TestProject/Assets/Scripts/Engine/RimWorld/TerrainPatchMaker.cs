using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000A5D RID: 2653
	public class TerrainPatchMaker
	{
		// Token: 0x06003EB4 RID: 16052 RVA: 0x0014D28C File Offset: 0x0014B48C
		private void Init(Map map)
		{
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.RenderSize = new IntVec2(map.Size.x, map.Size.z);
			NoiseDebugUI.StoreNoiseRender(this.noise, "TerrainPatchMaker " + this.thresholds[0].terrain.defName);
			this.currentlyInitializedForMap = map;
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0014D31D File Offset: 0x0014B51D
		public void Cleanup()
		{
			this.noise = null;
			this.currentlyInitializedForMap = null;
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0014D330 File Offset: 0x0014B530
		public TerrainDef TerrainAt(IntVec3 c, Map map, float fertility)
		{
			if (fertility < this.minFertility || fertility > this.maxFertility)
			{
				return null;
			}
			if (this.noise != null && map != this.currentlyInitializedForMap)
			{
				this.Cleanup();
			}
			if (this.noise == null)
			{
				this.Init(map);
			}
			if (this.minSize > 0)
			{
				int count = 0;
				map.floodFiller.FloodFill(c, (IntVec3 x) => TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(x)) != null, delegate(IntVec3 x)
				{
					int count = count;
					count++;
					return count >= this.minSize;
				}, int.MaxValue, false, null);
				if (count < this.minSize)
				{
					return null;
				}
			}
			return TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(c));
		}

		// Token: 0x0400247A RID: 9338
		private Map currentlyInitializedForMap;

		// Token: 0x0400247B RID: 9339
		public List<TerrainThreshold> thresholds = new List<TerrainThreshold>();

		// Token: 0x0400247C RID: 9340
		public float perlinFrequency = 0.01f;

		// Token: 0x0400247D RID: 9341
		public float perlinLacunarity = 2f;

		// Token: 0x0400247E RID: 9342
		public float perlinPersistence = 0.5f;

		// Token: 0x0400247F RID: 9343
		public int perlinOctaves = 6;

		// Token: 0x04002480 RID: 9344
		public float minFertility = -999f;

		// Token: 0x04002481 RID: 9345
		public float maxFertility = 999f;

		// Token: 0x04002482 RID: 9346
		public int minSize;

		// Token: 0x04002483 RID: 9347
		[Unsaved(false)]
		private ModuleBase noise;
	}
}
