using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006EE RID: 1774
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		// Token: 0x06002F0C RID: 12044 RVA: 0x00108BCF File Offset: 0x00106DCF
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.inventory.UnloadEverything)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.UnloadYourInventory);
		}
	}
}
