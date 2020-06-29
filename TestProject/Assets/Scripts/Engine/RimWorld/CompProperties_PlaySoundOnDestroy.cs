using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_PlaySoundOnDestroy : CompProperties
	{
		
		public CompProperties_PlaySoundOnDestroy()
		{
			this.compClass = typeof(CompPlaySoundOnDestroy);
		}

		
		public SoundDef sound;
	}
}
