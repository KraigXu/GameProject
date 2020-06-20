using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBB RID: 3515
	public class StockGenerator_BuySlaves : StockGenerator
	{
		// Token: 0x0600553F RID: 21823 RVA: 0x001C5787 File Offset: 0x001C3987
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x001C5338 File Offset: 0x001C3538
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}
	}
}
