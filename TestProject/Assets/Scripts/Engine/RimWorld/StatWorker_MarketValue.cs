﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001018 RID: 4120
	public class StatWorker_MarketValue : StatWorker
	{
		// Token: 0x060062B3 RID: 25267 RVA: 0x002242F4 File Offset: 0x002224F4
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.HasThing && req.Thing is Pawn)
			{
				return base.GetValueUnfinalized(StatRequest.For(req.BuildableDef, req.StuffDef, QualityCategory.Normal), applyPostProcess) * PriceUtility.PawnQualityPriceFactor((Pawn)req.Thing, null) + PriceUtility.PawnQualityPriceOffset((Pawn)req.Thing, null);
			}
			float result;
			if (req.StatBases.StatListContains(StatDefOf.MarketValue))
			{
				result = base.GetValueUnfinalized(req, true);
			}
			else
			{
				result = StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, req.StuffDef);
			}
			return result;
		}

		// Token: 0x060062B4 RID: 25268 RVA: 0x00224390 File Offset: 0x00222590
		public static float CalculatedBaseMarketValue(BuildableDef def, ThingDef stuffDef)
		{
			float num = 0f;
			RecipeDef recipeDef = StatWorker_MarketValue.CalculableRecipe(def);
			float num2;
			int num3;
			if (recipeDef != null)
			{
				num2 = recipeDef.workAmount;
				num3 = recipeDef.products[0].count;
				if (recipeDef.ingredients != null)
				{
					for (int i = 0; i < recipeDef.ingredients.Count; i++)
					{
						IngredientCount ingredientCount = recipeDef.ingredients[i];
						int num4 = ingredientCount.CountRequiredOfFor(ingredientCount.FixedIngredient, recipeDef);
						num += (float)num4 * ingredientCount.FixedIngredient.BaseMarketValue;
					}
				}
			}
			else
			{
				num2 = Mathf.Max(def.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef), def.GetStatValueAbstract(StatDefOf.WorkToBuild, stuffDef));
				num3 = 1;
				if (def.costList != null)
				{
					for (int j = 0; j < def.costList.Count; j++)
					{
						ThingDefCountClass thingDefCountClass = def.costList[j];
						num += (float)thingDefCountClass.count * thingDefCountClass.thingDef.BaseMarketValue;
					}
				}
				if (def.costStuffCount > 0)
				{
					if (stuffDef != null)
					{
						num += (float)def.costStuffCount / stuffDef.VolumePerUnit * stuffDef.GetStatValueAbstract(StatDefOf.MarketValue, null);
					}
					else
					{
						num += (float)def.costStuffCount * 2f;
					}
				}
			}
			if (num2 > 2f)
			{
				num += num2 * 0.0036f;
			}
			return num / (float)num3;
		}

		// Token: 0x060062B5 RID: 25269 RVA: 0x002244E4 File Offset: 0x002226E4
		public static RecipeDef CalculableRecipe(BuildableDef def)
		{
			if (def.costList.NullOrEmpty<ThingDefCountClass>() && def.costStuffCount <= 0)
			{
				List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					RecipeDef recipeDef = allDefsListForReading[i];
					if (recipeDef.products != null && recipeDef.products.Count == 1 && recipeDef.products[0].thingDef == def)
					{
						for (int j = 0; j < recipeDef.ingredients.Count; j++)
						{
							if (!recipeDef.ingredients[j].IsFixedIngredient)
							{
								return null;
							}
						}
						return recipeDef;
					}
				}
			}
			return null;
		}

		// Token: 0x060062B6 RID: 25270 RVA: 0x00224580 File Offset: 0x00222780
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			if (req.HasThing && req.Thing is Pawn)
			{
				Pawn pawn = (Pawn)req.Thing;
				StringBuilder stringBuilder = new StringBuilder(base.GetExplanationUnfinalized(req, numberSense));
				PriceUtility.PawnQualityPriceFactor(pawn, stringBuilder);
				PriceUtility.PawnQualityPriceOffset(pawn, stringBuilder);
				return stringBuilder.ToString();
			}
			if (req.StatBases.StatListContains(StatDefOf.MarketValue))
			{
				return base.GetExplanationUnfinalized(req, numberSense);
			}
			return "StatsReport_MarketValueFromStuffsAndWork".TranslateSimple().TrimEnd(new char[]
			{
				'.'
			}) + ": " + StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, req.StuffDef).ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense);
		}

		// Token: 0x04003C0B RID: 15371
		public const float ValuePerWork = 0.0036f;

		// Token: 0x04003C0C RID: 15372
		private const float DefaultGuessStuffCost = 2f;
	}
}
