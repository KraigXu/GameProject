using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE0 RID: 3808
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x06005D50 RID: 23888 RVA: 0x00204A3E File Offset: 0x00202C3E
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x00002681 File Offset: 0x00000881
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x00204A4B File Offset: 0x00202C4B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x00204A5F File Offset: 0x00202C5F
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
