using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE1 RID: 4065
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x060061A6 RID: 24998 RVA: 0x0021FB58 File Offset: 0x0021DD58
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x060061A7 RID: 24999 RVA: 0x0021FB88 File Offset: 0x0021DD88
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
