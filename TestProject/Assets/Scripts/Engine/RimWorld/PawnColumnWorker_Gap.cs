using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		
		// (get) Token: 0x06005D50 RID: 23888 RVA: 0x00204A3E File Offset: 0x00202C3E
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
