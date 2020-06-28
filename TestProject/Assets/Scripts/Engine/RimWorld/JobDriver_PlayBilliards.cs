using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000646 RID: 1606
	public class JobDriver_PlayBilliards : JobDriver
	{
		// Token: 0x06002BE8 RID: 11240 RVA: 0x000FC497 File Offset: 0x000FA697
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000FC4C8 File Offset: 0x000FA6C8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			Toil chooseCell = Toils_Misc.FindRandomAdjacentReachableCell(TargetIndex.A, TargetIndex.B);
			yield return chooseCell;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				this.job.locomotionUrgency = LocomotionUrgency.Walk;
			};
			toil.tickAction = delegate
			{
				this.pawn.rotationTracker.FaceCell(base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
				if (this.ticksLeftThisToil == 300)
				{
					SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				}
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, (Building)base.TargetThingA);
			};
			toil.handlingFacing = true;
			toil.socialMode = RandomSocialMode.SuperActive;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 600;
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return toil;
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return Toils_Jump.Jump(chooseCell);
			yield break;
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000FC4D8 File Offset: 0x000FA6D8
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		// Token: 0x040019C1 RID: 6593
		private const int ShotDuration = 600;
	}
}
