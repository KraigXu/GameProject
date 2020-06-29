using System;
using Verse;

namespace RimWorld
{
	
	public class ThingSetMaker_Conditional_ResearchFinished : ThingSetMaker_Conditional
	{
		
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return this.researchProject.IsFinished;
		}

		
		public ResearchProjectDef researchProject;
	}
}
