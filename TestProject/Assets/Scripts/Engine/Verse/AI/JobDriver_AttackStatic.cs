using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	
	public class JobDriver_AttackStatic : JobDriver
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate
				{
					Pawn pawn = base.TargetThingA as Pawn;
					if (pawn != null)
					{
						this.startedIncapacitated = pawn.Downed;
					}
					this.pawn.pather.StopDead();
				},
				tickAction = delegate
				{
					if (!base.TargetA.IsValid)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					if (base.TargetA.HasThing)
					{
						Pawn pawn = base.TargetA.Thing as Pawn;
						if (base.TargetA.Thing.Destroyed || (pawn != null && !this.startedIncapacitated && pawn.Downed) || (pawn != null && pawn.IsInvisible()))
						{
							base.EndJobWith(JobCondition.Succeeded);
							return;
						}
					}
					if (this.numAttacksMade >= this.job.maxNumStaticAttacks && !this.pawn.stances.FullBodyBusy)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					if (this.pawn.TryStartAttack(base.TargetA))
					{
						this.numAttacksMade++;
						return;
					}
					if (!this.pawn.stances.FullBodyBusy)
					{
						Verb verb = this.pawn.TryGetAttackVerb(base.TargetA.Thing, !this.pawn.IsColonist);
						if (this.job.endIfCantShootTargetFromCurPos && (verb == null || !verb.CanHitTargetFrom(this.pawn.Position, base.TargetA)))
						{
							base.EndJobWith(JobCondition.Incompletable);
							return;
						}
						if (this.job.endIfCantShootInMelee)
						{
							if (verb == null)
							{
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
							float num = verb.verbProps.EffectiveMinRange(base.TargetA, this.pawn);
							if ((float)this.pawn.Position.DistanceToSquared(base.TargetA.Cell) < num * num && this.pawn.Position.AdjacentTo8WayOrInside(base.TargetA.Cell))
							{
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		
		private bool startedIncapacitated;

		
		private int numAttacksMade;
	}
}
