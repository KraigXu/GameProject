using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006AF RID: 1711
	public class JobGiver_AIDefendPoint : JobGiver_AIFightEnemy
	{
		// Token: 0x06002E3C RID: 11836 RVA: 0x00103E78 File Offset: 0x00102078
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Verb verb = pawn.TryGetAttackVerb(enemyTarget, !pawn.IsColonist);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = (IntVec3)pawn.mindState.duty.focus,
				maxRangeFromLocus = pawn.mindState.duty.radius,
				wantCoverFromTarget = (verb.verbProps.range > 7f)
			}, out dest);
		}
	}
}
