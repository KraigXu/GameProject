using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ThingSetMaker_Conditional_MakingFaction : ThingSetMaker_Conditional
	{
		
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return (!this.requireNonNull || parms.makingFaction != null) && (this.makingFaction == null || (parms.makingFaction != null && parms.makingFaction.def == this.makingFaction)) && (this.makingFactionCategories.NullOrEmpty<string>() || (parms.makingFaction != null && this.makingFactionCategories.Contains(parms.makingFaction.def.categoryTag)));
		}

		
		public FactionDef makingFaction;

		
		public List<string> makingFactionCategories;

		
		public bool requireNonNull;
	}
}
