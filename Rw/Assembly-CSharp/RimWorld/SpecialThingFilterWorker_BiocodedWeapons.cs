using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEA RID: 4074
	public class SpecialThingFilterWorker_BiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x060061C9 RID: 25033 RVA: 0x0021FE3C File Offset: 0x0021E03C
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x0021FE53 File Offset: 0x0021E053
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon && def.HasComp(typeof(CompBiocodableWeapon));
		}
	}
}
