using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB3 RID: 3507
	public class StockGenerator_Tag : StockGenerator
	{
		// Token: 0x0600552A RID: 21802 RVA: 0x001C535B File Offset: 0x001C355B
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			Func<ThingDef, bool> <>9__0;
			int num;
			for (int i = 0; i < numThingDefsToUse; i = num + 1)
			{
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef d) => this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(d)) && !generatedDefs.Contains(d)));
				}
				ThingDef chosenThingDef;
				if (!allDefs.Where(predicate).TryRandomElement(out chosenThingDef))
				{
					yield break;
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

		// Token: 0x0600552B RID: 21803 RVA: 0x001C536B File Offset: 0x001C356B
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}

		// Token: 0x04002EA7 RID: 11943
		[NoTranslate]
		private string tradeTag;

		// Token: 0x04002EA8 RID: 11944
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x04002EA9 RID: 11945
		private List<ThingDef> excludedThingDefs;
	}
}
