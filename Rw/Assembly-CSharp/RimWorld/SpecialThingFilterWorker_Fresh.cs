using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE2 RID: 4066
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x060061A9 RID: 25001 RVA: 0x0021FBAC File Offset: 0x0021DDAC
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return t.def.IsIngestible;
			}
			return compRottable.Stage == RotStage.Fresh;
		}

		// Token: 0x060061AA RID: 25002 RVA: 0x0021FBDD File Offset: 0x0021DDDD
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		// Token: 0x060061AB RID: 25003 RVA: 0x0021FBF0 File Offset: 0x0021DDF0
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
