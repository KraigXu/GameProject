using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		
		public bool psychicSensitiveTargetsOnly;

		
		public bool fleshCorpsesOnly;

		
		public bool nonDessicatedCorpsesOnly;

		
		public bool nonDownedPawnOnly;

		
		public bool ignoreQuestLodgerPawns;

		
		public bool ignorePlayerFactionPawns;

		
		public ThingDef moteOnTarget;

		
		public ThingDef moteConnecting;
	}
}
