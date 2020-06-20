using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC4 RID: 3012
	public static class InteractionUtility
	{
		// Token: 0x06004737 RID: 18231 RVA: 0x00181BC8 File Offset: 0x0017FDC8
		public static bool CanInitiateInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.interactions != null && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, true, false);
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x00181C1A File Offset: 0x0017FE1A
		public static bool CanReceiveInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, false, false);
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x00181C40 File Offset: 0x0017FE40
		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanInitiateInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState && !p.IsInteractionBlocked(null, true, true) && p.Faction != null;
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x00181C8D File Offset: 0x0017FE8D
		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanReceiveInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState;
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x00181CBA File Offset: 0x0017FEBA
		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return InteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x00181CD3 File Offset: 0x0017FED3
		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x00181CF4 File Offset: 0x0017FEF4
		public static bool HasAnyVerbForSocialFight(Pawn p)
		{
			if (p.Dead)
			{
				return false;
			}
			List<Verb> allVerbs = p.verbTracker.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsMeleeAttack && allVerbs[i].IsStillUsableBy(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x00181D48 File Offset: 0x0017FF48
		public static bool TryGetRandomVerbForSocialFight(Pawn p, out Verb verb)
		{
			if (p.Dead)
			{
				verb = null;
				return false;
			}
			return (from x in p.verbTracker.AllVerbs
			where x.IsMeleeAttack && x.IsStillUsableBy(p)
			select x).TryRandomElementByWeight((Verb x) => x.verbProps.AdjustedMeleeDamageAmount(x, p), out verb);
		}

		// Token: 0x04002907 RID: 10503
		public const float MaxInteractRange = 6f;
	}
}
