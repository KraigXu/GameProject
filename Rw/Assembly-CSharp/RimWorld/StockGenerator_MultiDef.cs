using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB0 RID: 3504
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x0600551D RID: 21789 RVA: 0x001C5296 File Offset: 0x001C3496
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

		// Token: 0x0600551E RID: 21790 RVA: 0x001C52A6 File Offset: 0x001C34A6
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x001C52B4 File Offset: 0x001C34B4
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
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

		// Token: 0x04002EA3 RID: 11939
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
