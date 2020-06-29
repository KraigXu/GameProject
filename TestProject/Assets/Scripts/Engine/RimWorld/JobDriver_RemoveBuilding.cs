using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000DF68D File Offset: 0x000DD88D
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		
		// (get) Token: 0x06002B83 RID: 11139 RVA: 0x000FB46A File Offset: 0x000F966A
		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		
		// (get) Token: 0x06002B84 RID: 11140
		protected abstract DesignationDef Designation { get; }

		
		// (get) Token: 0x06002B85 RID: 11141
		protected abstract float TotalNeededWork { get; }

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		
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

		
		protected virtual void FinishedRemoving()
		{
		}

		
		protected virtual void TickAction()
		{
		}

		
		private float workLeft;

		
		private float totalNeededWork;
	}
}
