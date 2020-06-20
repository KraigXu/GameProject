using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC1 RID: 3521
	public class StockGenerator_Art : StockGenerator_MiscItems
	{
		// Token: 0x06005558 RID: 21848 RVA: 0x001C5B5F File Offset: 0x001C3D5F
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.Minifiable && td.category == ThingCategory.Building && td.thingClass == typeof(Building_Art);
		}

		// Token: 0x06005559 RID: 21849 RVA: 0x001C5B92 File Offset: 0x001C3D92
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Art.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04002EB9 RID: 11961
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 0.2f),
				true
			}
		};
	}
}
