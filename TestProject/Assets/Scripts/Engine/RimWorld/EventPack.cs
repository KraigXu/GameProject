using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F25 RID: 3877
	public struct EventPack
	{
		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06005EEE RID: 24302 RVA: 0x0020C602 File Offset: 0x0020A802
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06005EEF RID: 24303 RVA: 0x0020C60A File Offset: 0x0020A80A
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06005EF0 RID: 24304 RVA: 0x0020C612 File Offset: 0x0020A812
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x06005EF1 RID: 24305 RVA: 0x0020C61A File Offset: 0x0020A81A
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x0020C635 File Offset: 0x0020A835
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x06005EF3 RID: 24307 RVA: 0x0020C64C File Offset: 0x0020A84C
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x0020C667 File Offset: 0x0020A867
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x06005EF5 RID: 24309 RVA: 0x0020C670 File Offset: 0x0020A870
		public override string ToString()
		{
			if (this.Cell.IsValid)
			{
				return this.Tag + "-" + this.Cell;
			}
			return this.Tag;
		}

		// Token: 0x04003375 RID: 13173
		private string tagInt;

		// Token: 0x04003376 RID: 13174
		private IntVec3 cellInt;

		// Token: 0x04003377 RID: 13175
		private IEnumerable<IntVec3> cellsInt;
	}
}
