using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB6 RID: 3510
	public class StockGenerator_Techprints : StockGenerator
	{
		// Token: 0x0600552E RID: 21806 RVA: 0x001C5422 File Offset: 0x001C3622
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			this.tmpGenerated.Clear();
			int countToGenerate = CountChanceUtility.RandomCount(this.countChances);
			int num;
			for (int i = 0; i < countToGenerate; i = num + 1)
			{
				ThingDef thingDef;
				if (!TechprintUtility.TryGetTechprintDefToGenerate(faction, out thingDef, this.tmpGenerated, 3.40282347E+38f))
				{
					yield break;
				}
				this.tmpGenerated.Add(thingDef);
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, 1))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				num = i;
			}
			this.tmpGenerated.Clear();
			yield break;
			yield break;
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x001C5439 File Offset: 0x001C3639
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains("Techprint");
		}

		// Token: 0x04002EAC RID: 11948
		private List<CountChance> countChances;

		// Token: 0x04002EAD RID: 11949
		private List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
