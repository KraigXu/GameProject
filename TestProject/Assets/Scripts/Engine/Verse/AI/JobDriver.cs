﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	
	public abstract class JobDriver : IExposable, IJobEndable
	{
		
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

		
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000DF592 File Offset: 0x000DD792
		protected bool HaveCurToil
		{
			get
			{
				return this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count && this.job != null && this.pawn.CurJob == this.job;
			}
		}

		
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000DF5D0 File Offset: 0x000DD7D0
		private bool CanStartNextToilInBusyStance
		{
			get
			{
				int num = this.curToilIndex + 1;
				return num < this.toils.Count && this.toils[num].atomicWithPrevious;
			}
		}

		
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x000DF607 File Offset: 0x000DD807
		public int CurToilIndex
		{
			get
			{
				return this.curToilIndex;
			}
		}

		
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000DF60F File Offset: 0x000DD80F
		public bool OnLastToil
		{
			get
			{
				return this.CurToilIndex == this.toils.Count - 1;
			}
		}

		
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

		
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000DF64F File Offset: 0x000DD84F
		public bool HandlingFacing
		{
			get
			{
				return this.CurToil != null && this.CurToil.handlingFacing;
			}
		}

		
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x000DF666 File Offset: 0x000DD866
		protected LocalTargetInfo TargetA
		{
			get
			{
				return this.job.targetA;
			}
		}

		
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000DF673 File Offset: 0x000DD873
		protected LocalTargetInfo TargetB
		{
			get
			{
				return this.job.targetB;
			}
		}

		
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000DF680 File Offset: 0x000DD880
		protected LocalTargetInfo TargetC
		{
			get
			{
				return this.job.targetC;
			}
		}

		
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

		
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000DF6D7 File Offset: 0x000DD8D7
		protected IntVec3 TargetLocA
		{
			get
			{
				return this.job.targetA.Cell;
			}
		}

		
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x000DF6E9 File Offset: 0x000DD8E9
		protected Map Map
		{
			get
			{
				return this.pawn.Map;
			}
		}

		
		public virtual string GetReport()
		{
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		
		protected virtual string ReportStringProcessed(string str)
		{
			LocalTargetInfo a = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			LocalTargetInfo b = this.job.targetB.IsValid ? this.job.targetB : this.job.targetQueueB.FirstValid();
			LocalTargetInfo targetC = this.job.targetC;
			return JobUtility.GetResolvedJobReport(str, a, b, targetC);
		}

		
		public abstract bool TryMakePreToilReservations(bool errorOnFailed);

		
		protected abstract IEnumerable<Toil> MakeNewToils();

		
		public virtual void SetInitialPosture()
		{
			this.pawn.jobs.posture = PawnPosture.Standing;
		}

		
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

		
		public virtual bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		
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
						//JobDriver.c__DisplayClass57_0 c__DisplayClass57_;
						//c__DisplayClass57_.startingJob = this.pawn.CurJob;
						//c__DisplayClass57_.startingJobId = c__DisplayClass57_.startingJob.loadID;
						//if (this.CurToil.preTickActions != null)
						//{
						//	Toil curToil = this.CurToil;
						//	for (int i = 0; i < curToil.preTickActions.Count; i++)
						//	{
						//		curToil.preTickActions[i]();
						//		if (this.<DriverTick>g__JobChanged|57_0(ref c__DisplayClass57_))
						//		{
						//			return;
						//		}
						//		if (this.CurToil != curToil || this.wantBeginNextToil)
						//		{
						//			return;
						//		}
						//	}
						//}
						//if (this.CurToil.tickAction != null)
						//{
						//	this.CurToil.tickAction();
						//	if (this.<DriverTick>g__JobChanged|57_0(ref c__DisplayClass57_))
						//	{
						//		return;
						//	}
						//}
						//if (this.job.mote != null)
						//{
						//	this.job.mote.Maintain();
						//}
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		
		public void ReadyForNextToil()
		{
			this.wantBeginNextToil = true;
			this.TryActuallyStartNextToil();
		}

		
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

		
		public void EndJobWith(JobCondition condition)
		{
			if (!this.pawn.Destroyed && this.job != null && this.pawn.CurJob == this.job)
			{
				this.pawn.jobs.EndCurrentJob(condition, true, true);
			}
		}

		
		public virtual object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn
			};
		}

		
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

		
		public void JumpToToil(Toil to)
		{
			if (to == null)
			{
				Log.Warning("JumpToToil with null toil. pawn=" + this.pawn.ToStringSafe<Pawn>() + ", job=" + this.pawn.CurJob.ToStringSafe<Job>(), false);
			}
			this.SetNextToil(to);
			this.ReadyForNextToil();
		}

		
		public virtual void Notify_Starting()
		{
			this.startTick = Find.TickManager.TicksGame;
		}

		
		public virtual void Notify_PatherArrived()
		{
			if (this.curToilCompleteMode == ToilCompleteMode.PatherArrival)
			{
				this.ReadyForNextToil();
			}
		}

		
		public virtual void Notify_PatherFailed()
		{
			this.EndJobWith(JobCondition.ErroredPather);
		}

		
		public virtual void Notify_StanceChanged()
		{
		}

		
		public virtual void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		
		public Pawn GetActor()
		{
			return this.pawn;
		}

		
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.globalFailConditions.Add(newEndCondition);
		}

		
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

		
		public void AddFinishAction(Action newAct)
		{
			this.globalFinishActions.Add(newAct);
		}

		
		public virtual bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			return false;
		}

		
		public virtual RandomSocialMode DesiredSocialMode()
		{
			if (this.CurToil != null)
			{
				return this.CurToil.socialMode;
			}
			return RandomSocialMode.Normal;
		}

		
		public virtual bool IsContinuation(Job j)
		{
			return true;
		}

		
		public Pawn pawn;

		
		public Job job;

		
		private List<Toil> toils = new List<Toil>();

		
		public List<Func<JobCondition>> globalFailConditions = new List<Func<JobCondition>>();

		
		public List<Action> globalFinishActions = new List<Action>();

		
		public bool ended;

		
		private int curToilIndex = -1;

		
		private ToilCompleteMode curToilCompleteMode;

		
		public int ticksLeftThisToil = 99999;

		
		private bool wantBeginNextToil;

		
		protected int startTick = -1;

		
		public TargetIndex rotateToFace = TargetIndex.A;

		
		private int nextToilIndex = -1;

		
		public bool asleep;

		
		public float uninstallWorkLeft;

		
		public bool collideWithPawns;

		
		public Pawn locomotionUrgencySameAs;

		
		public int debugTicksSpentThisToil;
	}
}
