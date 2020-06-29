using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Mannable : CompProperties
	{
		
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}

		
		public WorkTags manWorkType;
	}
}
