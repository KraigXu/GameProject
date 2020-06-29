using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StockGenerator_MultiDef : StockGenerator
	{
		
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			ThingDef thingDef = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, base.RandomCountOf(thingDef)))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string text in this.ConfigErrors(parentDef))
			{
				
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.thingDefs.Count; i = num + 1)
			{
				if (!this.thingDefs[i].tradeability.TraderCanSell())
				{
					yield return this.thingDefs[i] + " tradeability doesn't allow traders to sell this thing";
				}
				num = i;
			}
			yield break;
			yield break;
		}

		
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
