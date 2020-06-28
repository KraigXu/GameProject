using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200052B RID: 1323
	public sealed class Toil : IJobEndable
	{
		// Token: 0x060025E1 RID: 9697 RVA: 0x000E0320 File Offset: 0x000DE520
		public void Cleanup(int myIndex, JobDriver jobDriver)
		{
			if (this.finishActions != null)
			{
				for (int i = 0; i < this.finishActions.Count; i++)
				{
					try
					{
						this.finishActions[i]();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn ",
							this.actor.ToStringSafe<Pawn>(),
							" threw exception while executing toil's finish action (",
							i,
							"), jobDriver=",
							jobDriver.ToStringSafe<JobDriver>(),
							", job=",
							jobDriver.job.ToStringSafe<Job>(),
							", toilIndex=",
							myIndex,
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000E0400 File Offset: 0x000DE600
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000E0408 File Offset: 0x000DE608
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.endConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000E0439 File Offset: 0x000DE639
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000E0447 File Offset: 0x000DE647
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000E0468 File Offset: 0x000DE668
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000E0489 File Offset: 0x000DE689
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}

		// Token: 0x04001708 RID: 5896
		public Pawn actor;

		// Token: 0x04001709 RID: 5897
		public Action initAction;

		// Token: 0x0400170A RID: 5898
		public Action tickAction;

		// Token: 0x0400170B RID: 5899
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x0400170C RID: 5900
		public List<Action> preInitActions;

		// Token: 0x0400170D RID: 5901
		public List<Action> preTickActions;

		// Token: 0x0400170E RID: 5902
		public List<Action> finishActions;

		// Token: 0x0400170F RID: 5903
		public bool atomicWithPrevious;

		// Token: 0x04001710 RID: 5904
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x04001711 RID: 5905
		public Func<SkillDef> activeSkill;

		// Token: 0x04001712 RID: 5906
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x04001713 RID: 5907
		public int defaultDuration;

		// Token: 0x04001714 RID: 5908
		public bool handlingFacing;
	}
}
