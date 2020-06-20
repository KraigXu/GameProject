using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C5 RID: 1989
	public class ThinkNode_ConditionalBodySize : ThinkNode_Conditional
	{
		// Token: 0x0600337B RID: 13179 RVA: 0x0011D8BC File Offset: 0x0011BABC
		protected override bool Satisfied(Pawn pawn)
		{
			float bodySize = pawn.BodySize;
			return bodySize >= this.min && bodySize <= this.max;
		}

		// Token: 0x04001BA6 RID: 7078
		public float min;

		// Token: 0x04001BA7 RID: 7079
		public float max = 99999f;
	}
}
