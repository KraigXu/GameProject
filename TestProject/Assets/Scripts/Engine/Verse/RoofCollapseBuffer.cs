using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class RoofCollapseBuffer
	{
		
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0004A98C File Offset: 0x00048B8C
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
