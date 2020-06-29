using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_ToggleDrawAffectedMeditationFoci : CompProperties
	{
		
		public CompProperties_ToggleDrawAffectedMeditationFoci()
		{
			this.compClass = typeof(CompToggleDrawAffectedMeditationFoci);
		}

		
		public bool defaultEnabled = true;
	}
}
