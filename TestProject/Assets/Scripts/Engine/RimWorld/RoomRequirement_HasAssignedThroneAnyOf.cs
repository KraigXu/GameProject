using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001047 RID: 4167
	public class RoomRequirement_HasAssignedThroneAnyOf : RoomRequirement_ThingAnyOf
	{
		// Token: 0x0600638F RID: 25487 RVA: 0x00228B98 File Offset: 0x00226D98
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
