using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_CausesGameCondition : CompProperties
	{
		
		public CompProperties_CausesGameCondition()
		{
			this.compClass = typeof(CompCauseGameCondition);
		}

		
		public GameConditionDef conditionDef;

		
		public int worldRange;

		
		public bool preventConditionStacking = true;
	}
}
