using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_UseEffectPlaySound : CompProperties_Usable
	{
		
		public CompProperties_UseEffectPlaySound()
		{
			this.compClass = typeof(CompUseEffect_PlaySound);
		}

		
		public SoundDef soundOnUsed;
	}
}
