using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		
		public float brainDamageChance = 0.3f;
	}
}
