using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_AssignableToPawn : CompProperties
	{
		
		public CompProperties_AssignableToPawn()
		{
			this.compClass = typeof(CompAssignableToPawn);
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		
		public override void PostLoadSpecial(ThingDef parent)
		{
			if (parent.thingClass == typeof(Building_Bed))
			{
				this.maxAssignedPawnsCount = BedUtility.GetSleepingSlotsCount(parent.size);
			}
		}

		
		public int maxAssignedPawnsCount = 1;

		
		public bool drawAssignmentOverlay = true;

		
		public bool drawUnownedAssignmentOverlay = true;

		
		public string singleton;
	}
}
