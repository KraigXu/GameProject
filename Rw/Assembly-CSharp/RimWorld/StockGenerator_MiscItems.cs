using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBC RID: 3516
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06005542 RID: 21826 RVA: 0x001C5790 File Offset: 0x001C3990
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			int count = this.countRange.RandomInRange;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				ThingDef def;
				if (!(from t in DefDatabase<ThingDef>.AllDefs
				where this.HandlesThingDef(t) && t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate
				select t).TryRandomElementByWeight(new Func<ThingDef, float>(this.SelectionWeight), out def))
				{
					yield break;
				}
				yield return this.MakeThing(def);
				num = i;
			}
			yield break;
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x001C57A0 File Offset: 0x001C39A0
		protected virtual Thing MakeThing(ThingDef def)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1);
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x001C57A9 File Offset: 0x001C39A9
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
