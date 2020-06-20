using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070C RID: 1804
	public class JobGiver_DropRandomGearOrApparel : ThinkNode_JobGiver
	{
		// Token: 0x06002FA2 RID: 12194 RVA: 0x0010C540 File Offset: 0x0010A740
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment != null && pawn.equipment.HasAnything())
			{
				return JobMaker.MakeJob(JobDefOf.DropEquipment, pawn.equipment.AllEquipmentListForReading.RandomElement<ThingWithComps>());
			}
			if (pawn.apparel != null && pawn.apparel.WornApparel.Any<Apparel>())
			{
				return JobMaker.MakeJob(JobDefOf.RemoveApparel, pawn.apparel.WornApparel.RandomElement<Apparel>());
			}
			return null;
		}
	}
}
