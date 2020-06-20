using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEC RID: 4076
	public class SpecialThingFilterWorker_BiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x060061CF RID: 25039 RVA: 0x0021FE91 File Offset: 0x0021E091
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x060061D0 RID: 25040 RVA: 0x0021FEA8 File Offset: 0x0021E0A8
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.HasComp(typeof(CompBiocodableApparel));
		}
	}
}
