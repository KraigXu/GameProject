using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000852 RID: 2130
	public class ThoughtWorker_ListeningToHarpsichord : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060034C2 RID: 13506 RVA: 0x00120D8C File Offset: 0x0011EF8C
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harpsichord;
			}
		}
	}
}
