using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000635 RID: 1589
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000DF68D File Offset: 0x000DD88D
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002B83 RID: 11139 RVA: 0x000FB46A File Offset: 0x000F966A
		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002B84 RID: 11140
		protected abstract DesignationDef Designation { get; }

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002B85 RID: 11141
		protected abstract float TotalNeededWork { get; }

		// Token: 0x06002B86 RID: 11142 RVA: 0x000FB47C File Offset: 0x000F967C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000FB4B0 File Offset: 0x000F96B0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000FB4D2 File Offset: 0x000F96D2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, (this.Target is Building_Trap) ? PathEndMode.OnCell : PathEndMode.Touch);
			Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate
			{
				this.totalNeededWork = this.TotalNeededWork;
				this.workLeft = this.totalNeededWork;
			};
			doWork.tickAction = delegate
			{
				this.workLeft -= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				this.TickAction();
				if (this.workLeft <= 0f)
				{
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield return new Toil
			{
				initAction = delegate
				{
					this.FinishedRemoving();
					base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void FinishedRemoving()
		{
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void TickAction()
		{
		}

		// Token: 0x040019A3 RID: 6563
		private float workLeft;

		// Token: 0x040019A4 RID: 6564
		private float totalNeededWork;
	}
}
