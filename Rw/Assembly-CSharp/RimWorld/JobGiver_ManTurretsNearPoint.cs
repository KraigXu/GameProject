using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006D2 RID: 1746
	public class JobGiver_ManTurretsNearPoint : JobGiver_ManTurrets
	{
		// Token: 0x06002EB2 RID: 11954 RVA: 0x00106481 File Offset: 0x00104681
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.GetLord().CurLordToil.FlagLoc;
		}
	}
}
