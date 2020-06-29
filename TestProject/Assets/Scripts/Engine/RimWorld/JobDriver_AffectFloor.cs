using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		
		
		protected abstract int BaseWorkAmount { get; }

		
		
		protected abstract DesignationDef DesDef { get; }

		
		
		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickAction = delegate
			{
				float num = (this.SpeedStat != null) ? doWork.actor.GetStatValue(this.SpeedStat, true) : 1f;
				num *= 1.7f;
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f, false);
				}
				if (this.clearSnow)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect(this.TargetLocA);
					Designation designation = this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef);
					if (designation != null)
					{
						designation.Delete();
					}
					this.ReadyForNextToil();
					return;
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		
		protected abstract void DoEffect(IntVec3 c);

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		
		private float workLeft = -1000f;

		
		protected bool clearSnow;
	}
}
