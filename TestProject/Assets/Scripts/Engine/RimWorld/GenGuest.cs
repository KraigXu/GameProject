using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7E RID: 2942
	public static class GenGuest
	{
		// Token: 0x060044EA RID: 17642 RVA: 0x00174188 File Offset: 0x00172388
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

		// Token: 0x060044EB RID: 17643 RVA: 0x00174258 File Offset: 0x00172458
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

		// Token: 0x060044EC RID: 17644 RVA: 0x001742D0 File Offset: 0x001724D0
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

		// Token: 0x060044ED RID: 17645 RVA: 0x00174354 File Offset: 0x00172554
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
