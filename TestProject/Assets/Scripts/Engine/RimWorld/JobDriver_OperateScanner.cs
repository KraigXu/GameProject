using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200066E RID: 1646
	public class JobDriver_OperateScanner : JobDriver
	{
		// Token: 0x06002CDC RID: 11484 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000FE961 File Offset: 0x000FCB61
		protected override IEnumerable<Toil> MakeNewToils()
		{
			CompScanner scannerComp = this.job.targetA.Thing.TryGetComp<CompScanner>();
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(() => !scannerComp.CanUseNow);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil work = new Toil();
			work.tickAction = delegate
			{
				Pawn actor = work.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				scannerComp.Used(actor);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.035f, false);
				actor.GainComfortFromCellIfPossible(true);
			};
			work.AddFailCondition(() => !scannerComp.CanUseNow);
			work.defaultCompleteMode = ToilCompleteMode.Never;
			work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			work.activeSkill = (() => SkillDefOf.Intellectual);
			yield return work;
			yield break;
		}
	}
}
