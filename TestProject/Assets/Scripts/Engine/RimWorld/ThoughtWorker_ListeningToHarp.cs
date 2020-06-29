using System;
using Verse;

namespace RimWorld
{
	
	public class ThoughtWorker_ListeningToHarp : ThoughtWorker_MusicalInstrumentListeningBase
	{
		
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
