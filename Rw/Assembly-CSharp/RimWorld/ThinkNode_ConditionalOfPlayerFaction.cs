using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CB RID: 1995
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x06003387 RID: 13191 RVA: 0x0011D962 File Offset: 0x0011BB62
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
