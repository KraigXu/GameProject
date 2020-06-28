using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB9 RID: 3513
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x06005539 RID: 21817 RVA: 0x001C574D File Offset: 0x001C394D
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x001C5756 File Offset: 0x001C3956
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x04002EAF RID: 11951
		public ThingDef thingDef;
	}
}
