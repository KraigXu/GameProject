using System;
using Verse;

namespace RimWorld
{
	
	public class SpecialThingFilterWorker_NonBiocodedWeapons : SpecialThingFilterWorker
	{
		
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && !EquipmentUtility.IsBiocoded(t);
		}

		
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon;
		}
	}
}
