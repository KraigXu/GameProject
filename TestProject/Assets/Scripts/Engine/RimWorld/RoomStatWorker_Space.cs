using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A9E RID: 2718
	public class RoomStatWorker_Space : RoomStatWorker
	{
		// Token: 0x06004004 RID: 16388 RVA: 0x00154880 File Offset: 0x00152A80
		public override float GetScore(Room room)
		{
			if (room.PsychologicallyOutdoors)
			{
				return 350f;
			}
			float num = 0f;
			foreach (IntVec3 c in room.Cells)
			{
				if (c.Standable(room.Map))
				{
					num += 1.4f;
				}
				else if (c.Walkable(room.Map))
				{
					num += 0.5f;
				}
			}
			return Mathf.Min(num, 350f);
		}
	}
}
