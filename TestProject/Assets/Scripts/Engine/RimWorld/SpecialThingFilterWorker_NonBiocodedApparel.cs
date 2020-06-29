using System;
using Verse;

namespace RimWorld
{
	
	public class SpecialThingFilterWorker_NonBiocodedApparel : SpecialThingFilterWorker
	{
		
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && !EquipmentUtility.IsBiocoded(t);
		}

		
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel;
		}
	}
}
