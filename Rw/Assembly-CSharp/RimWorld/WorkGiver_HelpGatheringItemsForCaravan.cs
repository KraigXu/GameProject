using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000749 RID: 1865
	public class WorkGiver_HelpGatheringItemsForCaravan : WorkGiver
	{
		// Token: 0x060030E1 RID: 12513 RVA: 0x00111FAC File Offset: 0x001101AC
		public override Job NonScanJob(Pawn pawn)
		{
			List<Lord> lords = pawn.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[i].LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
				{
					Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lords[i]);
					if (thing != null && this.AnyReachableCarrierOrColonist(pawn, lords[i]))
					{
						Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherItems, thing);
						job.lord = lords[i];
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x00112038 File Offset: 0x00110238
		private bool AnyReachableCarrierOrColonist(Pawn forPawn, Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(lord.ownedPawns[i], forPawn, false) && !lord.ownedPawns[i].IsForbidden(forPawn) && forPawn.CanReach(lord.ownedPawns[i], PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
