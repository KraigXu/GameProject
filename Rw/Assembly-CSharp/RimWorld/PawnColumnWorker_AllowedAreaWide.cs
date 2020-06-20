using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000EE4 RID: 3812
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x06005D6B RID: 23915 RVA: 0x00204CFE File Offset: 0x00202EFE
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
