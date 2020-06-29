using System;
using Verse;

namespace RimWorld
{
	
	public class ThoughtWorker_ListeningToPiano : ThoughtWorker_MusicalInstrumentListeningBase
	{
		
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
