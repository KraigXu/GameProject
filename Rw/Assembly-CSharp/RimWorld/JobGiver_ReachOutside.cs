using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000713 RID: 1811
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		// Token: 0x06002FB6 RID: 12214 RVA: 0x0010CB64 File Offset: 0x0010AD64
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room.PsychologicallyOutdoors && room.TouchesMapEdge)
			{
				return null;
			}
			if (!pawn.CanReachMapEdge())
			{
				return null;
			}
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out intVec))
			{
				return null;
			}
			if (intVec == pawn.Position)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec);
		}
	}
}
