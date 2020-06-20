using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DA RID: 2010
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		// Token: 0x060033A7 RID: 13223 RVA: 0x0011DBBE File Offset: 0x0011BDBE
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
