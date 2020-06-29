using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_UseEffect : CompProperties
	{
		
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}

		
		public bool doCameraShake;

		
		public ThingDef moteOnUsed;

		
		public float moteOnUsedScale = 1f;
	}
}
