using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB8 RID: 3512
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x06005536 RID: 21814 RVA: 0x001C56D4 File Offset: 0x001C38D4
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			yield break;
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x001C56E0 File Offset: 0x001C38E0
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && (thingDef == ThingDefOf.InsectJelly || thingDef.BaseMarketValue / thingDef.VolumePerUnit >= this.minValuePerUnit);
		}

		// Token: 0x04002EAE RID: 11950
		public float minValuePerUnit = 15f;
	}
}
