using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A2 RID: 2466
	public static class PawnsArriveQuestPartUtility
	{
		// Token: 0x06003A98 RID: 15000 RVA: 0x001364A5 File Offset: 0x001346A5
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
				p = null;
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x001364B8 File Offset: 0x001346B8
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
