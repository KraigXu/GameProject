using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBD RID: 3517
	public class StockGenerator_WeaponsRanged : StockGenerator_MiscItems
	{
		// Token: 0x06005548 RID: 21832 RVA: 0x001C57F1 File Offset: 0x001C39F1
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsRangedWeapon && (this.weaponTag == null || (td.weaponTags != null && td.weaponTags.Contains(this.weaponTag)));
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x001C582B File Offset: 0x001C3A2B
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsRanged.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04002EB1 RID: 11953
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

		// Token: 0x04002EB2 RID: 11954
		public string weaponTag;
	}
}
