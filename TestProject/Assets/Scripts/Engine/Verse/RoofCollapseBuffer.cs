using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001CD RID: 461
	public class RoofCollapseBuffer
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0004A98C File Offset: 0x00048B8C
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0004A994 File Offset: 0x00048B94
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0004A9A2 File Offset: 0x00048BA2
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0004A9BE File Offset: 0x00048BBE
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x04000A2D RID: 2605
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
