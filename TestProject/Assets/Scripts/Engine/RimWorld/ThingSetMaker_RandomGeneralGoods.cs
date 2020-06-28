using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD4 RID: 3284
	public class ThingSetMaker_RandomGeneralGoods : ThingSetMaker
	{
		// Token: 0x06004F92 RID: 20370 RVA: 0x001ACF18 File Offset: 0x001AB118
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IntRange intRange = parms.countRange ?? new IntRange(10, 20);
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			int num = Mathf.Max(intRange.RandomInRange, 1);
			for (int i = 0; i < num; i++)
			{
				outThings.Add(this.GenerateSingle(techLevel));
			}
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x001ACF90 File Offset: 0x001AB190
		private Thing GenerateSingle(TechLevel techLevel)
		{
			Thing thing = null;
			int num = 0;
			while (thing == null && num < 50)
			{
				IEnumerable<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>> goodsWeights = ThingSetMaker_RandomGeneralGoods.GoodsWeights;
				Func<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>, float> weightSelector = (Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float> x) => x.Second;
				switch (goodsWeights.RandomElementByWeight(weightSelector).First)
				{
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Meals:
					thing = this.RandomMeals(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood:
					thing = this.RandomRawFood(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine:
					thing = this.RandomMedicine(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs:
					thing = this.RandomDrugs(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Resources:
					thing = this.RandomResources(techLevel);
					break;
				default:
					throw new Exception();
				}
				num++;
			}
			return thing;
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x001AD040 File Offset: 0x001AB240
		private Thing RandomMeals(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.Pemmican;
			}
			else
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					thingDef = ThingDefOf.MealSimple;
				}
				else if ((double)value < 0.75)
				{
					thingDef = ThingDefOf.MealFine;
				}
				else
				{
					thingDef = ThingDefOf.MealSurvivalPack;
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 10);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001AD0B4 File Offset: 0x001AB2B4
		private Thing RandomRawFood(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (!this.PossibleRawFood(techLevel).TryRandomElement(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001AD0F8 File Offset: 0x001AB2F8
		private IEnumerable<ThingDef> PossibleRawFood(TechLevel techLevel)
		{
			return from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && x.techLevel <= techLevel && x.ingestible.preferability < FoodPreferability.MealAwful
			select x;
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x001AD128 File Offset: 0x001AB328
		private Thing RandomMedicine(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (Rand.Value < 0.75f && techLevel >= ThingDefOf.MedicineHerbal.techLevel)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsMedicine && x.techLevel <= techLevel
				select x).MaxBy((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			}
			else if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.MedicineHerbal;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 20);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x001AD210 File Offset: 0x001AB410
		private Thing RandomDrugs(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsDrug && x.techLevel <= techLevel
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 25);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x001AD270 File Offset: 0x001AB470
		private Thing RandomResources(TechLevel techLevel)
		{
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel, false);
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x001AD2A9 File Offset: 0x001AB4A9
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			if (techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.Pemmican;
			}
			else
			{
				yield return ThingDefOf.MealSimple;
				yield return ThingDefOf.MealFine;
				yield return ThingDefOf.MealSurvivalPack;
			}
			foreach (ThingDef thingDef in this.PossibleRawFood(techLevel))
			{
				yield return thingDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			foreach (ThingDef thingDef2 in from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x)
			{
				yield return thingDef2;
			}
			enumerator = null;
			IEnumerable<ThingDef> allGeneratableItems = ThingSetMakerUtility.allGeneratableItems;
			Func<ThingDef, bool> predicate=null;
			if (predicate == null)
			{
				predicate = x => x.IsDrug && x.techLevel <= techLevel;
			}
			foreach (ThingDef thingDef3 in allGeneratableItems.Where(predicate))
			{
				yield return thingDef3;
			}
			enumerator = null;
			if (techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.WoodLog;
			}
			else
			{
				foreach (ThingDef thingDef4 in from d in DefDatabase<ThingDef>.AllDefsListForReading
				where BaseGenUtility.IsCheapWallStuff(d)
				select d)
				{
					yield return thingDef4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04002C99 RID: 11417
		private static Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[] GoodsWeights = new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[]
		{
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Meals, 1f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood, 0.75f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Resources, 0.234f)
		};

		// Token: 0x02001C25 RID: 7205
		private enum GoodsType
		{
			// Token: 0x04006AC7 RID: 27335
			None,
			// Token: 0x04006AC8 RID: 27336
			Meals,
			// Token: 0x04006AC9 RID: 27337
			RawFood,
			// Token: 0x04006ACA RID: 27338
			Medicine,
			// Token: 0x04006ACB RID: 27339
			Drugs,
			// Token: 0x04006ACC RID: 27340
			Resources
		}
	}
}
