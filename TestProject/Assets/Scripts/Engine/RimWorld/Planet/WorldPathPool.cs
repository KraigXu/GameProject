using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldPathPool
	{
		
		// (get) Token: 0x060068F7 RID: 26871 RVA: 0x0024AB02 File Offset: 0x00248D02
		public static WorldPath NotFoundPath
		{
			get
			{
				return WorldPathPool.notFoundPathInt;
			}
		}

		
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

		
		private List<WorldPath> paths = new List<WorldPath>(64);

		
		private static readonly WorldPath notFoundPathInt = WorldPath.NewNotFound();
	}
}
