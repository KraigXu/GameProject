using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006B1 RID: 1713
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		// Token: 0x06002E4B RID: 11851
		protected abstract Pawn GetFollowee(Pawn pawn);

		// Token: 0x06002E4C RID: 11852
		protected abstract float GetRadius(Pawn pawn);

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002E4D RID: 11853 RVA: 0x001043B2 File Offset: 0x001025B2
		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 140;
			}
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x001043BC File Offset: 0x001025BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn followee = this.GetFollowee(pawn);
			if (followee == null)
			{
				Log.Error(base.GetType() + " has null followee. pawn=" + pawn.ToStringSafe<Pawn>(), false);
				return null;
			}
			if (!followee.Spawned || !pawn.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			float radius = this.GetRadius(pawn);
			if (!JobDriver_FollowClose.FarEnoughAndPossibleToStartJob(pawn, followee, radius))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.FollowClose, followee);
			job.expiryInterval = this.FollowJobExpireInterval;
			job.checkOverrideOnExpire = true;
			job.followRadius = radius;
			return job;
		}
	}
}
