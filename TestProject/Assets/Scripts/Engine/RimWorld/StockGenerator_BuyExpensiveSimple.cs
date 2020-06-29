using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && (thingDef == ThingDefOf.InsectJelly || thingDef.BaseMarketValue / thingDef.VolumePerUnit >= this.minValuePerUnit);
		}

		
		public float minValuePerUnit = 15f;
	}
}
