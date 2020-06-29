using System;
using Verse;

namespace RimWorld
{
	
	public class ThoughtWorker_ListeningToHarpsichord : ThoughtWorker_MusicalInstrumentListeningBase
	{
		
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
