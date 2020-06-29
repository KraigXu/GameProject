using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StockGenerator_BuyTradeTag : StockGenerator
	{
		
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tag);
		}

		
		public string tag;
	}
}
