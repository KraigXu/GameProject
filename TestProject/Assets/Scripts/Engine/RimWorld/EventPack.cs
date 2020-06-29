using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public struct EventPack
	{
		
		// (get) Token: 0x06005EEE RID: 24302 RVA: 0x0020C602 File Offset: 0x0020A802
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		
		// (get) Token: 0x06005EEF RID: 24303 RVA: 0x0020C60A File Offset: 0x0020A80A
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		
		// (get) Token: 0x06005EF0 RID: 24304 RVA: 0x0020C612 File Offset: 0x0020A812
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		
		public override string ToString()
		{
			if (this.Cell.IsValid)
			{
				return this.Tag + "-" + this.Cell;
			}
			return this.Tag;
		}

		
		private string tagInt;

		
		private IntVec3 cellInt;

		
		private IEnumerable<IntVec3> cellsInt;
	}
}
