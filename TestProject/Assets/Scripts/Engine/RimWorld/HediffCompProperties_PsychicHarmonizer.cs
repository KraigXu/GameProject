using System;
using Verse;

namespace RimWorld
{
	
	public class HediffCompProperties_PsychicHarmonizer : HediffCompProperties
	{
		
		public HediffCompProperties_PsychicHarmonizer()
		{
			this.compClass = typeof(HediffComp_PsychicHarmonizer);
		}

		
		public float range;

		
		public ThoughtDef thought;
	}
}
