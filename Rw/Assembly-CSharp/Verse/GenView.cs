using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000300 RID: 768
	public static class GenView
	{
		// Token: 0x0600159D RID: 5533 RVA: 0x0007E2A9 File Offset: 0x0007C4A9
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0007E2B8 File Offset: 0x0007C4B8
		public static bool ShouldSpawnMotesAt(this IntVec3 loc, Map map)
		{
			if (map != Find.CurrentMap)
			{
				return false;
			}
			if (!loc.InBounds(map))
			{
				return false;
			}
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			return GenView.viewRect.Contains(loc);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0007E304 File Offset: 0x0007C504
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x04000E27 RID: 3623
		private static CellRect viewRect;

		// Token: 0x04000E28 RID: 3624
		private const int ViewRectMargin = 5;
	}
}
