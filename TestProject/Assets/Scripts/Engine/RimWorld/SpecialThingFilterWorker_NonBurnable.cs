using System;
using Verse;

namespace RimWorld
{
	
	public class SpecialThingFilterWorker_NonBurnable : SpecialThingFilterWorker
	{
		
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.burnableByRecipe || def.MadeFromStuff;
		}

		
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
