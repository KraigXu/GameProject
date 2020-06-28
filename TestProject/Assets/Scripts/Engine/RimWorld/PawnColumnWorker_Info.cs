using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDB RID: 3803
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x06005D3B RID: 23867 RVA: 0x002048AC File Offset: 0x00202AAC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x002048B6 File Offset: 0x00202AB6
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x002048B6 File Offset: 0x00202AB6
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}
