using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000528 RID: 1320
	public abstract class JobDriver : IExposable, IJobEndable
	{
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x000DF4D0 File Offset: 0x000DD6D0
		protected Toil CurToil
		{
			get
			{
				if (this.curToilIndex < 0 || this.job == null || this.pawn.CurJob != this.job)
				{
					return null;
				}
				if (this.curToilIndex >= this.toils.Count)
				{
					Log.Error(string.Concat(new object[]
					{
						this.pawn,
						" with job ",
						this.pawn.CurJob,
						" tried to get CurToil with curToilIndex=",
						this.curToilIndex,
						" but only has ",
						this.toils.Count,
						" toils."
					}), false);
					return null;
				}
				return this.toils[this.curToilIndex];
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000DF592 File Offset: 0x000DD792
		protected bool HaveCurToil
		{
			get
			{
				return this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count && this.job != null && this.pawn.CurJob == this.job;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000DF5D0 File Offset: 0x000DD7D0
		private bool CanStartNextToilInBusyStance
		{
			get
			{
				int num = this.curToilIndex + 1;
				return num < this.toils.Count && this.toils[num].atomicWithPrevious;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x000DF607 File Offset: 0x000DD807
		public int CurToilIndex
		{
			get
			{
				return this.curToilIndex;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000DF60F File Offset: 0x000DD80F
		public bool OnLastToil
		{
			get
			{
				return this.CurToilIndex == this.toils.Count - 1;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x000DF626 File Offset: 0x000DD826
		public SkillDef ActiveSkill
		{
			get
			{
				if (!this.HaveCurToil || this.CurToil.activeSkill == null)
				{
					return null;
				}
				return this.CurToil.activeSkill();
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000DF64F File Offset: 0x000DD84F
		public bool HandlingFacing
		{
			get
			{
				return this.CurToil != null && this.CurToil.handlingFacing;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x000DF666 File Offset: 0x000DD866
		protected LocalTargetInfo TargetA
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000DF673 File Offset: 0x000DD873
		protected LocalTargetInfo TargetB
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000DF680 File Offset: 0x000DD880
		protected LocalTargetInfo TargetC
		{
			get
			{
				return this.job.targetC;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000DF68D File Offset: 0x000DD88D
		// (set) Token: 0x060025BB RID: 9659 RVA: 0x000DF69F File Offset: 0x000DD89F
		protected Thing TargetThingA
		{
			get
			{
				return this.job.targetA.Thing;
			}
			set
			{
				this.job.targetA = value;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x000DF6B2 File Offset: 0x000DD8B2
		// (set) Token: 0x060025BD RID: 9661 RVA: 0x000DF6C4 File Offset: 0x000DD8C4
		protected Thing TargetThingB
		{
			get
			{
				return this.job.targetB.Thing;
			}
			set
			{
				this.job.targetB = value;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000DF6D7 File Offset: 0x000DD8D7
		protected IntVec3 TargetLocA
		{
			get
			{
				return this.job.targetA.Cell;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x000DF6E9 File Offset: 0x000DD8E9
		protected Map Map
		{
			get
			{
				return this.pawn.Map;
			}
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000DF6F6 File Offset: 0x000DD8F6
		public virtual string GetReport()
		{
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000DF710 File Offset: 0x000DD910
		protected virtual string ReportStringProcessed(string str)
		{
			LocalTargetInfo a = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			LocalTargetInfo b = this.job.targetB.IsValid ? this.job.targetB : this.job.targetQueueB.FirstValid();
			LocalTargetInfo targetC = this.job.targetC;
			return JobUtility.GetResolvedJobReport(str, a, b, targetC);
		}

		// Token: 0x060025C2 RID: 9666
		public abstract bool TryMakePreToilReservations(bool errorOnFailed);

		// Token: 0x060025C3 RID: 9667
		protected abstract IEnumerable<Toil> MakeNewToils();

		// Token: 0x060025C4 RID: 9668 RVA: 0x000DF792 File Offset: 0x000DD992
		public virtual void SetInitialPosture()
		{
			this.pawn.jobs.posture = PawnPosture.Standing;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000DF7A8 File Offset: 0x000DD9A8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<int>(ref this.curToilIndex, "curToilIndex", 0, true);
			Scribe_Values.Look<int>(ref this.ticksLeftThisToil, "ticksLeftThisToil", 0, false);
			Scribe_Values.Look<bool>(ref this.wantBeginNextToil, "wantBeginNextToil", false, false);
			Scribe_Values.Look<ToilCompleteMode>(ref this.curToilCompleteMode, "curToilCompleteMode", ToilCompleteMode.Undefined, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<TargetIndex>(ref this.rotateToFace, "rotateToFace", TargetIndex.A, false);
			Scribe_Values.Look<bool>(ref this.asleep, "asleep", false, false);
			Scribe_Values.Look<float>(ref this.uninstallWorkLeft, "uninstallWorkLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.nextToilIndex, "nextToilIndex", -1, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_References.Look<Pawn>(ref this.locomotionUrgencySameAs, "locomotionUrgencySameAs", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupToils();
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000DF8A0 File Offset: 0x000DDAA0
		public void Cleanup(JobCondition condition)
		{
			for (int i = 0; i < this.globalFinishActions.Count; i++)
			{
				try
				{
					this.globalFinishActions[i]();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn ",
						this.pawn.ToStringSafe<Pawn>(),
						" threw exception while executing a global finish action (",
						i,
						"), jobDriver=",
						this.ToStringSafe<JobDriver>(),
						", job=",
						this.job.ToStringSafe<Job>(),
						": ",
						ex
					}), false);
				}
			}
			if (this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count)
			{
				this.toils[this.curToilIndex].Cleanup(this.curToilIndex, this);
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000DF998 File Offset: 0x000DDB98
		internal void SetupToils()
		{
			try
			{
				this.toils.Clear();
				foreach (Toil toil in this.MakeNewToils())
				{
					if (toil.defaultCompleteMode == ToilCompleteMode.Undefined)
					{
						Log.Error("Toil has undefined complete mode.", false);
						toil.defaultCompleteMode = ToilCompleteMode.Instant;
					}
					toil.actor = this.pawn;
					this.toils.Add(toil);
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in SetupToils for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000DFA50 File Offset: 0x000DDC50
		public void DriverTick()
		{
			try
			{
				this.ticksLeftThisToil--;
				this.debugTicksSpentThisToil++;
				if (this.CurToil == null)
				{
					if (!this.pawn.stances.FullBodyBusy || this.CanStartNextToilInBusyStance)
					{
						this.ReadyForNextToil();
					}
				}
				else if (!this.CheckCurrentToilEndOrFail())
				{
					if (this.curToilCompleteMode == ToilCompleteMode.Delay)
					{
						if (this.ticksLeftThisToil <= 0)
						{
							this.ReadyForNextToil();
							return;
						}
					}
					else if (this.curToilCompleteMode == ToilCompleteMode.FinishedBusy && !this.pawn.stances.FullBodyBusy)
					{
						this.ReadyForNextToil();
						return;
					}
					if (this.wantBeginNextToil)
					{
						this.TryActuallyStartNextToil();
					}
					else if (this.curToilCompleteMode == ToilCompleteMode.Instant && this.debugTicksSpentThisToil > 300)
					{
						Log.Error(string.Concat(new object[]
						{
							this.pawn,
							" had to be broken from frozen state. He was doing job ",
							this.job,
							", toilindex=",
							this.curToilIndex
						}), false);
						this.ReadyForNextToil();
					}
					else
					{
						JobDriver.<>c__DisplayClass57_0 <>c__DisplayClass57_;
						<>c__DisplayClass57_.startingJob = this.pawn.CurJob;
						<>c__DisplayClass57_.startingJobId = <>c__DisplayClass57_.startingJob.loadID;
						if (this.CurToil.preTickActions != null)
						{
							Toil curToil = this.CurToil;
							for (int i = 0; i < curToil.preTickActions.Count; i++)
							{
								curToil.preTickActions[i]();
								if (this.<DriverTick>g__JobChanged|57_0(ref <>c__DisplayClass57_))
								{
									return;
								}
								if (this.CurToil != curToil || this.wantBeginNextToil)
								{
									return;
								}
							}
						}
						if (this.CurToil.tickAction != null)
						{
							this.CurToil.tickAction();
							if (this.<DriverTick>g__JobChanged|57_0(ref <>c__DisplayClass57_))
							{
								return;
							}
						}
						if (this.job.mote != null)
						{
							this.job.mote.Maintain();
						}
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000DFC78 File Offset: 0x000DDE78
		public void ReadyForNextToil()
		{
			this.wantBeginNextToil = true;
			this.TryActuallyStartNextToil();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000DFC88 File Offset: 0x000DDE88
		private void TryActuallyStartNextToil()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.stances.FullBodyBusy && !this.CanStartNextToilInBusyStance)
			{
				return;
			}
			if (this.job == null || this.pawn.CurJob != this.job)
			{
				return;
			}
			if (this.HaveCurToil)
			{
				this.CurToil.Cleanup(this.curToilIndex, this);
			}
			if (this.nextToilIndex >= 0)
			{
				this.curToilIndex = this.nextToilIndex;
				this.nextToilIndex = -1;
			}
			else
			{
				this.curToilIndex++;
			}
			this.wantBeginNextToil = false;
			if (!this.HaveCurToil)
			{
				if (this.pawn.stances != null && this.pawn.stances.curStance.StanceBusy)
				{
					Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " ended job " + this.job.ToStringSafe<Job>() + " due to running out of toils during a busy stance.", 6453432, false);
				}
				this.EndJobWith(JobCondition.Succeeded);
				return;
			}
			this.debugTicksSpentThisToil = 0;
			this.ticksLeftThisToil = this.CurToil.defaultDuration;
			this.curToilCompleteMode = this.CurToil.defaultCompleteMode;
			if (!this.CheckCurrentToilEndOrFail())
			{
				Toil curToil = this.CurToil;
				if (this.CurToil.preInitActions != null)
				{
					for (int i = 0; i < this.CurToil.preInitActions.Count; i++)
					{
						try
						{
							this.CurToil.preInitActions[i]();
						}
						catch (Exception exception)
						{
							JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new object[]
							{
								"JobDriver threw exception in preInitActions[",
								i,
								"] for pawn ",
								this.pawn.ToStringSafe<Pawn>()
							}), exception, this);
							return;
						}
						if (this.CurToil != curToil)
						{
							break;
						}
					}
				}
				if (this.CurToil == curToil)
				{
					if (this.CurToil.initAction != null)
					{
						try
						{
							this.CurToil.initAction();
						}
						catch (Exception exception2)
						{
							JobUtility.TryStartErrorRecoverJob(this.pawn, "JobDriver threw exception in initAction for pawn " + this.pawn.ToStringSafe<Pawn>(), exception2, this);
							return;
						}
					}
					if (!this.ended && this.curToilCompleteMode == ToilCompleteMode.Instant && this.CurToil == curToil)
					{
						this.ReadyForNextToil();
					}
				}
			}
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000DFEE4 File Offset: 0x000DE0E4
		public void EndJobWith(JobCondition condition)
		{
			if (!this.pawn.Destroyed && this.job != null && this.pawn.CurJob == this.job)
			{
				this.pawn.jobs.EndCurrentJob(condition, true, true);
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000DFF21 File Offset: 0x000DE121
		public virtual object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn
			};
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000DFF34 File Offset: 0x000DE134
		private bool CheckCurrentToilEndOrFail()
		{
			bool result;
			try
			{
				Toil curToil = this.CurToil;
				if (this.globalFailConditions != null)
				{
					for (int i = 0; i < this.globalFailConditions.Count; i++)
					{
						JobCondition jobCondition = this.globalFailConditions[i]();
						if (jobCondition != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new object[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of globalFailConditions[",
									i,
									"]"
								}));
							}
							this.EndJobWith(jobCondition);
							return true;
						}
					}
				}
				if (curToil != null && curToil.endConditions != null)
				{
					for (int j = 0; j < curToil.endConditions.Count; j++)
					{
						JobCondition jobCondition2 = curToil.endConditions[j]();
						if (jobCondition2 != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new object[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of toils[",
									this.curToilIndex,
									"].endConditions[",
									j,
									"]"
								}));
							}
							this.EndJobWith(jobCondition2);
							return true;
						}
					}
				}
				result = false;
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in CheckCurrentToilEndOrFail for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
				result = true;
			}
			return result;
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x000E0124 File Offset: 0x000DE324
		private void SetNextToil(Toil to)
		{
			if (to != null && !this.toils.Contains(to))
			{
				Log.Warning(string.Concat(new string[]
				{
					"SetNextToil with non-existent toil (",
					to.ToStringSafe<Toil>(),
					"). pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					", job=",
					this.pawn.CurJob.ToStringSafe<Job>()
				}), false);
			}
			this.nextToilIndex = this.toils.IndexOf(to);
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000E01A8 File Offset: 0x000DE3A8
		public void JumpToToil(Toil to)
		{
			if (to == null)
			{
				Log.Warning("JumpToToil with null toil. pawn=" + this.pawn.ToStringSafe<Pawn>() + ", job=" + this.pawn.CurJob.ToStringSafe<Job>(), false);
			}
			this.SetNextToil(to);
			this.ReadyForNextToil();
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000E01F5 File Offset: 0x000DE3F5
		public virtual void Notify_Starting()
		{
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000E0207 File Offset: 0x000DE407
		public virtual void Notify_PatherArrived()
		{
			if (this.curToilCompleteMode == ToilCompleteMode.PatherArrival)
			{
				this.ReadyForNextToil();
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000E0218 File Offset: 0x000DE418
		public virtual void Notify_PatherFailed()
		{
			this.EndJobWith(JobCondition.ErroredPather);
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_StanceChanged()
		{
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000E0221 File Offset: 0x000DE421
		public Pawn GetActor()
		{
			return this.pawn;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000E0229 File Offset: 0x000DE429
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.globalFailConditions.Add(newEndCondition);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000E0238 File Offset: 0x000DE438
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.globalFailConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000E0269 File Offset: 0x000DE469
		public void AddFinishAction(Action newAct)
		{
			this.globalFinishActions.Add(newAct);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			return false;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000E0277 File Offset: 0x000DE477
		public virtual RandomSocialMode DesiredSocialMode()
		{
			if (this.CurToil != null)
			{
				return this.CurToil.socialMode;
			}
			return RandomSocialMode.Normal;
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsContinuation(Job j)
		{
			return true;
		}

		// Token: 0x040016EF RID: 5871
		public Pawn pawn;

		// Token: 0x040016F0 RID: 5872
		public Job job;

		// Token: 0x040016F1 RID: 5873
		private List<Toil> toils = new List<Toil>();

		// Token: 0x040016F2 RID: 5874
		public List<Func<JobCondition>> globalFailConditions = new List<Func<JobCondition>>();

		// Token: 0x040016F3 RID: 5875
		public List<Action> globalFinishActions = new List<Action>();

		// Token: 0x040016F4 RID: 5876
		public bool ended;

		// Token: 0x040016F5 RID: 5877
		private int curToilIndex = -1;

		// Token: 0x040016F6 RID: 5878
		private ToilCompleteMode curToilCompleteMode;

		// Token: 0x040016F7 RID: 5879
		public int ticksLeftThisToil = 99999;

		// Token: 0x040016F8 RID: 5880
		private bool wantBeginNextToil;

		// Token: 0x040016F9 RID: 5881
		protected int startTick = -1;

		// Token: 0x040016FA RID: 5882
		public TargetIndex rotateToFace = TargetIndex.A;

		// Token: 0x040016FB RID: 5883
		private int nextToilIndex = -1;

		// Token: 0x040016FC RID: 5884
		public bool asleep;

		// Token: 0x040016FD RID: 5885
		public float uninstallWorkLeft;

		// Token: 0x040016FE RID: 5886
		public bool collideWithPawns;

		// Token: 0x040016FF RID: 5887
		public Pawn locomotionUrgencySameAs;

		// Token: 0x04001700 RID: 5888
		public int debugTicksSpentThisToil;
	}
}
