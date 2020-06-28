using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006AB RID: 1707
	public abstract class JobGiver_AIDefendPawn : JobGiver_AIFightEnemy
	{
		// Token: 0x06002E2C RID: 11820 RVA: 0x00103C22 File Offset: 0x00101E22
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIDefendPawn jobGiver_AIDefendPawn = (JobGiver_AIDefendPawn)base.DeepCopy(resolve);
			jobGiver_AIDefendPawn.attackMeleeThreatEvenIfNotHostile = this.attackMeleeThreatEvenIfNotHostile;
			return jobGiver_AIDefendPawn;
		}

		// Token: 0x06002E2D RID: 11821
		protected abstract Pawn GetDefendee(Pawn pawn);

		// Token: 0x06002E2E RID: 11822 RVA: 0x00103C3C File Offset: 0x00101E3C
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee.Spawned || defendee.CarriedBy != null)
			{
				return defendee.PositionHeld;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x00103C70 File Offset: 0x00101E70
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee == null)
			{
				Log.Error(base.GetType() + " has null defendee. pawn=" + pawn.ToStringSafe<Pawn>(), false);
				return null;
			}
			Pawn carriedBy = defendee.CarriedBy;
			if (carriedBy != null)
			{
				if (!pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return null;
				}
			}
			else if (!defendee.Spawned || !pawn.CanReach(defendee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x00103CE8 File Offset: 0x00101EE8
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			if (this.attackMeleeThreatEvenIfNotHostile)
			{
				Pawn defendee = this.GetDefendee(pawn);
				if (defendee.Spawned && !defendee.InMentalState && defendee.mindState.meleeThreat != null && defendee.mindState.meleeThreat != pawn && pawn.CanReach(defendee.mindState.meleeThreat, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return defendee.mindState.meleeThreat;
				}
			}
			return base.FindAttackTarget(pawn);
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x00103D60 File Offset: 0x00101F60
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Verb verb = pawn.TryGetAttackVerb(null, !pawn.IsColonist);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = pawn.mindState.enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = this.GetDefendee(pawn).PositionHeld,
				maxRangeFromLocus = this.GetFlagRadius(pawn),
				wantCoverFromTarget = (verb.verbProps.range > 7f),
				maxRegions = 50
			}, out dest);
		}

		// Token: 0x04001A5A RID: 6746
		private bool attackMeleeThreatEvenIfNotHostile;
	}
}
