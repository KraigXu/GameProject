using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000651 RID: 1617
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		// Token: 0x06002C25 RID: 11301 RVA: 0x000DE503 File Offset: 0x000DC703
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000FCE1C File Offset: 0x000FB01C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(this.job.def.joyDuration, TargetIndex.None);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.tickAction = delegate
			{
				this.WaitTickAction();
			};
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return toil;
			yield break;
		}

		// Token: 0x06002C27 RID: 11303
		protected abstract void WaitTickAction();

		// Token: 0x040019C9 RID: 6601
		protected const TargetIndex TargetThingIndex = TargetIndex.A;
	}
}
