using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006BF RID: 1727
	public class JobGiver_UnloadMyLordCarriers : ThinkNode_JobGiver
	{
		// Token: 0x06002E7D RID: 11901 RVA: 0x001055F0 File Offset: 0x001037F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, lord.ownedPawns[i], false))
				{
					return JobMaker.MakeJob(JobDefOf.UnloadInventory, lord.ownedPawns[i]);
				}
			}
			return null;
		}
	}
}
