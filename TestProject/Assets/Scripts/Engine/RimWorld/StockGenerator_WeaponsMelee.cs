using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBE RID: 3518
	public class StockGenerator_WeaponsMelee : StockGenerator_MiscItems
	{
		// Token: 0x0600554C RID: 21836 RVA: 0x001C58B7 File Offset: 0x001C3AB7
		public override bool HandlesThingDef(ThingDef td)
		{
			return base.HandlesThingDef(td) && td.IsMeleeWeapon && (this.weaponTag == null || (td.weaponTags != null && td.weaponTags.Contains(this.weaponTag)));
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x001C58F1 File Offset: 0x001C3AF1
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_WeaponsMelee.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x04002EB3 RID: 11955
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

		// Token: 0x04002EB4 RID: 11956
		public string weaponTag;
	}
}
