using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DE RID: 2014
	public class ThinkNode_ConditionalDangerousTemperature : ThinkNode_Conditional
	{
		// Token: 0x060033AF RID: 13231 RVA: 0x0011DC20 File Offset: 0x0011BE20
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature);
		}
	}
}
