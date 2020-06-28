using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DF RID: 1759
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		// Token: 0x06002EDC RID: 11996 RVA: 0x0010751C File Offset: 0x0010571C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (Rand.Value < 1f && RCellFinder.TryFindRandomCellNearWith(pawn.Position, (IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire, pawn.Map, out c, 5, this.MaxDistance.RandomInRange))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		// Token: 0x04001A91 RID: 6801
		private const float ActivateChance = 1f;

		// Token: 0x04001A92 RID: 6802
		private readonly IntRange MaxDistance = new IntRange(10, 16);
	}
}
