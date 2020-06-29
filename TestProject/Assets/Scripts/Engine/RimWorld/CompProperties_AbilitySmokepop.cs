using System;

namespace RimWorld
{
	
	public class CompProperties_AbilitySmokepop : CompProperties_AbilityEffect
	{
		
		public CompProperties_AbilitySmokepop()
		{
			this.compClass = typeof(CompAbilityEffect_Smokepop);
		}

		
		public float smokeRadius;
	}
}
