using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200069C RID: 1692
	public class JobGiver_LayEgg : ThinkNode_JobGiver
	{
		// Token: 0x06002DF4 RID: 11764 RVA: 0x0010284C File Offset: 0x00100A4C
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompEggLayer compEggLayer = pawn.TryGetComp<CompEggLayer>();
			if (compEggLayer == null || !compEggLayer.CanLayNow)
			{
				return null;
			}
			IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some);
			return JobMaker.MakeJob(JobDefOf.LayEgg, c);
		}

		// Token: 0x04001A48 RID: 6728
		private const float LayRadius = 5f;
	}
}
