using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC0 RID: 3520
	public class StockGenerator_Clothes : StockGenerator_MiscItems
	{
		// Token: 0x06005554 RID: 21844 RVA: 0x001C5A5C File Offset: 0x001C3C5C
		public override bool HandlesThingDef(ThingDef td)
		{
			return td != ThingDefOf.Apparel_ShieldBelt && (base.HandlesThingDef(td) && td.IsApparel && (this.apparelTag == null || (td.apparel.tags != null && td.apparel.tags.Contains(this.apparelTag)))) && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null) < 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null) < 0.15f);
		}

		// Token: 0x06005555 RID: 21845 RVA: 0x001C5ADB File Offset: 0x001C3CDB
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Clothes.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04002EB7 RID: 11959
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 0.5f),
				true
			},
			{
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x04002EB8 RID: 11960
		public string apparelTag;
	}
}
