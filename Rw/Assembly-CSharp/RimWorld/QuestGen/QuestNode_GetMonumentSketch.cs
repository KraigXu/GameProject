using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001130 RID: 4400
	public class QuestNode_GetMonumentSketch : QuestNode
	{
		// Token: 0x060066DB RID: 26331 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x002400A4 File Offset: 0x0023E2A4
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060066DD RID: 26333 RVA: 0x002400B4 File Offset: 0x0023E2B4
		private bool DoWork(Slate slate)
		{
			float num = slate.Get<float>("points", 0f, false);
			float value = this.pointsPerArea.GetValue(slate);
			float num2 = Mathf.Min(num / value, 2500f);
			float randomInRange = QuestNode_GetMonumentSketch.RandomAspectRatioRange.RandomInRange;
			float f = Mathf.Sqrt(randomInRange * num2);
			float f2 = Mathf.Sqrt(num2 / randomInRange);
			int num3 = GenMath.RoundRandom(f);
			int num4 = GenMath.RoundRandom(f2);
			if (Rand.Bool)
			{
				int num5 = num3;
				num3 = num4;
				num4 = num5;
			}
			int? value2 = this.maxSize.GetValue(slate);
			if (value2 != null)
			{
				num3 = Mathf.Min(num3, value2.Value);
				num4 = Mathf.Min(num4, value2.Value);
			}
			num3 = Mathf.Max(num3, 3);
			num4 = Mathf.Max(num4, 3);
			IntVec2 value3 = new IntVec2(num3, num4);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.sketch = new Sketch();
			resolveParams.monumentSize = new IntVec2?(value3);
			resolveParams.useOnlyStonesAvailableOnMap = this.useOnlyResourcesAvailableOnMap.GetValue(slate);
			resolveParams.onlyBuildableByPlayer = new bool?(true);
			if (this.useOnlyResourcesAvailableOnMap.GetValue(slate) != null)
			{
				resolveParams.allowWood = new bool?(this.useOnlyResourcesAvailableOnMap.GetValue(slate).Biome.TreeDensity >= BiomeDefOf.BorealForest.TreeDensity);
			}
			resolveParams.allowedMonumentThings = new ThingFilter();
			resolveParams.allowedMonumentThings.SetAllowAll(null, true);
			resolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Urn, false);
			Sketch sketch = SketchGen.Generate(SketchResolverDefOf.Monument, resolveParams);
			if (this.clearStuff.GetValue(slate) ?? true)
			{
				List<SketchThing> things = sketch.Things;
				for (int i = 0; i < things.Count; i++)
				{
					things[i].stuff = null;
				}
				List<SketchTerrain> terrain = sketch.Terrain;
				for (int j = 0; j < terrain.Count; j++)
				{
					terrain[j].treatSimilarAsSame = true;
				}
			}
			slate.Set<Sketch>(this.storeAs.GetValue(slate), sketch, false);
			return true;
		}

		// Token: 0x04003EF2 RID: 16114
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EF3 RID: 16115
		public SlateRef<Map> useOnlyResourcesAvailableOnMap;

		// Token: 0x04003EF4 RID: 16116
		public SlateRef<int?> maxSize;

		// Token: 0x04003EF5 RID: 16117
		public SlateRef<float> pointsPerArea;

		// Token: 0x04003EF6 RID: 16118
		public SlateRef<bool?> clearStuff;

		// Token: 0x04003EF7 RID: 16119
		private static readonly FloatRange RandomAspectRatioRange = new FloatRange(1f, 3f);

		// Token: 0x04003EF8 RID: 16120
		private const int MinEdgeLength = 3;

		// Token: 0x04003EF9 RID: 16121
		private const int MaxArea = 2500;
	}
}
