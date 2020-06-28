using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200015D RID: 349
	public sealed class DynamicDrawManager
	{
		// Token: 0x060009CE RID: 2510 RVA: 0x00035455 File Offset: 0x00033655
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0003546F File Offset: 0x0003366F
		public void RegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + t + " while drawing is in progress. Things shouldn't be spawned in Draw methods.", false);
				}
				this.drawThings.Add(t);
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x000354A9 File Offset: 0x000336A9
		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + t + " while drawing is in progress. Things shouldn't be despawned in Draw methods.", false);
				}
				this.drawThings.Remove(t);
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000354E4 File Offset: 0x000336E4
		public void DrawDynamicThings()
		{
			if (!DebugViewSettings.drawThingsDynamic)
			{
				return;
			}
			this.drawingNow = true;
			try
			{
				bool[] fogGrid = this.map.fogGrid.fogGrid;
				CellRect cellRect = Find.CameraDriver.CurrentViewRect;
				cellRect.ClipInsideMap(this.map);
				cellRect = cellRect.ExpandedBy(1);
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Thing thing in this.drawThings)
				{
					IntVec3 position = thing.Position;
					if ((cellRect.Contains(position) || thing.def.drawOffscreen) && (!fogGrid[cellIndices.CellToIndex(position)] || thing.def.seeThroughFog) && (thing.def.hideAtSnowDepth >= 1f || this.map.snowGrid.GetDepth(position) <= thing.def.hideAtSnowDepth))
					{
						try
						{
							thing.Draw();
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception drawing ",
								thing,
								": ",
								ex.ToString()
							}), false);
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic things: " + arg, false);
			}
			this.drawingNow = false;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00035690 File Offset: 0x00033890
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings), false);
		}

		// Token: 0x040007FD RID: 2045
		private Map map;

		// Token: 0x040007FE RID: 2046
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x040007FF RID: 2047
		private bool drawingNow;
	}
}
