using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Flickable : CompProperties
	{
		
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
