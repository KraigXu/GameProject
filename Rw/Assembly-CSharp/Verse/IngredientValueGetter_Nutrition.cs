using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DB RID: 219
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x0001CF79 File Offset: 0x0001B179
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (!t.IsNutritionGivingIngestible)
			{
				return 0f;
			}
			return t.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001CF95 File Offset: 0x0001B195
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(ing.GetBaseCount()) + " (" + ing.filter.Summary + ")";
		}
	}
}
