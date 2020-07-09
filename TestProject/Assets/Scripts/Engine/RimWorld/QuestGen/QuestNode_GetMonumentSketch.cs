using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetMonumentSketch : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		
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
			Sketch sketch = SketchGenCore.Generate(SketchResolverDefOf.Monument, resolveParams);
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<Map> useOnlyResourcesAvailableOnMap;

		
		public SlateRef<int?> maxSize;

		
		public SlateRef<float> pointsPerArea;

		
		public SlateRef<bool?> clearStuff;

		
		private static readonly FloatRange RandomAspectRatioRange = new FloatRange(1f, 3f);

		
		private const int MinEdgeLength = 3;

		
		private const int MaxArea = 2500;
	}
}
