using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE1 RID: 3809
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x06005D56 RID: 23894 RVA: 0x00002681 File Offset: 0x00000881
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x00127936 File Offset: 0x00125B36
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x00204A73 File Offset: 0x00202C73
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
