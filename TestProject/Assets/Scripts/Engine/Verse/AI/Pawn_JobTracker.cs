﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
	
	public class Pawn_JobTracker : IExposable
	{
		
		// (get) Token: 0x060027D3 RID: 10195 RVA: 0x000E9E84 File Offset: 0x000E8084
		public bool HandlingFacing
		{
			get
			{
				return this.curDriver != null && this.curDriver.HandlingFacing;
			}
		}

		
		public Pawn_JobTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		
		public virtual void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.curJob, "curJob", Array.Empty<object>());
			Scribe_Deep.Look<JobDriver>(ref this.curDriver, "curDriver", Array.Empty<object>());
			Scribe_Deep.Look<JobQueue>(ref this.jobQueue, "jobQueue", Array.Empty<object>());
			Scribe_Values.Look<PawnPosture>(ref this.posture, "posture", PawnPosture.Standing, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.curDriver != null)
				{
					this.curDriver.pawn = this.pawn;
					this.curDriver.job = this.curJob;
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curDriver == null && this.curJob != null)
			{
				Log.Warning(string.Format("Cleaning up invalid job state on {0}", this.pawn), false);
				this.EndCurrentJob(JobCondition.Errored, true, true);
			}
		}

		
		public void Notify_WorkTypeDisabled(WorkTypeDef wType)
		{
			bool flag = this.pawn.WorkTypeIsDisabled(wType);
			try
			{
				if (this.curJob != null && this.curJob.workGiverDef != null && this.curJob.workGiverDef.workType == wType && (flag || !this.curJob.playerForced))
				{
					Pawn_JobTracker.tmpJobsToDequeue.Add(this.curJob);
				}
				foreach (QueuedJob queuedJob in this.jobQueue)
				{
					if (queuedJob.job.workGiverDef != null && queuedJob.job.workGiverDef.workType == wType && (flag || !queuedJob.job.playerForced))
					{
						Pawn_JobTracker.tmpJobsToDequeue.Add(queuedJob.job);
					}
				}
				foreach (Job job in Pawn_JobTracker.tmpJobsToDequeue)
				{
					this.EndCurrentOrQueuedJob(job, JobCondition.InterruptForced, true);
				}
			}
			finally
			{
				Pawn_JobTracker.tmpJobsToDequeue.Clear();
			}
		}

		
		public void Notify_JoyKindDisabled(JoyKindDef joyKind)
		{
			try
			{
				if (this.curJob != null && this.curJob.def.joyKind == joyKind)
				{
					Pawn_JobTracker.tmpJobsToDequeue.Add(this.curJob);
				}
				foreach (QueuedJob queuedJob in this.jobQueue)
				{
					if (queuedJob.job.def.joyKind == joyKind)
					{
						Pawn_JobTracker.tmpJobsToDequeue.Add(queuedJob.job);
					}
				}
				foreach (Job job in Pawn_JobTracker.tmpJobsToDequeue)
				{
					this.EndCurrentOrQueuedJob(job, JobCondition.InterruptForced, true);
				}
			}
			finally
			{
				Pawn_JobTracker.tmpJobsToDequeue.Clear();
			}
		}

		
		public virtual void JobTrackerTick()
		{
			this.jobsGivenThisTick = 0;
			this.jobsGivenThisTickTextual = "";
			if (this.pawn.IsHashIntervalTick(30))
			{
				ThinkResult thinkResult = this.DetermineNextConstantThinkTreeJob();
				if (thinkResult.IsValid)
				{
					if (this.ShouldStartJobFromThinkTree(thinkResult))
					{
						this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
						this.StartJob(thinkResult.Job, JobCondition.InterruptForced, thinkResult.SourceNode, false, false, this.pawn.thinker.ConstantThinkTree, thinkResult.Tag, false, false);
					}
					else if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
					{
						JobMaker.ReturnToPool(thinkResult.Job);
					}
				}
			}
			if (this.curDriver != null)
			{
				if (this.curJob.expiryInterval > 0 && (Find.TickManager.TicksGame - this.curJob.startTick) % this.curJob.expiryInterval == 0 && Find.TickManager.TicksGame != this.curJob.startTick)
				{
					if (!this.curJob.expireRequiresEnemiesNearby || PawnUtility.EnemiesAreNearby(this.pawn, 25, false))
					{
						if (this.debugLog)
						{
							this.DebugLogEvent("Job expire");
						}
						if (!this.curJob.checkOverrideOnExpire)
						{
							this.EndCurrentJob(JobCondition.Succeeded, true, true);
						}
						else
						{
							this.CheckForJobOverride();
						}
						this.FinalizeTick();
						return;
					}
					if (this.debugLog)
					{
						this.DebugLogEvent("Job expire skipped because there are no enemies nearby");
					}
				}
				this.curDriver.DriverTick();
			}
			if (this.curJob == null && !this.pawn.Dead && this.pawn.mindState.Active && this.CanDoAnyJob())
			{
				if (this.debugLog)
				{
					this.DebugLogEvent("Starting job from Tick because curJob == null.");
				}
				this.TryFindAndStartJob();
			}
			this.FinalizeTick();
		}

		
		private void FinalizeTick()
		{
			this.jobsGivenRecentTicks.Add(this.jobsGivenThisTick);
			this.jobsGivenRecentTicksTextual.Add(this.jobsGivenThisTickTextual);
			while (this.jobsGivenRecentTicks.Count > 10)
			{
				this.jobsGivenRecentTicks.RemoveAt(0);
				this.jobsGivenRecentTicksTextual.RemoveAt(0);
			}
			if (this.jobsGivenThisTick != 0)
			{
				int num = 0;
				for (int i = 0; i < this.jobsGivenRecentTicks.Count; i++)
				{
					num += this.jobsGivenRecentTicks[i];
				}
				if (num >= 10)
				{
					string text = this.jobsGivenRecentTicksTextual.ToCommaList(false);
					this.jobsGivenRecentTicks.Clear();
					this.jobsGivenRecentTicksTextual.Clear();
					JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new object[]
					{
						this.pawn.ToStringSafe<Pawn>(),
						" started ",
						10,
						" jobs in ",
						10,
						" ticks. List: ",
						text
					}), null, null);
				}
			}
		}

		
		public void StartJob(Job newJob, JobCondition lastJobEndCondition = JobCondition.None, ThinkNode jobGiver = null, bool resumeCurJobAfterwards = false, bool cancelBusyStances = true, ThinkTreeDef thinkTree = null, JobTag? tag = null, bool fromQueue = false, bool canReturnCurJobToPool = false)
		{
			this.startingNewJob = true;
			try
			{
				if (!fromQueue && (!Find.TickManager.Paused || this.lastJobGivenAtFrame == RealTime.frameCount))
				{
					this.jobsGivenThisTick++;
					if (Prefs.DevMode)
					{
						this.jobsGivenThisTickTextual = this.jobsGivenThisTickTextual + "(" + newJob.ToString() + ") ";
					}
				}
				this.lastJobGivenAtFrame = RealTime.frameCount;
				if (this.jobsGivenThisTick > 10)
				{
					string text = this.jobsGivenThisTickTextual;
					this.jobsGivenThisTick = 0;
					this.jobsGivenThisTickTextual = "";
					this.startingNewJob = false;
					this.pawn.ClearReservationsForJob(newJob);
					JobUtility.TryStartErrorRecoverJob(this.pawn, string.Concat(new string[]
					{
						this.pawn.ToStringSafe<Pawn>(),
						" started 10 jobs in one tick. newJob=",
						newJob.ToStringSafe<Job>(),
						" jobGiver=",
						jobGiver.ToStringSafe<ThinkNode>(),
						" jobList=",
						text
					}), null, null);
				}
				else
				{
					if (this.debugLog)
					{
						this.DebugLogEvent(string.Concat(new object[]
						{
							"StartJob [",
							newJob,
							"] lastJobEndCondition=",
							lastJobEndCondition,
							", jobGiver=",
							jobGiver,
							", cancelBusyStances=",
							cancelBusyStances.ToString()
						}));
					}
					if (cancelBusyStances && this.pawn.stances.FullBodyBusy)
					{
						this.pawn.stances.CancelBusyStanceHard();
					}
					if (this.curJob != null)
					{
						if (lastJobEndCondition == JobCondition.None)
						{
							Log.Warning(string.Concat(new object[]
							{
								this.pawn,
								" starting job ",
								newJob,
								" from JobGiver ",
								newJob.jobGiver,
								" while already having job ",
								this.curJob,
								" without a specific job end condition."
							}), false);
							lastJobEndCondition = JobCondition.InterruptForced;
						}
						if (resumeCurJobAfterwards && this.curJob.def.suspendable)
						{
							this.jobQueue.EnqueueFirst(this.curJob, null);
							if (this.debugLog)
							{
								this.DebugLogEvent("   JobQueue EnqueueFirst curJob: " + this.curJob);
							}
							this.CleanupCurrentJob(lastJobEndCondition, false, cancelBusyStances, false);
						}
						else
						{
							this.CleanupCurrentJob(lastJobEndCondition, true, cancelBusyStances, canReturnCurJobToPool);
						}
					}
					if (newJob == null)
					{
						Log.Warning(this.pawn + " tried to start doing a null job.", false);
					}
					else
					{
						newJob.startTick = Find.TickManager.TicksGame;
						if (this.pawn.Drafted || newJob.playerForced)
						{
							newJob.ignoreForbidden = true;
							newJob.ignoreDesignations = true;
						}
						this.curJob = newJob;
						this.curJob.jobGiverThinkTree = thinkTree;
						this.curJob.jobGiver = jobGiver;
						this.curDriver = this.curJob.MakeDriver(this.pawn);
						if (this.curDriver.TryMakePreToilReservations(!fromQueue))
						{
							Job job = this.TryOpportunisticJob(newJob);
							if (job != null)
							{
								this.jobQueue.EnqueueFirst(newJob, null);
								this.curJob = null;
								this.curDriver = null;
								this.StartJob(job, JobCondition.None, null, false, true, null, null, false, false);
							}
							else
							{
								if (tag != null)
								{
									this.pawn.mindState.lastJobTag = tag.Value;
								}
								this.curDriver.SetInitialPosture();
								this.curDriver.Notify_Starting();
								this.curDriver.SetupToils();
								this.curDriver.ReadyForNextToil();
							}
						}
						else if (fromQueue)
						{
							this.EndCurrentJob(JobCondition.QueuedNoLongerValid, true, true);
						}
						else
						{
							Log.Warning("TryMakePreToilReservations() returned false for a non-queued job right after StartJob(). This should have been checked before. curJob=" + this.curJob.ToStringSafe<Job>(), false);
							this.EndCurrentJob(JobCondition.Errored, true, true);
						}
					}
				}
			}
			finally
			{
				this.startingNewJob = false;
			}
		}

		
		public void EndCurrentOrQueuedJob(Job job, JobCondition condition, bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"EndJob [",
					job,
					"] condition=",
					condition
				}));
			}
			QueuedJob queuedJob = this.jobQueue.Extract(job);
			if (queuedJob != null)
			{
				this.pawn.ClearReservationsForJob(queuedJob.job);
				if (canReturnToPool)
				{
					JobMaker.ReturnToPool(queuedJob.job);
				}
			}
			if (this.curJob == job)
			{
				this.EndCurrentJob(condition, true, canReturnToPool);
			}
		}

		
		public void EndCurrentJob(JobCondition condition, bool startNewJob = true, bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"EndCurrentJob ",
					(this.curJob != null) ? this.curJob.ToString() : "null",
					" condition=",
					condition,
					" curToil=",
					(this.curDriver != null) ? this.curDriver.CurToilIndex.ToString() : "null_driver"
				}));
			}
			if (condition == JobCondition.Ongoing)
			{
				Log.Warning("Ending a job with Ongoing as the condition. This makes no sense.", false);
			}
			if (condition == JobCondition.Succeeded && this.curJob != null && this.curJob.def.taleOnCompletion != null)
			{
				TaleRecorder.RecordTale(this.curJob.def.taleOnCompletion, this.curDriver.TaleParameters());
			}
			JobDef jobDef = (this.curJob != null) ? this.curJob.def : null;
			this.CleanupCurrentJob(condition, true, true, canReturnToPool);
			if (startNewJob)
			{
				if (condition == JobCondition.ErroredPather || condition == JobCondition.Errored)
				{
					this.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 250, false), JobCondition.None, null, false, true, null, null, false, false);
					return;
				}
				if (condition == JobCondition.Succeeded && jobDef != null && jobDef != JobDefOf.Wait_MaintainPosture && !this.pawn.pather.Moving)
				{
					this.StartJob(JobMaker.MakeJob(JobDefOf.Wait_MaintainPosture, 1, false), JobCondition.None, null, false, false, null, null, false, false);
					return;
				}
				this.TryFindAndStartJob();
			}
		}

		
		private void CleanupCurrentJob(JobCondition condition, bool releaseReservations, bool cancelBusyStancesSoft = true, bool canReturnToPool = false)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent(string.Concat(new object[]
				{
					"CleanupCurrentJob ",
					(this.curJob != null) ? this.curJob.def.ToString() : "null",
					" condition ",
					condition
				}));
			}
			if (this.curJob == null)
			{
				return;
			}
			if (releaseReservations)
			{
				this.pawn.ClearReservationsForJob(this.curJob);
			}
			if (this.curDriver != null)
			{
				this.curDriver.ended = true;
				this.curDriver.Cleanup(condition);
			}
			this.curDriver = null;
			Job job = this.curJob;
			this.curJob = null;
			this.pawn.VerifyReservations();
			if (cancelBusyStancesSoft)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
			if (!this.pawn.Destroyed && this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null)
			{
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
			}
			if (releaseReservations && canReturnToPool)
			{
				JobMaker.ReturnToPool(job);
			}
		}

		
		public void ClearQueuedJobs(bool canReturnToPool = true)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("ClearQueuedJobs");
			}
			while (this.jobQueue.Count > 0)
			{
				Job job = this.jobQueue.Dequeue().job;
				this.pawn.ClearReservationsForJob(job);
				if (canReturnToPool)
				{
					JobMaker.ReturnToPool(job);
				}
			}
		}

		
		public void CheckForJobOverride()
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("CheckForJobOverride");
			}
			ThinkTreeDef thinkTree;
			ThinkResult thinkResult = this.DetermineNextJob(out thinkTree);
			if (thinkResult.IsValid)
			{
				if (this.ShouldStartJobFromThinkTree(thinkResult))
				{
					this.CheckLeaveJoinableLordBecauseJobIssued(thinkResult);
					this.StartJob(thinkResult.Job, JobCondition.InterruptOptional, thinkResult.SourceNode, false, false, thinkTree, thinkResult.Tag, thinkResult.FromQueue, false);
					return;
				}
				if (thinkResult.Job != this.curJob && !this.jobQueue.Contains(thinkResult.Job))
				{
					JobMaker.ReturnToPool(thinkResult.Job);
				}
			}
		}

		
		public void StopAll(bool ifLayingKeepLaying = false, bool canReturnToPool = true)
		{
			if ((!this.pawn.InBed() && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.LayDown || !this.pawn.GetPosture().Laying())) || !ifLayingKeepLaying)
			{
				this.CleanupCurrentJob(JobCondition.InterruptForced, true, true, canReturnToPool);
			}
			this.ClearQueuedJobs(canReturnToPool);
		}

		
		private void TryFindAndStartJob()
		{
			if (this.pawn.thinker == null)
			{
				Log.ErrorOnce(this.pawn + " did TryFindAndStartJob but had no thinker.", 8573261, false);
				return;
			}
			if (this.curJob != null)
			{
				Log.Warning(this.pawn + " doing TryFindAndStartJob while still having job " + this.curJob, false);
			}
			if (this.debugLog)
			{
				this.DebugLogEvent("TryFindAndStartJob");
			}
			if (!this.CanDoAnyJob())
			{
				if (this.debugLog)
				{
					this.DebugLogEvent("   CanDoAnyJob is false. Clearing queue and returning");
				}
				this.ClearQueuedJobs(true);
				return;
			}
			ThinkTreeDef thinkTree;
			ThinkResult result = this.DetermineNextJob(out thinkTree);
			if (result.IsValid)
			{
				this.CheckLeaveJoinableLordBecauseJobIssued(result);
				this.StartJob(result.Job, JobCondition.None, result.SourceNode, false, false, thinkTree, result.Tag, result.FromQueue, false);
			}
		}

		
		public Job TryOpportunisticJob(Job job)
		{
			if (this.pawn.def.race.intelligence < Intelligence.Humanlike)
			{
				return null;
			}
			if (this.pawn.Faction != Faction.OfPlayer)
			{
				return null;
			}
			if (this.pawn.Drafted)
			{
				return null;
			}
			if (job.playerForced)
			{
				return null;
			}
			if (this.pawn.RaceProps.intelligence < Intelligence.Humanlike)
			{
				return null;
			}
			if (!job.def.allowOpportunisticPrefix)
			{
				return null;
			}
			if (this.pawn.WorkTagIsDisabled(WorkTags.ManualDumb | WorkTags.Hauling))
			{
				return null;
			}
			if (this.pawn.InMentalState || this.pawn.IsBurning())
			{
				return null;
			}
			IntVec3 cell = job.targetA.Cell;
			if (!cell.IsValid || cell.IsForbidden(this.pawn))
			{
				return null;
			}
			float num = this.pawn.Position.DistanceTo(cell);
			if (num < 3f)
			{
				return null;
			}
			List<Thing> list = this.pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				float num2 = this.pawn.Position.DistanceTo(thing.Position);
				if (num2 <= 30f && num2 <= num * 0.5f && num2 + thing.Position.DistanceTo(cell) <= num * 1.7f && this.pawn.Map.reservationManager.FirstRespectedReserver(thing, this.pawn) == null && !thing.IsForbidden(this.pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(this.pawn, thing, false))
				{
					StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
					IntVec3 invalid = IntVec3.Invalid;
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, this.pawn.Map, currentPriority, this.pawn.Faction, out invalid, true))
					{
						float num3 = invalid.DistanceTo(cell);
						if (num3 <= 50f && num3 <= num * 0.6f && num2 + thing.Position.DistanceTo(invalid) + num3 <= num * 1.7f && num2 + num3 <= num && this.pawn.Position.WithinRegions(thing.Position, this.pawn.Map, 25, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable) && invalid.WithinRegions(cell, this.pawn.Map, 25, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
						{
							if (DebugViewSettings.drawOpportunisticJobs)
							{
								Log.Message("Opportunistic job spawned", false);
								this.pawn.Map.debugDrawer.FlashLine(this.pawn.Position, thing.Position, 600, SimpleColor.Red);
								this.pawn.Map.debugDrawer.FlashLine(thing.Position, invalid, 600, SimpleColor.Green);
								this.pawn.Map.debugDrawer.FlashLine(invalid, cell, 600, SimpleColor.Blue);
							}
							return HaulAIUtility.HaulToCellStorageJob(this.pawn, thing, invalid, false);
						}
					}
				}
			}
			return null;
		}

		
		private ThinkResult DetermineNextJob(out ThinkTreeDef thinkTree)
		{
			ThinkResult result = this.DetermineNextConstantThinkTreeJob();
			if (result.Job != null)
			{
				thinkTree = this.pawn.thinker.ConstantThinkTree;
				return result;
			}
			ThinkResult result2 = ThinkResult.NoJob;
			try
			{
				result2 = this.pawn.thinker.MainThinkNodeRoot.TryIssueJobPackage(this.pawn, default(JobIssueParams));
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (main)", exception, null);
				thinkTree = null;
				return ThinkResult.NoJob;
			}
			finally
			{
			}
			thinkTree = this.pawn.thinker.MainThinkTree;
			return result2;
		}

		
		private ThinkResult DetermineNextConstantThinkTreeJob()
		{
			if (this.pawn.thinker.ConstantThinkTree == null)
			{
				return ThinkResult.NoJob;
			}
			try
			{
				return this.pawn.thinker.ConstantThinkNodeRoot.TryIssueJobPackage(this.pawn, default(JobIssueParams));
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, this.pawn.ToStringSafe<Pawn>() + " threw exception while determining job (constant)", exception, null);
			}
			finally
			{
			}
			return ThinkResult.NoJob;
		}

		
		private void CheckLeaveJoinableLordBecauseJobIssued(ThinkResult result)
		{
			if (!result.IsValid || result.SourceNode == null)
			{
				return;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null || !(lord.LordJob is LordJob_VoluntarilyJoinable))
			{
				return;
			}
			bool flag = false;
			ThinkNode thinkNode = result.SourceNode;
			while (!thinkNode.leaveJoinableLordIfIssuesJob)
			{
				thinkNode = thinkNode.parent;
				if (thinkNode == null)
				{
					IL_50:
					if (flag)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.LeftVoluntarily, null);
					}
					return;
				}
			}
			flag = true;
			goto IL_50;
		}

		
		private bool CanDoAnyJob()
		{
			return this.pawn.Spawned;
		}

		
		private bool ShouldStartJobFromThinkTree(ThinkResult thinkResult)
		{
			return this.curJob == null || (this.curJob != thinkResult.Job && (thinkResult.FromQueue || (thinkResult.Job.def != this.curJob.def || thinkResult.SourceNode != this.curJob.jobGiver || !this.curDriver.IsContinuation(thinkResult.Job))));
		}

		
		public bool IsCurrentJobPlayerInterruptible()
		{
			return (this.curJob == null || this.curJob.def.playerInterruptible) && !this.pawn.HasAttachment(ThingDefOf.Fire);
		}

		
		public bool TryTakeOrderedJobPrioritizedWork(Job job, WorkGiver giver, IntVec3 cell)
		{
			if (this.TryTakeOrderedJob(job, giver.def.tagToGive))
			{
				job.workGiverDef = giver.def;
				if (giver.def.prioritizeSustains)
				{
					this.pawn.mindState.priorityWork.Set(cell, giver.def);
				}
				return true;
			}
			return false;
		}

		
		public bool TryTakeOrderedJob(Job job, JobTag tag = JobTag.Misc)
		{
			if (this.debugLog)
			{
				this.DebugLogEvent("TryTakeOrderedJob " + job);
			}
			job.playerForced = true;
			if (this.curJob != null && this.curJob.JobIsSameAs(job))
			{
				return true;
			}
			bool flag = this.pawn.jobs.IsCurrentJobPlayerInterruptible();
			bool flag2 = this.pawn.mindState.IsIdle || this.pawn.CurJob == null || this.pawn.CurJob.def.isIdle;
			bool isDownEvent = KeyBindingDefOf.QueueOrder.IsDownEvent;
			if (isDownEvent)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.QueueOrders, KnowledgeAmount.NoteTaught);
			}
			if (flag && (!isDownEvent || flag2))
			{
				this.pawn.stances.CancelBusyStanceSoft();
				if (this.debugLog)
				{
					this.DebugLogEvent("    Queueing job");
				}
				this.ClearQueuedJobs(true);
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueFirst(job, new JobTag?(tag));
					if (this.curJob != null)
					{
						this.curDriver.EndJobWith(JobCondition.InterruptForced);
					}
					else
					{
						this.CheckForJobOverride();
					}
					return true;
				}
				Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>(), false);
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
			else if (isDownEvent)
			{
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueLast(job, new JobTag?(tag));
					return true;
				}
				Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>(), false);
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
			else
			{
				this.ClearQueuedJobs(true);
				if (job.TryMakePreToilReservations(this.pawn, true))
				{
					this.jobQueue.EnqueueLast(job, new JobTag?(tag));
					return true;
				}
				Log.Warning("TryMakePreToilReservations() returned false right after TryTakeOrderedJob(). This should have been checked before. job=" + job.ToStringSafe<Job>(), false);
				this.pawn.ClearReservationsForJob(job);
				return false;
			}
		}

		
		public void Notify_TuckedIntoBed(Building_Bed bed)
		{
			this.pawn.Position = RestUtility.GetBedSleepingSlotPosFor(this.pawn, bed);
			this.pawn.Notify_Teleported(false, true);
			this.pawn.stances.CancelBusyStanceHard();
			this.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, bed), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.TuckedIntoBed), false, false);
		}

		
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (this.curJob == null)
			{
				return;
			}
			Job job = this.curJob;
			this.curDriver.Notify_DamageTaken(dinfo);
			if (this.curJob == job && dinfo.Def.ExternalViolenceFor(this.pawn) && dinfo.Def.canInterruptJobs && !this.curJob.playerForced && Find.TickManager.TicksGame >= this.lastDamageCheckTick + 180)
			{
				Thing instigator = dinfo.Instigator;
				if (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.Always || (this.curJob.def.checkOverrideOnDamage == CheckJobOverrideOnDamageMode.OnlyIfInstigatorNotJobTarget && !this.curJob.AnyTargetIs(instigator)))
				{
					this.lastDamageCheckTick = Find.TickManager.TicksGame;
					this.CheckForJobOverride();
				}
			}
		}

		
		internal void Notify_MasterDraftedOrUndrafted()
		{
			Pawn master = this.pawn.playerSettings.Master;
			if (master.Spawned && master.Map == this.pawn.Map && this.pawn.playerSettings.followDrafted)
			{
				this.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		
		public void DrawLinesBetweenTargets()
		{
			Vector3 a = this.pawn.Position.ToVector3Shifted();
			if (this.pawn.pather.curPath != null)
			{
				a = this.pawn.pather.Destination.CenterVector3;
			}
			else if (this.curJob != null && this.curJob.targetA.IsValid && (!this.curJob.targetA.HasThing || (this.curJob.targetA.Thing.Spawned && this.curJob.targetA.Thing.Map == this.pawn.Map)))
			{
				GenDraw.DrawLineBetween(a, this.curJob.targetA.CenterVector3, AltitudeLayer.Item.AltitudeFor());
				a = this.curJob.targetA.CenterVector3;
			}
			for (int i = 0; i < this.jobQueue.Count; i++)
			{
				if (this.jobQueue[i].job.targetA.IsValid)
				{
					if (!this.jobQueue[i].job.targetA.HasThing || (this.jobQueue[i].job.targetA.Thing.Spawned && this.jobQueue[i].job.targetA.Thing.Map == this.pawn.Map))
					{
						Vector3 centerVector = this.jobQueue[i].job.targetA.CenterVector3;
						GenDraw.DrawLineBetween(a, centerVector, AltitudeLayer.Item.AltitudeFor());
						a = centerVector;
					}
				}
				else
				{
					List<LocalTargetInfo> targetQueueA = this.jobQueue[i].job.targetQueueA;
					if (targetQueueA != null)
					{
						for (int j = 0; j < targetQueueA.Count; j++)
						{
							if (!targetQueueA[j].HasThing || (targetQueueA[j].Thing.Spawned && targetQueueA[j].Thing.Map == this.pawn.Map))
							{
								Vector3 centerVector2 = targetQueueA[j].CenterVector3;
								GenDraw.DrawLineBetween(a, centerVector2, AltitudeLayer.Item.AltitudeFor());
								a = centerVector2;
							}
						}
					}
				}
			}
		}

		
		public void DebugLogEvent(string s)
		{
			if (this.debugLog)
			{
				Log.Message(string.Concat(new object[]
				{
					Find.TickManager.TicksGame,
					" ",
					this.pawn,
					": ",
					s
				}), false);
			}
		}

		
		protected Pawn pawn;

		
		public Job curJob;

		
		public JobDriver curDriver;

		
		public JobQueue jobQueue = new JobQueue();

		
		public PawnPosture posture;

		
		public bool startingNewJob;

		
		private int jobsGivenThisTick;

		
		private string jobsGivenThisTickTextual = "";

		
		private int lastJobGivenAtFrame = -1;

		
		private List<int> jobsGivenRecentTicks = new List<int>(10);

		
		private List<string> jobsGivenRecentTicksTextual = new List<string>(10);

		
		public bool debugLog;

		
		private const int RecentJobQueueMaxLength = 10;

		
		private const int MaxRecentJobs = 10;

		
		private static List<Job> tmpJobsToDequeue = new List<Job>();

		
		private int lastDamageCheckTick = -99999;

		
		private const int DamageCheckMinInterval = 180;
	}
}
