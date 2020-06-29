using System;
using Verse;

namespace RimWorld
{
	
	public class SpecialThingFilterWorker_BiocodedWeapons : SpecialThingFilterWorker
	{
		
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && EquipmentUtility.IsBiocoded(t);
		}

		
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon && def.HasComp(typeof(CompBiocodableWeapon));
		}
	}
}
