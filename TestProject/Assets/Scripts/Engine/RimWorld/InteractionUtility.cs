using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public static class InteractionUtility
	{
		
		public static bool CanInitiateInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.interactions != null && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, true, false);
		}

		
		public static bool CanReceiveInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, false, false);
		}

		
		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanInitiateInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState && !p.IsInteractionBlocked(null, true, true) && p.Faction != null;
		}

		
		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanReceiveInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState;
		}

		
		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return InteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		
		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		
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

		
		public const float MaxInteractRange = 6f;
	}
}
