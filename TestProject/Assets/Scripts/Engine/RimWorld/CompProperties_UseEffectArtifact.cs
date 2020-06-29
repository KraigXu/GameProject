using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_UseEffectArtifact : CompProperties_UseEffect
	{
		
		public CompProperties_UseEffectArtifact()
		{
			this.compClass = typeof(CompUseEffect_Artifact);
		}

		
		public SoundDef sound;
	}
}
