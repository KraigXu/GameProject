using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DF RID: 2015
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		// Token: 0x060033B1 RID: 13233 RVA: 0x0011DC44 File Offset: 0x0011BE44
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
