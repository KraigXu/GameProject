using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006A9 RID: 1705
	public static class DrugAIUtility
	{
		// Token: 0x06002E29 RID: 11817 RVA: 0x00103AE8 File Offset: 0x00101CE8
		public static Job IngestAndTakeToInventoryJob(Thing drug, Pawn pawn, int maxNumToCarry = 9999)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, drug);
			job.count = Mathf.Min(new int[]
			{
				drug.stackCount,
				drug.def.ingestible.maxNumToIngestAtOnce,
				maxNumToCarry
			});
			if (drug.Spawned && pawn.drugs != null && !pawn.inventory.innerContainer.Contains(drug.def))
			{
				DrugPolicyEntry drugPolicyEntry = pawn.drugs.CurrentPolicy[drug.def];
				if (drugPolicyEntry.allowScheduled)
				{
					job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
				}
			}
			return job;
		}
	}
}
