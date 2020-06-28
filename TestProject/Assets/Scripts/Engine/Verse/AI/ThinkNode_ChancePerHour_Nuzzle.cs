using System;

namespace Verse.AI
{
	// Token: 0x02000588 RID: 1416
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06002852 RID: 10322 RVA: 0x000EE8CD File Offset: 0x000ECACD
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
