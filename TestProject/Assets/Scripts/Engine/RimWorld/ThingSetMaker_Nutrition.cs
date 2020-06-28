using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CCF RID: 3279
	public class ThingSetMaker_Nutrition : ThingSetMaker
	{
		// Token: 0x06004F75 RID: 20341 RVA: 0x001AC2EE File Offset: 0x001AA4EE
		public ThingSetMaker_Nutrition()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x001AC304 File Offset: 0x001AA504
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			if (!this.AllowedThingDefs(parms).Any<ThingDef>())
			{
				return false;
			}
			if (parms.countRange != null && parms.countRange.Value.max <= 0)
			{
				return false;
			}
			if (parms.totalNutritionRange == null || parms.totalNutritionRange.Value.max <= 0f)
			{
				return false;
			}
			float maxValue;
			if (parms.maxTotalMass != null)
			{
				float? maxTotalMass = parms.maxTotalMass;
				maxValue = float.MaxValue;
				if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null) && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(this.AllowedThingDefs(parms), parms.techLevel ?? TechLevel.Undefined, parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.min : 1))
				{
					return false;
				}
			}
			return this.GeneratePossibleDefs(parms, out maxValue, this.nextSeed).Any<ThingStuffPairWithQuality>();
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x001AC410 File Offset: 0x001AA610
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing());
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue_NewTemp(outThings, totalValue, (Thing x) => x.GetStatValue(StatDefOf.Nutrition, true), maxMass, false);
			this.nextSeed++;
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x001ABF6B File Offset: 0x001AA16B
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x001AC4AE File Offset: 0x001AA6AE
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalNutrition);
			Rand.PopState();
			return result;
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x001AC4C4 File Offset: 0x001AA6C4
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalNutrition = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
			FloatRange floatRange = parms.totalNutritionRange ?? FloatRange.Zero;
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			QualityGenerator qualityGenerator = parms.qualityGenerator ?? QualityGenerator.BaseGen;
			totalNutrition = floatRange.RandomInRange;
			int numMeats = enumerable.Count((ThingDef x) => x.IsMeat);
			int numLeathers = enumerable.Count((ThingDef x) => x.IsLeather);
			Func<ThingDef, float> weightSelector = (ThingDef x) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(x, numMeats, numLeathers);
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp3(countRange, totalNutrition, enumerable, techLevel, qualityGenerator, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition) * (float)x.thing.stackLimit, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), weightSelector, 100, maxMass, true, 0f);
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x001AC674 File Offset: 0x001AA874
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			foreach (ThingDef thingDef in this.AllowedThingDefs(parms))
			{
				if (parms.maxTotalMass != null)
				{
					float? maxTotalMass = parms.maxTotalMass;
					float maxValue = float.MaxValue;
					if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null))
					{
						float minMass = ThingSetMakerUtility.GetMinMass(thingDef, techLevel);
						maxTotalMass = parms.maxTotalMass;
						if (minMass > maxTotalMass.GetValueOrDefault() & maxTotalMass != null)
						{
							continue;
						}
					}
				}
				if (parms.totalNutritionRange == null || parms.totalNutritionRange.Value.max == 3.40282347E+38f || !thingDef.IsNutritionGivingIngestible || thingDef.ingestible.CachedNutrition <= parms.totalNutritionRange.Value.max)
				{
					yield return thingDef;
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04002C8E RID: 11406
		private int nextSeed;
	}
}
