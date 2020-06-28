using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000851 RID: 2129
	public class ThoughtWorker_ListeningToHarp : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060034C0 RID: 13504 RVA: 0x00120D7D File Offset: 0x0011EF7D
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harp;
			}
		}
	}
}
