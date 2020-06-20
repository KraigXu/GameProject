using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011BB RID: 4539
	public class WorldPathPool
	{
		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x060068F7 RID: 26871 RVA: 0x0024AB02 File Offset: 0x00248D02
		public static WorldPath NotFoundPath
		{
			get
			{
				return WorldPathPool.notFoundPathInt;
			}
		}

		// Token: 0x060068F9 RID: 26873 RVA: 0x0024AB18 File Offset: 0x00248D18
		public WorldPath GetEmptyWorldPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > Find.WorldObjects.CaravansCount + 2 + (Find.WorldObjects.RoutePlannerWaypointsCount - 1))
			{
				Log.ErrorOnce("WorldPathPool leak: more paths than caravans. Force-recovering.", 664788, false);
				this.paths.Clear();
			}
			WorldPath worldPath = new WorldPath();
			this.paths.Add(worldPath);
			worldPath.inUse = true;
			return worldPath;
		}

		// Token: 0x04004152 RID: 16722
		private List<WorldPath> paths = new List<WorldPath>(64);

		// Token: 0x04004153 RID: 16723
		private static readonly WorldPath notFoundPathInt = WorldPath.NewNotFound();
	}
}
