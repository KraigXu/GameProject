using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Psylinkable : CompProperties
	{
		
		public CompProperties_Psylinkable()
		{
			this.compClass = typeof(CompPsylinkable);
		}

		
		public List<int> requiredSubplantCountPerPsylinkLevel;

		
		public MeditationFocusDef requiredFocus;

		
		public SoundDef linkSound;

		
		public string enoughPlantsLetterLabel;

		
		public string enoughPlantsLetterText;

		
		public string enoughPlantsLetterLevelText;
	}
}
