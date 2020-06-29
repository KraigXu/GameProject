﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Lovin : JobDriver
	{
		
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x000FE278 File Offset: 0x000FC478
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}

		
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x000FE295 File Offset: 0x000FC495
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.BedInd);
			this.FailOnDespawnedOrNull(this.PartnerInd);
			this.FailOn(() => !this.Partner.health.capacities.CanBeAwake);
			this.KeepLyingDown(this.BedInd);
			yield return Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
			yield return Toils_Bed.GotoBed(this.BedInd);
			yield return new Toil
			{
				initAction = delegate
				{
					if (this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin)
					{
						Job newJob = JobMaker.MakeJob(JobDefOf.Lovin, this.pawn, this.Bed);
						this.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false);
						this.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
						return;
					}
					this.ticksLeft = 9999999;
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil toil = Toils_LayDown.LayDown(this.BedInd, true, false, false, false);
			toil.FailOn(() => this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin);
			toil.AddPreTickAction(delegate
			{
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					return;
				}
				if (this.pawn.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
			});
			toil.AddFinishAction(delegate
			{
				Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				if (this.pawn.health != null && this.pawn.health.hediffSet != null)
				{
					if (this.pawn.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
					{
						goto IL_C4;
					}
				}
				if (this.Partner.health == null || this.Partner.health.hediffSet == null)
				{
					goto IL_CF;
				}
				if (!this.Partner.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
				{
					goto IL_CF;
				}
				IL_C4:
				thought_Memory.moodPowerFactor = 1.5f;
				IL_CF:
				if (this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, this.Partner);
				}
				this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + this.GenerateRandomMinTicksToNextLovin(this.pawn);
			});
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield break;
		}

		
		private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin)
			{
				return 100;
			}
			float num = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			num = Rand.Gaussian(num, 0.3f);
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			return (int)(num * 2500f);
		}

		
		private int ticksLeft;

		
		private TargetIndex PartnerInd = TargetIndex.A;

		
		private TargetIndex BedInd = TargetIndex.B;

		
		private const int TicksBetweenHeartMotes = 100;

		
		private static readonly SimpleCurve LovinIntervalHoursFromAgeCurve = new SimpleCurve
		{
			{
				new CurvePoint(16f, 1.5f),
				true
			},
			{
				new CurvePoint(22f, 1.5f),
				true
			},
			{
				new CurvePoint(30f, 4f),
				true
			},
			{
				new CurvePoint(50f, 12f),
				true
			},
			{
				new CurvePoint(75f, 36f),
				true
			}
		};
	}
}
