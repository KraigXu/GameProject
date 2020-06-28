using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000518 RID: 1304
	public class JobDriver_FollowClose : JobDriver
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x000DD65C File Offset: 0x000DB85C
		private Pawn Followee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x000DD682 File Offset: 0x000DB882
		private bool CurrentlyWalkingToFollowee
		{
			get
			{
				return this.pawn.pather.Moving && this.pawn.pather.Destination == this.Followee;
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000DD6B8 File Offset: 0x000DB8B8
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.job.followRadius <= 0f)
			{
				Log.Error("Follow radius is <= 0. pawn=" + this.pawn.ToStringSafe<Pawn>(), false);
				this.job.followRadius = 10f;
			}
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000DD708 File Offset: 0x000DB908
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate
				{
					Pawn followee = this.Followee;
					float followRadius = this.job.followRadius;
					if (!this.pawn.pather.Moving || this.pawn.IsHashIntervalTick(30))
					{
						bool flag = false;
						if (this.CurrentlyWalkingToFollowee)
						{
							if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, followRadius))
							{
								flag = true;
							}
						}
						else
						{
							float radius = followRadius * 1.2f;
							if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, radius))
							{
								flag = true;
							}
							else
							{
								if (!this.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									base.EndJobWith(JobCondition.Incompletable);
									return;
								}
								this.pawn.pather.StartPath(followee, PathEndMode.Touch);
								this.locomotionUrgencySameAs = null;
							}
						}
						if (flag)
						{
							if (JobDriver_FollowClose.NearDestinationOrNotMoving(this.pawn, followee, followRadius))
							{
								base.EndJobWith(JobCondition.Succeeded);
								return;
							}
							IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
							if (!this.pawn.pather.Moving || this.pawn.pather.Destination.HasThing || !this.pawn.pather.Destination.Cell.InHorDistOf(lastPassableCellInPath, followRadius))
							{
								IntVec3 intVec = CellFinder.RandomClosewalkCellNear(lastPassableCellInPath, base.Map, Mathf.FloorToInt(followRadius), null);
								if (intVec == this.pawn.Position)
								{
									base.EndJobWith(JobCondition.Succeeded);
									return;
								}
								if (intVec.IsValid && this.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
									this.locomotionUrgencySameAs = followee;
									return;
								}
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000DD564 File Offset: 0x000DB764
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000DD718 File Offset: 0x000DB918
		public static bool FarEnoughAndPossibleToStartJob(Pawn follower, Pawn followee, float radius)
		{
			if (radius <= 0f)
			{
				string text = "Checking follow job with radius <= 0. pawn=" + follower.ToStringSafe<Pawn>();
				if (follower.mindState != null && follower.mindState.duty != null)
				{
					text = text + " duty=" + follower.mindState.duty.def;
				}
				Log.ErrorOnce(text, follower.thingIDNumber ^ 843254009, false);
				return false;
			}
			if (!follower.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return false;
			}
			float radius2 = radius * 1.2f;
			return !JobDriver_FollowClose.NearFollowee(follower, followee, radius2) || (!JobDriver_FollowClose.NearDestinationOrNotMoving(follower, followee, radius2) && follower.CanReach(followee.pather.LastPassableCellInPath, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn));
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000DD7D4 File Offset: 0x000DB9D4
		private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
		{
			return follower.Position.AdjacentTo8WayOrInside(followee.Position) || (follower.Position.InHorDistOf(followee.Position, radius) && GenSight.LineOfSight(follower.Position, followee.Position, follower.Map, false, null, 0, 0));
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000DD82C File Offset: 0x000DBA2C
		private static bool NearDestinationOrNotMoving(Pawn follower, Pawn followee, float radius)
		{
			if (!followee.pather.Moving)
			{
				return true;
			}
			IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
			return !lastPassableCellInPath.IsValid || follower.Position.AdjacentTo8WayOrInside(lastPassableCellInPath) || follower.Position.InHorDistOf(lastPassableCellInPath, radius);
		}

		// Token: 0x040016DA RID: 5850
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x040016DB RID: 5851
		private const int CheckPathIntervalTicks = 30;
	}
}
