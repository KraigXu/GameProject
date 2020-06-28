using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001D3 RID: 467
	public static class RoofUtility
	{
		// Token: 0x06000D42 RID: 3394 RVA: 0x0004B928 File Offset: 0x00049B28
		public static Thing FirstBlockingThing(IntVec3 pos, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(pos);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.plant != null && list[i].def.plant.interferesWithRoof)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0004B988 File Offset: 0x00049B88
		public static bool IsAnyCellUnderRoof(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool result = false;
			RoofGrid roofGrid = thing.Map.roofGrid;
			foreach (IntVec3 c in cellRect)
			{
				if (roofGrid.Roofed(c))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0004B9F8 File Offset: 0x00049BF8
		public static bool CanHandleBlockingThing(Thing blocker, Pawn worker, bool forced = false)
		{
			return blocker == null || (blocker.def.category == ThingCategory.Plant && worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced));
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0004BA29 File Offset: 0x00049C29
		public static Job HandleBlockingThingJob(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return null;
			}
			if (blocker.def.category == ThingCategory.Plant && worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
			{
				return JobMaker.MakeJob(JobDefOf.CutPlant, blocker);
			}
			return null;
		}
	}
}
