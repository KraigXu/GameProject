using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public static class PawnsArriveQuestPartUtility
	{
		
		public static IEnumerable<Pawn> GetQuestLookTargets(IEnumerable<Pawn> pawns)
		{
			if (pawns.Count<Pawn>() == 1)
			{
				yield return pawns.First<Pawn>();
				yield break;
			}
			foreach (Pawn p in pawns)
			{
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					yield return p;
				}
				if (p.Faction == null && p.Downed)
				{
					yield return p;
				}
				
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		
		public static bool IncreasesPopulation(IEnumerable<Pawn> pawns, bool joinPlayer, bool makePrisoners)
		{
			foreach (Pawn pawn in pawns)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Destroyed && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || pawn.Downed || joinPlayer || makePrisoners))
				{
					return true;
				}
			}
			return false;
		}
	}
}
