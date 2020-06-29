using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StockGenerator_Techprints : StockGenerator
	{
		
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

		
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains("Techprint");
		}

		
		private List<CountChance> countChances;

		
		private List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
