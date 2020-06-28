using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D0 RID: 1744
	public class JobGiver_LoadTransporters : ThinkNode_JobGiver
	{
		// Token: 0x06002EAB RID: 11947 RVA: 0x00106358 File Offset: 0x00104558
		protected override Job TryGiveJob(Pawn pawn)
		{
			TransporterUtility.GetTransportersInGroup(pawn.mindState.duty.transportersGroup, pawn.Map, JobGiver_LoadTransporters.tmpTransporters);
			for (int i = 0; i < JobGiver_LoadTransporters.tmpTransporters.Count; i++)
			{
				CompTransporter transporter = JobGiver_LoadTransporters.tmpTransporters[i];
				if (LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter))
				{
					return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
				}
			}
			return null;
		}

		// Token: 0x04001A7D RID: 6781
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}
