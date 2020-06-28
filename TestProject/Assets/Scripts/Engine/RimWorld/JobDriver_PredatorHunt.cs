using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000620 RID: 1568
	public class JobDriver_PredatorHunt : JobDriver
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x000F9FC0 File Offset: 0x000F81C0
		public Pawn Prey
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x000F9FF8 File Offset: 0x000F81F8
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000FA01E File Offset: 0x000F821E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.firstHit, "firstHit", false, false);
			Scribe_Values.Look<bool>(ref this.notifiedPlayerAttacking, "notifiedPlayerAttacking", false, false);
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000FA04A File Offset: 0x000F824A
		public override string GetReport()
		{
			if (this.Corpse != null)
			{
				return this.ReportStringProcessed(JobDefOf.Ingest.reportString);
			}
			return base.GetReport();
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000FA06B File Offset: 0x000F826B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFinishAction(delegate
			{
				this.Map.attackTargetsCache.UpdateTarget(this.pawn);
			});
			Toil prepareToEatCorpse = new Toil();
			prepareToEatCorpse.initAction = delegate
			{
				Pawn actor = prepareToEatCorpse.actor;
				Corpse corpse = this.Corpse;
				if (corpse == null)
				{
					Pawn prey = this.Prey;
					if (prey == null)
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					corpse = prey.Corpse;
					if (corpse == null || !corpse.Spawned)
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
				}
				if (actor.Faction == Faction.OfPlayer)
				{
					corpse.SetForbidden(false, false);
				}
				else
				{
					corpse.SetForbidden(true, false);
				}
				actor.CurJob.SetTarget(TargetIndex.A, corpse);
			};
			yield return Toils_General.DoAtomic(delegate
			{
				this.Map.attackTargetsCache.UpdateTarget(this.pawn);
			});
			Action hitAction = delegate
			{
				Pawn prey = this.Prey;
				bool surpriseAttack = this.firstHit && !prey.IsColonist;
				if (this.pawn.meleeVerbs.TryMeleeAttack(prey, this.job.verbToUse, surpriseAttack))
				{
					if (!this.notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
					{
						this.notifiedPlayerAttacked = true;
						Messages.Message("MessageAttackedByPredator".Translate(prey.LabelShort, this.pawn.LabelIndefinite(), prey.Named("PREY"), this.pawn.Named("PREDATOR")).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall, true);
					}
					this.Map.attackTargetsCache.UpdateTarget(this.pawn);
					this.firstHit = false;
				}
			};
			Toil toil = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, hitAction).JumpIfDespawnedOrNull(TargetIndex.A, prepareToEatCorpse).JumpIf(() => this.Corpse != null, prepareToEatCorpse).FailOn(() => Find.TickManager.TicksGame > this.startTick + 5000 && (float)(this.job.GetTarget(TargetIndex.A).Cell - this.pawn.Position).LengthHorizontalSquared > 4f);
			toil.AddPreTickAction(new Action(this.CheckWarnPlayer));
			yield return toil;
			yield return prepareToEatCorpse;
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoCorpse;
			float durationMultiplier = 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
			yield return Toils_Ingest.ChewIngestible(this.pawn, durationMultiplier, TargetIndex.A, TargetIndex.None).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(gotoCorpse, () => this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000FA07C File Offset: 0x000F827C
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && dinfo.Def.isRanged && dinfo.Instigator != null && dinfo.Instigator != this.Prey && !this.pawn.InMentalState && !this.pawn.Downed)
			{
				this.pawn.mindState.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000FA0FC File Offset: 0x000F82FC
		private void CheckWarnPlayer()
		{
			if (this.notifiedPlayerAttacking)
			{
				return;
			}
			Pawn prey = this.Prey;
			if (!prey.Spawned || prey.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame <= this.pawn.mindState.lastPredatorHuntingPlayerNotificationTick + 2500)
			{
				return;
			}
			if (!prey.Position.InHorDistOf(this.pawn.Position, 60f))
			{
				return;
			}
			if (prey.RaceProps.Humanlike)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelPredatorHuntingColonist".Translate(this.pawn.LabelShort, prey.LabelDefinite(), this.pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), "LetterPredatorHuntingColonist".Translate(this.pawn.LabelIndefinite(), prey.LabelDefinite(), this.pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), LetterDefOf.ThreatBig, this.pawn, null, null, null, null);
			}
			else
			{
				Messages.Message((prey.Name.Numerical ? "LetterPredatorHuntingColonist" : "MessagePredatorHuntingPlayerAnimal").Translate(this.pawn.Named("PREDATOR"), prey.Named("PREY")), this.pawn, MessageTypeDefOf.ThreatBig, true);
			}
			this.pawn.mindState.Notify_PredatorHuntingPlayerNotification();
			this.notifiedPlayerAttacking = true;
		}

		// Token: 0x0400197E RID: 6526
		private bool notifiedPlayerAttacked;

		// Token: 0x0400197F RID: 6527
		private bool notifiedPlayerAttacking;

		// Token: 0x04001980 RID: 6528
		private bool firstHit = true;

		// Token: 0x04001981 RID: 6529
		public const TargetIndex PreyInd = TargetIndex.A;

		// Token: 0x04001982 RID: 6530
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04001983 RID: 6531
		private const int MaxHuntTicks = 5000;
	}
}
