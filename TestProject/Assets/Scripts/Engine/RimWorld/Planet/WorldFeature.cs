using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011CA RID: 4554
	public class WorldFeature : IExposable, ILoadReferenceable
	{
		// Token: 0x06006951 RID: 26961 RVA: 0x0024CA35 File Offset: 0x0024AC35
		protected static void FeatureSizePoint10_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06006952 RID: 26962 RVA: 0x0024CA35 File Offset: 0x0024AC35
		protected static void FeatureSizePoint25_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06006953 RID: 26963 RVA: 0x0024CA35 File Offset: 0x0024AC35
		protected static void FeatureSizePoint50_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x0024CA35 File Offset: 0x0024AC35
		protected static void FeatureSizePoint100_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x0024CA35 File Offset: 0x0024AC35
		protected static void FeatureSizePoint200_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x0024CA3C File Offset: 0x0024AC3C
		private static void TweakChanged()
		{
			Find.WorldFeatures.textsCreated = false;
			WorldFeature.EffectiveDrawSizeCurve[0] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[0].x, WorldFeature.FeatureSizePoint10);
			WorldFeature.EffectiveDrawSizeCurve[1] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[1].x, WorldFeature.FeatureSizePoint25);
			WorldFeature.EffectiveDrawSizeCurve[2] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[2].x, WorldFeature.FeatureSizePoint50);
			WorldFeature.EffectiveDrawSizeCurve[3] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[3].x, WorldFeature.FeatureSizePoint100);
			WorldFeature.EffectiveDrawSizeCurve[4] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[4].x, WorldFeature.FeatureSizePoint200);
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06006957 RID: 26967 RVA: 0x0024CB1C File Offset: 0x0024AD1C
		public float EffectiveDrawSize
		{
			get
			{
				return WorldFeature.EffectiveDrawSizeCurve.Evaluate(this.maxDrawSizeInTiles);
			}
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x0024CB30 File Offset: 0x0024AD30
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", 0, false);
			Scribe_Defs.Look<FeatureDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<Vector3>(ref this.drawCenter, "drawCenter", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.drawAngle, "drawAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.maxDrawSizeInTiles, "maxDrawSizeInTiles", 0f, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06006959 RID: 26969 RVA: 0x0024CBBD File Offset: 0x0024ADBD
		public string GetUniqueLoadID()
		{
			return "WorldFeature_" + this.uniqueID;
		}

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x0600695A RID: 26970 RVA: 0x0024CBD4 File Offset: 0x0024ADD4
		public IEnumerable<int> Tiles
		{
			get
			{
				WorldGrid worldGrid = Find.WorldGrid;
				int tilesCount = worldGrid.TilesCount;
				int num;
				for (int i = 0; i < tilesCount; i = num + 1)
				{
					if (worldGrid[i].feature == this)
					{
						yield return i;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x04004172 RID: 16754
		public int uniqueID;

		// Token: 0x04004173 RID: 16755
		public FeatureDef def;

		// Token: 0x04004174 RID: 16756
		public string name;

		// Token: 0x04004175 RID: 16757
		public Vector3 drawCenter;

		// Token: 0x04004176 RID: 16758
		public float drawAngle;

		// Token: 0x04004177 RID: 16759
		public float maxDrawSizeInTiles;

		// Token: 0x04004178 RID: 16760
		public float alpha;

		// Token: 0x04004179 RID: 16761
		protected static SimpleCurve EffectiveDrawSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 15f),
				true
			},
			{
				new CurvePoint(25f, 40f),
				true
			},
			{
				new CurvePoint(50f, 90f),
				true
			},
			{
				new CurvePoint(100f, 150f),
				true
			},
			{
				new CurvePoint(200f, 200f),
				true
			}
		};

		// Token: 0x0400417A RID: 16762
		[TweakValue("Interface.World", 0f, 40f)]
		protected static float FeatureSizePoint10 = 15f;

		// Token: 0x0400417B RID: 16763
		[TweakValue("Interface.World", 0f, 100f)]
		protected static float FeatureSizePoint25 = 40f;

		// Token: 0x0400417C RID: 16764
		[TweakValue("Interface.World", 0f, 200f)]
		protected static float FeatureSizePoint50 = 90f;

		// Token: 0x0400417D RID: 16765
		[TweakValue("Interface.World", 0f, 400f)]
		protected static float FeatureSizePoint100 = 150f;

		// Token: 0x0400417E RID: 16766
		[TweakValue("Interface.World", 0f, 800f)]
		protected static float FeatureSizePoint200 = 200f;
	}
}
