using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000638 RID: 1592
	public class JobDriver_SmoothWall : JobDriver
	{
		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000FB5EC File Offset: 0x000F97EC
		protected int BaseWorkAmount
		{
			get
			{
				return 6500;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002B9B RID: 11163 RVA: 0x000FB5F3 File Offset: 0x000F97F3
		protected DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothWall;
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000FB5FC File Offset: 0x000F97FC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000FB657 File Offset: 0x000F9857
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickAction = delegate
			{
				float num = doWork.actor.GetStatValue(StatDefOf.SmoothingSpeed, true) * 1.7f;
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f, false);
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
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

		// Token: 0x06002B9E RID: 11166 RVA: 0x000FB668 File Offset: 0x000F9868
		protected void DoEffect()
		{
			SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, this.pawn), this.pawn);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000FB699 File Offset: 0x000F9899
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x040019A9 RID: 6569
		private float workLeft = -1000f;
	}
}
