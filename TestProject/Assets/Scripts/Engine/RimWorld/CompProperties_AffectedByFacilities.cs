using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		
		public List<ThingDef> linkableFacilities;
	}
}
