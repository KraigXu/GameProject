using System;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_HasAssignedThroneAnyOf : RoomRequirement_ThingAnyOf
	{
		
		public override bool Met(Room r, Pawn p = null)
		{
			if (p == null)
			{
				return false;
			}
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				if (this.things.Contains(thing.def) && p.ownership.AssignedThrone == thing)
				{
					return true;
				}
			}
			return false;
		}
	}
}
