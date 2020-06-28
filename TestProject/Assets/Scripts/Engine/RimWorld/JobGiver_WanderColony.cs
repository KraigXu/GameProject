using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000717 RID: 1815
	public class JobGiver_WanderColony : JobGiver_Wander
	{
		// Token: 0x06002FBE RID: 12222 RVA: 0x0010CD0C File Offset: 0x0010AF0C
		public JobGiver_WanderColony()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => true);
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0010CD61 File Offset: 0x0010AF61
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.GetColonyWanderRoot(pawn);
		}
	}
}
