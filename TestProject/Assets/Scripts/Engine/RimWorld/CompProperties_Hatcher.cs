using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Hatcher : CompProperties
	{
		
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}

		
		public float hatcherDaystoHatch = 1f;

		
		public PawnKindDef hatcherPawn;
	}
}
