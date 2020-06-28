using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200062C RID: 1580
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002B49 RID: 11081
		protected abstract int BaseWorkAmount { get; }

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002B4A RID: 11082
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002B4B RID: 11083 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000FAF0E File Offset: 0x000F910E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed);
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x000FAF34 File Offset: 0x000F9134
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

		// Token: 0x06002B4E RID: 11086
		protected abstract void DoEffect(IntVec3 c);

		// Token: 0x06002B4F RID: 11087 RVA: 0x000FAF44 File Offset: 0x000F9144
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04001996 RID: 6550
		private float workLeft = -1000f;

		// Token: 0x04001997 RID: 6551
		protected bool clearSnow;
	}
}
