using System;

namespace Verse.AI
{
	// Token: 0x02000589 RID: 1417
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06002854 RID: 10324 RVA: 0x000EE8E2 File Offset: 0x000ECAE2
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
