using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200069A RID: 1690
	public class JobGiver_WanderHive : JobGiver_Wander
	{
		// Token: 0x06002DF0 RID: 11760 RVA: 0x0010275E File Offset: 0x0010095E
		public JobGiver_WanderHive()
		{
			this.wanderRadius = 7.5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x00102784 File Offset: 0x00100984
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive == null || !hive.Spawned)
			{
				return pawn.Position;
			}
			return hive.Position;
		}
	}
}
