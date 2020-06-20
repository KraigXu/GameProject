using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C9 RID: 1737
	public class JobGiver_WanderInGatheringArea : JobGiver_Wander
	{
		// Token: 0x06002E98 RID: 11928 RVA: 0x00105DFC File Offset: 0x00103FFC
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 result;
			if (!GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, out result))
			{
				return IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000255BF File Offset: 0x000237BF
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}
	}
}
