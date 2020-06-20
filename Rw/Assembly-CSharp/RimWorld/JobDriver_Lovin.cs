using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000667 RID: 1639
	public class JobDriver_Lovin : JobDriver
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x000FE278 File Offset: 0x000FC478
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x000FE295 File Offset: 0x000FC495
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000FE2B2 File Offset: 0x000FC4B2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000FE2CC File Offset: 0x000FC4CC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000FE327 File Offset: 0x000FC527
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000FE345 File Offset: 0x000FC545
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

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000FE358 File Offset: 0x000FC558
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

		// Token: 0x040019EF RID: 6639
		private int ticksLeft;

		// Token: 0x040019F0 RID: 6640
		private TargetIndex PartnerInd = TargetIndex.A;

		// Token: 0x040019F1 RID: 6641
		private TargetIndex BedInd = TargetIndex.B;

		// Token: 0x040019F2 RID: 6642
		private const int TicksBetweenHeartMotes = 100;

		// Token: 0x040019F3 RID: 6643
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
