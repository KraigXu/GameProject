using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006EA RID: 1770
	public class JobGiver_RescueNearby : ThinkNode_JobGiver
	{
		// Token: 0x06002F00 RID: 12032 RVA: 0x001087F4 File Offset: 0x001069F4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_RescueNearby jobGiver_RescueNearby = (JobGiver_RescueNearby)base.DeepCopy(resolve);
			jobGiver_RescueNearby.radius = this.radius;
			return jobGiver_RescueNearby;
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x00108810 File Offset: 0x00106A10
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn3 = (Pawn)t;
				return pawn3.Downed && pawn3.Faction == pawn.Faction && !pawn3.InBed() && pawn.CanReserve(pawn3, 1, -1, null, false) && !pawn3.IsForbidden(pawn) && !GenAI.EnemyIsNear(pawn3, 25f);
			};
			Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.radius, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (pawn2 == null)
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(pawn2, pawn, pawn2.HostFaction == pawn.Faction, false, false);
			if (building_Bed == null || !pawn2.CanReserve(building_Bed, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, building_Bed);
			job.count = 1;
			return job;
		}

		// Token: 0x04001AA5 RID: 6821
		private float radius = 30f;

		// Token: 0x04001AA6 RID: 6822
		private const float MinDistFromEnemy = 25f;
	}
}
