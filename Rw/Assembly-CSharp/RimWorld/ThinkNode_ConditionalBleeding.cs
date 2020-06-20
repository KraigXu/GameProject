using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DD RID: 2013
	public class ThinkNode_ConditionalBleeding : ThinkNode_Conditional
	{
		// Token: 0x060033AD RID: 13229 RVA: 0x0011DC06 File Offset: 0x0011BE06
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.health.hediffSet.BleedRateTotal > 0.001f;
		}
	}
}
