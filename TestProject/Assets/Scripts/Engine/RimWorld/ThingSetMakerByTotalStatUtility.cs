using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD9 RID: 3289
	public static class ThingSetMakerByTotalStatUtility
	{
		// Token: 0x06004FB2 RID: 20402 RVA: 0x001ADC30 File Offset: 0x001ABE30
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.40282347E+38f)
		{
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp(countRange, totalValue, allowed, techLevel, qualityGenerator, getMinValue, getMaxValue, weightSelector, tries, maxMass, true);
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x001ADC54 File Offset: 0x001ABE54
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue_NewTmp(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.40282347E+38f, bool allowNonStackableDuplicates = true)
		{
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp2(countRange, totalValue, allowed, techLevel, qualityGenerator, getMinValue, getMaxValue, weightSelector, tries, maxMass, allowNonStackableDuplicates, 0f);
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x001ADC80 File Offset: 0x001ABE80
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue_NewTmp2(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.40282347E+38f, bool allowNonStackableDuplicates = true, float minSingleItemValue = 0f)
		{
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp3(countRange, totalValue, allowed, techLevel, qualityGenerator, getMinValue, getMaxValue, getMinValue, weightSelector, tries, maxMass, allowNonStackableDuplicates, minSingleItemValue);
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x001ADCA8 File Offset: 0x001ABEA8
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue_NewTmp3(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingStuffPairWithQuality, float> getSingleThingValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.40282347E+38f, bool allowNonStackableDuplicates = true, float minSingleItemValue = 0f)
		{
			List<ThingStuffPairWithQuality> chosen = new List<ThingStuffPairWithQuality>();
			if (countRange.max <= 0)
			{
				return chosen;
			}
			if (countRange.min < 1)
			{
				countRange.min = 1;
			}
			ThingSetMakerByTotalStatUtility.CalculateAllowedThingStuffPairs(allowed, techLevel, qualityGenerator);
			float trashThreshold = Mathf.Max(ThingSetMakerByTotalStatUtility.GetTrashThreshold(countRange, totalValue, getMaxValue), minSingleItemValue);
			ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.RemoveAll((ThingStuffPairWithQuality x) => getMaxValue(x) < trashThreshold);
			if (!ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Any<ThingStuffPairWithQuality>())
			{
				return chosen;
			}
			float minCandidateValueEver = float.MaxValue;
			float maxCandidateValueEver = float.MinValue;
			float minMassEver = float.MaxValue;
			foreach (ThingStuffPairWithQuality thingStuffPairWithQuality in ThingSetMakerByTotalStatUtility.allowedThingStuffPairs)
			{
				minCandidateValueEver = Mathf.Min(minCandidateValueEver, getMinValue(thingStuffPairWithQuality));
				maxCandidateValueEver = Mathf.Max(maxCandidateValueEver, getMaxValue(thingStuffPairWithQuality));
				if (thingStuffPairWithQuality.thing.category != ThingCategory.Pawn)
				{
					minMassEver = Mathf.Min(minMassEver, ThingSetMakerByTotalStatUtility.GetNonTrashMass(thingStuffPairWithQuality, trashThreshold, getMinValue));
				}
			}
			minCandidateValueEver = Mathf.Max(minCandidateValueEver, trashThreshold);
			float totalMinValueSoFar = 0f;
			float totalMaxValueSoFar = 0f;
			float minMassSoFar = 0f;
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 10000)
				{
					break;
				}
				IEnumerable<ThingStuffPairWithQuality> source = ThingSetMakerByTotalStatUtility.allowedThingStuffPairs;
				Func<ThingStuffPairWithQuality, bool> predicate=null;
				if (predicate== null)
				{
					predicate =  delegate(ThingStuffPairWithQuality x)
					{
						if (!allowNonStackableDuplicates && x.thing.stackLimit == 1 && chosen.Any((ThingStuffPairWithQuality c) => c.thing == x.thing))
						{
							return false;
						}
						if (maxMass != 3.40282347E+38f && x.thing.category != ThingCategory.Pawn)
						{
							float nonTrashMass = ThingSetMakerByTotalStatUtility.GetNonTrashMass(x, trashThreshold, getMinValue);
							if (minMassSoFar + nonTrashMass > maxMass)
							{
								return false;
							}
							if (chosen.Count < countRange.min && minMassSoFar + minMassEver * (float)(countRange.min - chosen.Count - 1) + nonTrashMass > maxMass)
							{
								return false;
							}
						}
						return totalMinValueSoFar + Mathf.Max(getMinValue(x), trashThreshold) <= totalValue && (chosen.Count >= countRange.min || totalMinValueSoFar + minCandidateValueEver * (float)(countRange.min - chosen.Count - 1) + Mathf.Max(getMinValue(x), trashThreshold) <= totalValue);
					};
				}
				IEnumerable<ThingStuffPairWithQuality> enumerable = source.Where(predicate);
				if (countRange.max != 2147483647 && totalMaxValueSoFar < totalValue * 0.5f)
				{
					IEnumerable<ThingStuffPairWithQuality> enumerable2 = enumerable;
					IEnumerable<ThingStuffPairWithQuality> source2 = enumerable;
					Func<ThingStuffPairWithQuality, bool> predicate2=null;
					if (predicate2 == null)
					{
						predicate2 = x => totalMaxValueSoFar + maxCandidateValueEver * (float)(countRange.max - chosen.Count - 1) + getMaxValue(x) >= totalValue * 0.5f;
					}
					enumerable = source2.Where(predicate2);
					if (!enumerable.Any<ThingStuffPairWithQuality>())
					{
						enumerable = enumerable2;
					}
				}
				float maxCandidateMinValue = float.MinValue;
				foreach (ThingStuffPairWithQuality arg in enumerable)
				{
					maxCandidateMinValue = Mathf.Max(maxCandidateMinValue, Mathf.Max(getMinValue(arg), trashThreshold));
				}
				ThingStuffPairWithQuality thingStuffPairWithQuality2;
				if (!enumerable.TryRandomElementByWeight(delegate(ThingStuffPairWithQuality x)
				{
					float a = 1f;
					if (countRange.max != 2147483647 && chosen.Count < countRange.max && totalValue >= totalMaxValueSoFar)
					{
						int num2 = countRange.max - chosen.Count;
						float b = (totalValue - totalMaxValueSoFar) / (float)num2;
						a = Mathf.InverseLerp(0f, b, getMaxValue(x));
					}
					float b2 = 1f;
					if (chosen.Count < countRange.min && totalValue >= totalMinValueSoFar)
					{
						int num3 = countRange.min - chosen.Count;
						float num4 = (totalValue - totalMinValueSoFar) / (float)num3;
						float num5 = Mathf.Max(getMinValue(x), trashThreshold);
						if (num5 > num4)
						{
							b2 = num4 / num5;
						}
					}
					float num6 = Mathf.Max(Mathf.Min(a, b2), 1E-05f);
					if (weightSelector != null)
					{
						num6 *= weightSelector(x.thing);
					}
					if (totalValue > totalMaxValueSoFar)
					{
						int num7 = Mathf.Max(countRange.min - chosen.Count, 1);
						float num8 = Mathf.InverseLerp(0f, maxCandidateMinValue * 0.85f, ThingSetMakerByTotalStatUtility.GetMaxValueWithMaxMass(x, minMassSoFar, maxMass, getMinValue, getMaxValue) * (float)num7);
						num6 *= num8 * num8;
					}
					if (PawnWeaponGenerator.IsDerpWeapon(x.thing, x.stuff))
					{
						num6 *= 0.1f;
					}
					if (techLevel != TechLevel.Undefined)
					{
						TechLevel techLevel2 = x.thing.techLevel;
						if (techLevel2 < techLevel && techLevel2 <= TechLevel.Neolithic && (x.thing.IsApparel || x.thing.IsWeapon))
						{
							num6 *= 0.1f;
						}
					}
					return num6;
				}, out thingStuffPairWithQuality2))
				{
					goto IL_476;
				}
				chosen.Add(thingStuffPairWithQuality2);
				totalMinValueSoFar += Mathf.Max(getMinValue(thingStuffPairWithQuality2), trashThreshold);
				totalMaxValueSoFar += getMaxValue(thingStuffPairWithQuality2);
				if (thingStuffPairWithQuality2.thing.category != ThingCategory.Pawn)
				{
					minMassSoFar += ThingSetMakerByTotalStatUtility.GetNonTrashMass(thingStuffPairWithQuality2, trashThreshold, getMinValue);
				}
				if (chosen.Count >= countRange.max || (chosen.Count >= countRange.min && totalMaxValueSoFar >= totalValue * 0.9f))
				{
					goto IL_476;
				}
			}
			Log.Error("Too many iterations.", false);
			IL_476:
			return chosen;
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x001AE150 File Offset: 0x001AC350
		[Obsolete]
		public static void IncreaseStackCountsToTotalValue(List<Thing> things, float totalValue, Func<Thing, float> getValue, float maxMass = 3.40282347E+38f)
		{
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue_NewTemp(things, totalValue, getValue, maxMass, false);
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x001AE15C File Offset: 0x001AC35C
		public static void IncreaseStackCountsToTotalValue_NewTemp(List<Thing> things, float totalValue, Func<Thing, float> getValue, float maxMass = 3.40282347E+38f, bool satisfyMinRewardCount = false)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < things.Count; i++)
			{
				num += getValue(things[i]) * (float)things[i].stackCount;
				if (!(things[i] is Pawn))
				{
					num2 += things[i].GetStatValue(StatDefOf.Mass, true) * (float)things[i].stackCount;
				}
			}
			if (num >= totalValue || num2 >= maxMass)
			{
				return;
			}
			things.SortByDescending((Thing x) => getValue(x) / x.GetStatValue(StatDefOf.Mass, true));
			ThingSetMakerByTotalStatUtility.DistributeEvenly(things, num + (totalValue - num) * 0.1f, ref num, ref num2, getValue, (maxMass == float.MaxValue) ? float.MaxValue : (num2 + (maxMass - num2) * 0.1f), false);
			if (num >= totalValue || num2 >= maxMass)
			{
				return;
			}
			if (satisfyMinRewardCount)
			{
				ThingSetMakerByTotalStatUtility.SatisfyMinRewardCount(things, totalValue, ref num, ref num2, getValue, maxMass);
				if (num >= totalValue || num2 >= maxMass)
				{
					return;
				}
			}
			ThingSetMakerByTotalStatUtility.DistributeEvenly(things, totalValue, ref num, ref num2, getValue, maxMass, true);
			if (num >= totalValue || num2 >= maxMass)
			{
				return;
			}
			ThingSetMakerByTotalStatUtility.GiveRemainingValueToAnything(things, totalValue, ref num, ref num2, getValue, maxMass);
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x001AE290 File Offset: 0x001AC490
		private static void DistributeEvenly(List<Thing> things, float totalValue, ref float currentTotalValue, ref float currentTotalMass, Func<Thing, float> getValue, float maxMass, bool useValueMassRatio = false)
		{
			float num = (totalValue - currentTotalValue) / (float)things.Count;
			float num2 = maxMass - currentTotalMass;
			float num3 = (maxMass == float.MaxValue) ? float.MaxValue : (num2 / (float)things.Count);
			float num4 = 0f;
			if (useValueMassRatio)
			{
				for (int i = 0; i < things.Count; i++)
				{
					num4 += getValue(things[i]) / things[i].GetStatValue(StatDefOf.Mass, true);
				}
			}
			for (int j = 0; j < things.Count; j++)
			{
				float num5 = getValue(things[j]);
				int num6 = Mathf.Min(Mathf.FloorToInt(num / num5), things[j].def.stackLimit - things[j].stackCount);
				if (maxMass != 3.40282347E+38f && !(things[j] is Pawn))
				{
					float b;
					if (useValueMassRatio)
					{
						b = num2 * (getValue(things[j]) / things[j].GetStatValue(StatDefOf.Mass, true) / num4);
					}
					else
					{
						b = num3;
					}
					num6 = Mathf.Min(num6, Mathf.FloorToInt(Mathf.Min(maxMass - currentTotalMass, b) / things[j].GetStatValue(StatDefOf.Mass, true)));
				}
				if (num6 > 0)
				{
					things[j].stackCount += num6;
					currentTotalValue += num5 * (float)num6;
					if (!(things[j] is Pawn))
					{
						currentTotalMass += things[j].GetStatValue(StatDefOf.Mass, true) * (float)num6;
					}
				}
			}
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x001AE434 File Offset: 0x001AC634
		private static void SatisfyMinRewardCount(List<Thing> things, float totalValue, ref float currentTotalValue, ref float currentTotalMass, Func<Thing, float> getValue, float maxMass)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].stackCount < things[i].def.minRewardCount)
				{
					float num = getValue(things[i]);
					int num2 = Mathf.FloorToInt((totalValue - currentTotalValue) / num);
					int num3 = Mathf.Min(new int[]
					{
						num2,
						things[i].def.stackLimit - things[i].stackCount,
						things[i].def.minRewardCount - things[i].stackCount
					});
					if (maxMass != 3.40282347E+38f && !(things[i] is Pawn))
					{
						num3 = Mathf.Min(num3, Mathf.FloorToInt((maxMass - currentTotalMass) / things[i].GetStatValue(StatDefOf.Mass, true)));
					}
					if (num3 > 0)
					{
						things[i].stackCount += num3;
						currentTotalValue += num * (float)num3;
						if (!(things[i] is Pawn))
						{
							currentTotalMass += things[i].GetStatValue(StatDefOf.Mass, true) * (float)num3;
						}
					}
				}
			}
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x001AE56C File Offset: 0x001AC76C
		private static void GiveRemainingValueToAnything(List<Thing> things, float totalValue, ref float currentTotalValue, ref float currentTotalMass, Func<Thing, float> getValue, float maxMass)
		{
			for (int i = 0; i < things.Count; i++)
			{
				float num = getValue(things[i]);
				int num2 = Mathf.Min(Mathf.FloorToInt((totalValue - currentTotalValue) / num), things[i].def.stackLimit - things[i].stackCount);
				if (maxMass != 3.40282347E+38f && !(things[i] is Pawn))
				{
					num2 = Mathf.Min(num2, Mathf.FloorToInt((maxMass - currentTotalMass) / things[i].GetStatValue(StatDefOf.Mass, true)));
				}
				if (num2 > 0)
				{
					things[i].stackCount += num2;
					currentTotalValue += num * (float)num2;
					if (!(things[i] is Pawn))
					{
						currentTotalMass += things[i].GetStatValue(StatDefOf.Mass, true) * (float)num2;
					}
				}
			}
		}

		private static void CalculateAllowedThingStuffPairs(IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator)
		{
			ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Clear();
			using (IEnumerator<ThingDef> enumerator = allowed.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef td = enumerator.Current;
					for (int i = 0; i < 5; i++)
					{
						ThingDef td2 = td;
						Predicate<ThingDef> validator=null;
						if (validator==null)
						{
							validator = x => !ThingSetMakerUtility.IsDerpAndDisallowed(td, x, new QualityGenerator?(qualityGenerator));
						}
						ThingDef stuff;
						if (GenStuff.TryRandomStuffFor(td2, out stuff, techLevel, validator))
						{
							QualityCategory quality = td.HasComp(typeof(CompQuality)) ? QualityUtility.GenerateQuality(qualityGenerator) : QualityCategory.Normal;
							ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Add(new ThingStuffPairWithQuality(td, stuff, quality));
						}
					}
				}
			}
		}


		private static float GetTrashThreshold(IntRange countRange, float totalValue, Func<ThingStuffPairWithQuality, float> getMaxValue)
		{
			float num = GenMath.Median<ThingStuffPairWithQuality>(ThingSetMakerByTotalStatUtility.allowedThingStuffPairs, getMaxValue, 0f, 0.5f);
			int num2 = Mathf.Clamp(Mathf.CeilToInt(totalValue / num), countRange.min, countRange.max);
			return totalValue / (float)num2 * 0.2f;
		}
		private static float GetNonTrashMass(ThingStuffPairWithQuality t, float trashThreshold, Func<ThingStuffPairWithQuality, float> getSingleThingValue)
		{
			int num = Mathf.Clamp(Mathf.CeilToInt(trashThreshold / getSingleThingValue(t)), 1, t.thing.stackLimit);
			return t.GetStatValue(StatDefOf.Mass) * (float)num;
		}


		private static float GetMaxValueWithMaxMass(ThingStuffPairWithQuality t, float massSoFar, float maxMass, Func<ThingStuffPairWithQuality, float> getSingleThingValue, Func<ThingStuffPairWithQuality, float> getMaxValue)
		{
			if (maxMass == 3.40282347E+38f)
			{
				return getMaxValue(t);
			}
			int num = Mathf.Clamp(Mathf.FloorToInt((maxMass - massSoFar) / t.GetStatValue(StatDefOf.Mass)), 1, t.thing.stackLimit);
			return getSingleThingValue(t) * (float)num;
		}

		private static List<ThingStuffPairWithQuality> allowedThingStuffPairs = new List<ThingStuffPairWithQuality>();
	}
}
