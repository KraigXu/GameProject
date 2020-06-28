using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAF RID: 3503
	public class StockGenerator_Category : StockGenerator
	{
		// Token: 0x0600551A RID: 21786 RVA: 0x001C51DD File Offset: 0x001C33DD
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			Func<ThingDef, bool> <>9__0;
			int num;
			for (int i = 0; i < numThingDefsToUse; i = num + 1)
			{
				IEnumerable<ThingDef> descendantThingDefs = this.categoryDef.DescendantThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef t) => t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate && !generatedDefs.Contains(t) && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)))));
				}
				ThingDef chosenThingDef;
				if (!descendantThingDefs.Where(predicate).TryRandomElement(out chosenThingDef))
				{
					break;
				}
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				generatedDefs.Add(chosenThingDef);
				chosenThingDef = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x001C51F0 File Offset: 0x001C33F0
		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != Tradeability.None && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}

		// Token: 0x04002E9F RID: 11935
		private ThingCategoryDef categoryDef;

		// Token: 0x04002EA0 RID: 11936
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x04002EA1 RID: 11937
		private List<ThingDef> excludedThingDefs;

		// Token: 0x04002EA2 RID: 11938
		private List<ThingCategoryDef> excludedCategories;
	}
}
