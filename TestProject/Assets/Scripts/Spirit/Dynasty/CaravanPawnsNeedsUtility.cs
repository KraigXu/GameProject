using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200123A RID: 4666
	public static class CaravanPawnsNeedsUtility
	{
		// Token: 0x06006CB1 RID: 27825 RVA: 0x0025EE61 File Offset: 0x0025D061
		public static bool CanEatForNutritionEver(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.WillEat(food, null, false) && food.ingestible.preferability > FoodPreferability.NeverForNutrition && (!food.IsDrug || !pawn.IsTeetotaler());
		}

		// Token: 0x06006CB2 RID: 27826 RVA: 0x0025EE99 File Offset: 0x0025D099
		public static bool CanEatForNutritionNow(ThingDef food, Pawn pawn)
		{
			return CaravanPawnsNeedsUtility.CanEatForNutritionEver(food, pawn) && (!pawn.RaceProps.Humanlike || pawn.needs.food.CurCategory >= HungerCategory.Starving || food.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes);
		}

		// Token: 0x06006CB3 RID: 27827 RVA: 0x0025EED7 File Offset: 0x0025D0D7
		public static bool CanEatForNutritionNow(Thing food, Pawn pawn)
		{
			return food.IngestibleNow && CaravanPawnsNeedsUtility.CanEatForNutritionNow(food.def, pawn);
		}

		// Token: 0x06006CB4 RID: 27828 RVA: 0x0025EEF4 File Offset: 0x0025D0F4
		public static float GetFoodScore(Thing food, Pawn pawn)
		{
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true));
			if (pawn.RaceProps.Humanlike)
			{
				CompRottable compRottable = food.TryGetComp<CompRottable>();
				int a = (compRottable != null) ? compRottable.TicksUntilRotAtCurrentTemp : int.MaxValue;
				float a2 = 1f - (float)Mathf.Min(a, 3600000) / 3600000f;
				num += Mathf.Min(a2, 0.999f);
			}
			return num;
		}

		// Token: 0x06006CB5 RID: 27829 RVA: 0x0025EF68 File Offset: 0x0025D168
		public static float GetFoodScore(ThingDef food, Pawn pawn, float singleFoodNutrition)
		{
			if (pawn.RaceProps.Humanlike)
			{
				return (float)food.ingestible.preferability;
			}
			float num = 0f;
			if (food == ThingDefOf.Kibble || food == ThingDefOf.Hay)
			{
				num = 5f;
			}
			else if (food.ingestible.preferability == FoodPreferability.DesperateOnlyForHumanlikes)
			{
				num = 4f;
			}
			else if (food.ingestible.preferability == FoodPreferability.RawBad)
			{
				num = 3f;
			}
			else if (food.ingestible.preferability == FoodPreferability.RawTasty)
			{
				num = 2f;
			}
			else if (food.ingestible.preferability < FoodPreferability.MealAwful)
			{
				num = 1f;
			}
			return num + Mathf.Min(singleFoodNutrition / 100f, 0.999f);
		}
	}
}
