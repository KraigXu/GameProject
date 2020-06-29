using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_RoyalImplant : CompProperties
	{
		
		public CompProperties_RoyalImplant()
		{
			this.compClass = typeof(CompRoyalImplant);
		}

		
		public HediffDef implantHediff;
	}
}
