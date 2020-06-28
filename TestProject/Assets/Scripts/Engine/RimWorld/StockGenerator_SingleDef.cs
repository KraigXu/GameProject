using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB1 RID: 3505
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x06005522 RID: 21794 RVA: 0x001C52E7 File Offset: 0x001C34E7
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef)))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x001C52F7 File Offset: 0x001C34F7
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x001C5302 File Offset: 0x001C3502
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!this.thingDef.tradeability.TraderCanSell())
			{
				yield return this.thingDef + " tradeability doesn't allow traders to sell this thing";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002EA4 RID: 11940
		private ThingDef thingDef;
	}
}
