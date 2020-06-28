using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBA RID: 3514
	public class StockGenerator_BuyTradeTag : StockGenerator
	{
		// Token: 0x0600553C RID: 21820 RVA: 0x001C5761 File Offset: 0x001C3961
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x0600553D RID: 21821 RVA: 0x001C576A File Offset: 0x001C396A
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tag);
		}

		// Token: 0x04002EB0 RID: 11952
		public string tag;
	}
}
