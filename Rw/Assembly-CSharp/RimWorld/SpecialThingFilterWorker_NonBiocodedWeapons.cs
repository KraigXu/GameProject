using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEB RID: 4075
	public class SpecialThingFilterWorker_NonBiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x060061CC RID: 25036 RVA: 0x0021FE6F File Offset: 0x0021E06F
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && !EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x060061CD RID: 25037 RVA: 0x0021FE89 File Offset: 0x0021E089
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon;
		}
	}
}
