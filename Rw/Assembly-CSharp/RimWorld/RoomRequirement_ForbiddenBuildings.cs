using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001046 RID: 4166
	public class RoomRequirement_ForbiddenBuildings : RoomRequirement
	{
		// Token: 0x0600638D RID: 25485 RVA: 0x00228AE4 File Offset: 0x00226CE4
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

		// Token: 0x04003C95 RID: 15509
		public List<string> buildingTags = new List<string>();
	}
}
