using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D1 RID: 1745
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		// Token: 0x06002EAE RID: 11950 RVA: 0x001063C4 File Offset: 0x001045C4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x001063E0 File Offset: 0x001045E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = (Thing t) => t.def.hasInteractionCell && t.def.HasComp(typeof(CompMannable)) && pawn.CanReserve(t, 1, -1, null, false) && JobDriver_ManTurret.FindAmmoForTurret(pawn, (Building_TurretGun)t) != null;
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(pawn), pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, thing);
				job.expiryInterval = 2000;
				job.checkOverrideOnExpire = true;
				return job;
			}
			return null;
		}

		// Token: 0x06002EB0 RID: 11952
		protected abstract IntVec3 GetRoot(Pawn pawn);

		// Token: 0x04001A7E RID: 6782
		public float maxDistFromPoint = -1f;
	}
}
