using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002FE RID: 766
	public class LinkGrid
	{
		// Token: 0x06001594 RID: 5524 RVA: 0x0007E014 File Offset: 0x0007C214
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0007E039 File Offset: 0x0007C239
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0007E054 File Offset: 0x0007C254
		public void Notify_LinkerCreatedOrDestroyed(Thing linker)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in linker.OccupiedRect())
			{
				LinkFlags linkFlags = LinkFlags.None;
				List<Thing> list = this.map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.graphicData != null)
					{
						linkFlags |= list[i].def.graphicData.linkFlags;
					}
				}
				this.linkGrid[cellIndices.CellToIndex(c)] = linkFlags;
			}
		}

		// Token: 0x04000E20 RID: 3616
		private Map map;

		// Token: 0x04000E21 RID: 3617
		private LinkFlags[] linkGrid;
	}
}
