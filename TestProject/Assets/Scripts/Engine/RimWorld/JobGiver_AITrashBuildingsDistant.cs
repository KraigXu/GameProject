using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006B8 RID: 1720
	public class JobGiver_AITrashBuildingsDistant : ThinkNode_JobGiver
	{
		// Token: 0x06002E63 RID: 11875 RVA: 0x00104AFD File Offset: 0x00102CFD
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AITrashBuildingsDistant jobGiver_AITrashBuildingsDistant = (JobGiver_AITrashBuildingsDistant)base.DeepCopy(resolve);
			jobGiver_AITrashBuildingsDistant.attackAllInert = this.attackAllInert;
			return jobGiver_AITrashBuildingsDistant;
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x00104B18 File Offset: 0x00102D18
		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			if (allBuildingsColonist.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < 75; i++)
			{
				Building building = allBuildingsColonist.RandomElement<Building>();
				if (TrashUtility.ShouldTrashBuilding(pawn, building, this.attackAllInert))
				{
					Job job = TrashUtility.TrashJob(pawn, building, this.attackAllInert);
					if (job != null)
					{
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x04001A6C RID: 6764
		public bool attackAllInert;
	}
}
