using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public static class GenGuest
	{
		
		public static void PrisonerRelease(Pawn p)
		{
			if (p.ownership != null)
			{
				p.ownership.UnclaimAll();
			}
			if (p.Faction == Faction.OfPlayer || p.IsWildMan())
			{
				if (p.needs.mood != null)
				{
					p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasImprisoned, null);
				}
				p.guest.SetGuestStatus(null, false);
				if (p.IsWildMan())
				{
					p.mindState.WildManEverReachedOutside = false;
					return;
				}
			}
			else
			{
				p.guest.Released = true;
				IntVec3 c;
				if (RCellFinder.TryFindBestExitSpot(p, out c, TraverseMode.ByPawn))
				{
					Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
					job.exitMapOnArrival = true;
					p.jobs.StartJob(job, JobCondition.None, null, false, true, null, null, false, false);
				}
			}
		}

		
		public static void AddPrisonerSoldThoughts(Pawn prisoner)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
			{
				if (pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold, null);
				}
			}
		}

		
		public static void AddHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			if (!prisoner.IsColonist)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
				{
					if (pawn.needs.mood != null && pawn != prisoner)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner);
					}
				}
			}
		}

		
		public static void RemoveHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists)
			{
				if (pawn.needs.mood != null && pawn != prisoner)
				{
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner);
				}
			}
		}
	}
}
