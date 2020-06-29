using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_AbilitySpawn : CompProperties_AbilityEffect
	{
		
		public CompProperties_AbilitySpawn()
		{
			this.compClass = typeof(CompAbilityEffect_Spawn);
		}

		
		public ThingDef thingDef;
	}
}
