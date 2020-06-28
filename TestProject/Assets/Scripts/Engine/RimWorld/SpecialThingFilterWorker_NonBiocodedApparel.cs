using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FED RID: 4077
	public class SpecialThingFilterWorker_NonBiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x060061D2 RID: 25042 RVA: 0x0021FEC4 File Offset: 0x0021E0C4
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && !EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x060061D3 RID: 25043 RVA: 0x0021FEDE File Offset: 0x0021E0DE
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel;
		}
	}
}
