using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000DA RID: 218
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x0001CE41 File Offset: 0x0001B041
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (t.IsStuff)
			{
				return t.VolumePerUnit;
			}
			return 1f;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001CE58 File Offset: 0x0001B058
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			if (!ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume) || ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))
			{
				return "BillRequires".Translate(ing.GetBaseCount(), ing.filter.Summary);
			}
			return "BillRequires".Translate(ing.GetBaseCount() * 10f, ing.filter.Summary);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001CF1C File Offset: 0x0001B11C
		public override string ExtraDescriptionLine(RecipeDef r)
		{
			Func<ThingDef, bool> <>9__1;
			if (r.ingredients.Any(delegate(IngredientCount ing)
			{
				IEnumerable<ThingDef> allowedThingDefs = ing.filter.AllowedThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)));
				}
				return allowedThingDefs.Any(predicate);
			}))
			{
				return "BillRequiresMayVary".Translate(10.ToStringCached());
			}
			return null;
		}
	}
}
