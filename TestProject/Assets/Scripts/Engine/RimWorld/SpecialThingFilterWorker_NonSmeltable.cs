using System;
using Verse;

namespace RimWorld
{
	
	public class SpecialThingFilterWorker_NonSmeltable : SpecialThingFilterWorker
	{
		
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.Smeltable;
		}

		
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.smeltable || def.MadeFromStuff;
		}

		
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.smeltable && !def.MadeFromStuff;
		}
	}
}
