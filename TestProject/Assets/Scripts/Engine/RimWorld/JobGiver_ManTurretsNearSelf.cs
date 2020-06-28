using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D3 RID: 1747
	public class JobGiver_ManTurretsNearSelf : JobGiver_ManTurrets
	{
		// Token: 0x06002EB4 RID: 11956 RVA: 0x000EFC74 File Offset: 0x000EDE74
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
