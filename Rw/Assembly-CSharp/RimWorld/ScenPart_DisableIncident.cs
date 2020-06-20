using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000C1A RID: 3098
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x001901B3 File Offset: 0x0018E3B3
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x001901BA File Offset: 0x0018E3BA
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.TraderCaravanArrival;
			yield return IncidentDefOf.OrbitalTraderArrival;
			yield return IncidentDefOf.WandererJoin;
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}
	}
}
