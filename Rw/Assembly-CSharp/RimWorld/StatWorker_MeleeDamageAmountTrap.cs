using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101D RID: 4125
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x060062CC RID: 25292 RVA: 0x00225004 File Offset: 0x00223204
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x060062CD RID: 25293 RVA: 0x00225037 File Offset: 0x00223237
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
