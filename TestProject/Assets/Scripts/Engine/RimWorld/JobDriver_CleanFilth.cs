﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000657 RID: 1623
	public class JobDriver_CleanFilth : JobDriver
	{
		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x000FD464 File Offset: 0x000FB664
		private Filth Filth
		{
			get
			{
				return (Filth)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000FD48A File Offset: 0x000FB68A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000FD4AD File Offset: 0x000FB6AD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue).JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			Toil clean = new Toil();
			clean.initAction = delegate
			{
				this.cleaningWorkDone = 0f;
				this.totalCleaningWorkDone = 0f;
				this.totalCleaningWorkRequired = this.Filth.def.filth.cleaningWorkToReduceThickness * (float)this.Filth.thickness;
			};
			clean.tickAction = delegate
			{
				Filth filth = this.Filth;
				this.cleaningWorkDone += 1f;
				this.totalCleaningWorkDone += 1f;
				if (this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
				{
					filth.ThinFilth();
					this.cleaningWorkDone = 0f;
					if (filth.Destroyed)
					{
						clean.actor.records.Increment(RecordDefOf.MessesCleaned);
						this.ReadyForNextToil();
						return;
					}
				}
			};
			clean.defaultCompleteMode = ToilCompleteMode.Never;
			clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
			clean.WithProgressBar(TargetIndex.A, () => this.totalCleaningWorkDone / this.totalCleaningWorkRequired, true, -0.5f);
			clean.PlaySustainerOrSound(delegate
			{
				ThingDef def = this.Filth.def;
				if (!def.filth.cleaningSound.NullOrUndefined())
				{
					return def.filth.cleaningSound;
				}
				return SoundDefOf.Interact_CleanFilth;
			});
			clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			yield return clean;
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x000FD4C0 File Offset: 0x000FB6C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}

		// Token: 0x040019D2 RID: 6610
		private float cleaningWorkDone;

		// Token: 0x040019D3 RID: 6611
		private float totalCleaningWorkDone;

		// Token: 0x040019D4 RID: 6612
		private float totalCleaningWorkRequired;

		// Token: 0x040019D5 RID: 6613
		private const TargetIndex FilthInd = TargetIndex.A;
	}
}
