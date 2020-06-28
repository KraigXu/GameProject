using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200051E RID: 1310
	public class JobDriver_AttackMelee : JobDriver
	{
		// Token: 0x0600256B RID: 9579 RVA: 0x000DE03A File Offset: 0x000DC23A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numMeleeAttacksMade, "numMeleeAttacksMade", 0, false);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000DE054 File Offset: 0x000DC254
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			IAttackTarget attackTarget = this.job.targetA.Thing as IAttackTarget;
			if (attackTarget != null)
			{
				this.pawn.Map.attackTargetReservationManager.Reserve(this.pawn, this.job, attackTarget);
			}
			return true;
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000DE09D File Offset: 0x000DC29D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.DoAtomic(delegate
			{
				Pawn pawn = this.job.targetA.Thing as Pawn;
				if (pawn != null && pawn.Downed && this.pawn.mindState.duty != null && this.pawn.mindState.duty.attackDownedIfStarving && this.pawn.Starving())
				{
					this.job.killIncappedTarget = true;
				}
			});
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, delegate
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (this.pawn.meleeVerbs.TryMeleeAttack(thing, this.job.verbToUse, false))
				{
					if (this.pawn.CurJob == null || this.pawn.jobs.curDriver != this)
					{
						return;
					}
					this.numMeleeAttacksMade++;
					if (this.numMeleeAttacksMade >= this.job.maxNumMeleeAttacks)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
				}
			}).FailOnDespawnedOrNull(TargetIndex.A);
			yield break;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000DE0B0 File Offset: 0x000DC2B0
		public override void Notify_PatherFailed()
		{
			if (this.job.attackDoorIfTargetLost)
			{
				Thing thing;
				using (PawnPath pawnPath = base.Map.pathFinder.FindPath(this.pawn.Position, base.TargetA.Cell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
				{
					if (!pawnPath.Found)
					{
						return;
					}
					IntVec3 position;
					thing = pawnPath.FirstBlockingBuilding(out position, this.pawn);
				}
				if (thing != null)
				{
					IntVec3 position = thing.Position;
					if (position.InHorDistOf(this.pawn.Position, 6f))
					{
						this.job.targetA = thing;
						this.job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
						this.job.expiryInterval = Rand.Range(2000, 4000);
						return;
					}
				}
			}
			base.Notify_PatherFailed();
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000DD564 File Offset: 0x000DB764
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x040016DD RID: 5853
		private int numMeleeAttacksMade;
	}
}
