using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000853 RID: 2131
	public class ThoughtWorker_ListeningToPiano : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060034C4 RID: 13508 RVA: 0x00120D93 File Offset: 0x0011EF93
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Piano;
			}
		}
	}
}
