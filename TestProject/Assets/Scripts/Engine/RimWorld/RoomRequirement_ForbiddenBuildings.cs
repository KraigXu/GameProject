using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RoomRequirement_ForbiddenBuildings : RoomRequirement
	{
		
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				if (thing.def.building != null)
				{
					for (int i = 0; i < this.buildingTags.Count; i++)
					{
						string item = this.buildingTags[i];
						if (thing.def.building.buildingTags.Contains(item))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		
		public List<string> buildingTags = new List<string>();
	}
}
