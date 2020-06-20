using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000759 RID: 1881
	public class WorkGiver_RescueDowned : WorkGiver_TakeToBed
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003137 RID: 12599 RVA: 0x0001028D File Offset: 0x0000E48D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x0010F64C File Offset: 0x0010D84C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x00113253 File Offset: 0x00111453
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedDownedPawns;
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x00113268 File Offset: 0x00111468
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Downed && !list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x001132BC File Offset: 0x001114BC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.Downed || pawn2.Faction != pawn.Faction || pawn2.InBed() || !pawn.CanReserve(pawn2, 1, -1, null, forced) || GenAI.EnemyIsNear(pawn2, 40f))
			{
				return false;
			}
			Thing thing = base.FindBed(pawn, pawn2);
			return thing != null && pawn2.CanReserve(thing, 1, -1, null, false);
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x00113334 File Offset: 0x00111534
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, t2);
			job.count = 1;
			return job;
		}

		// Token: 0x04001B02 RID: 6914
		private const float MinDistFromEnemy = 40f;
	}
}
