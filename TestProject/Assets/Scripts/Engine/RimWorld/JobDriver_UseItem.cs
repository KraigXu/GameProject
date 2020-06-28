using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200067C RID: 1660
	public class JobDriver_UseItem : JobDriver
	{
		// Token: 0x06002D3A RID: 11578 RVA: 0x000FF905 File Offset: 0x000FDB05
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.useDuration, "useDuration", 0, false);
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000FF920 File Offset: 0x000FDB20
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.useDuration = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000FF95C File Offset: 0x000FDB5C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(this.useDuration, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			if (this.job.targetB.IsValid)
			{
				toil.FailOnDespawnedOrNull(TargetIndex.B);
				CompTargetable compTargetable = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTargetable>();
				if (compTargetable != null && compTargetable.Props.nonDownedPawnOnly)
				{
					toil.FailOnDownedOrDead(TargetIndex.B);
				}
			}
			yield return toil;
			Toil use = new Toil();
			use.initAction = delegate
			{
				Pawn actor = use.actor;
				actor.CurJob.targetA.Thing.TryGetComp<CompUsable>().UsedBy(actor);
			};
			use.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return use;
			yield break;
		}

		// Token: 0x04001A1C RID: 6684
		private int useDuration = -1;
	}
}
