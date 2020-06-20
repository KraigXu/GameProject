using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBF RID: 3519
	public class StockGenerator_Armor : StockGenerator_MiscItems
	{
		// Token: 0x06005550 RID: 21840 RVA: 0x001C5974 File Offset: 0x001C3B74
		public override bool HandlesThingDef(ThingDef td)
		{
			if (td == ThingDefOf.Apparel_ShieldBelt)
			{
				return true;
			}
			if (td == ThingDefOf.Apparel_SmokepopBelt)
			{
				return true;
			}
			ThingDef stuff = GenStuff.DefaultStuffFor(td);
			return base.HandlesThingDef(td) && td.IsApparel && (td.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff) > 0.15f || td.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff) > 0.15f);
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x001C59D7 File Offset: 0x001C3BD7
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_Armor.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04002EB5 RID: 11957
		public const float MinArmor = 0.15f;

		// Token: 0x04002EB6 RID: 11958
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
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};
	}
}
