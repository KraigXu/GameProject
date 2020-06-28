using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000768 RID: 1896
	public static class GatheringsUtility
	{
		// Token: 0x06003186 RID: 12678 RVA: 0x00113AC0 File Offset: 0x00111CC0
		public static bool ShouldGuestKeepAttendingGathering(Pawn p)
		{
			return !p.Downed && (p.needs == null || !p.needs.food.Starving) && p.health.hediffSet.BleedRateTotal <= 0f && (p.needs.rest == null || p.needs.rest.CurCategory < RestCategory.Exhausted) && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && p.Awake() && !p.InAggroMentalState && !p.IsPrisoner;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x00113B64 File Offset: 0x00111D64
		public static bool PawnCanStartOrContinueGathering(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.3f)
			{
				return false;
			}
			if (pawn.IsPrisoner)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			return (firstHediffOfDef == null || firstHediffOfDef.Severity <= 0.2f) && !pawn.IsWildMan() && (pawn.psychicEntropy == null || !pawn.psychicEntropy.IsCurrentlyMeditating) && (pawn.Spawned && !pawn.Downed) && !pawn.InMentalState;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x00113C04 File Offset: 0x00111E04
		public static bool AnyLordJobPreventsNewGatherings(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (!lords[i].LordJob.AllowStartNewGatherings)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x00113C44 File Offset: 0x00111E44
		public static bool AcceptableGameConditionsToStartGathering(Map map, GatheringDef gatheringDef)
		{
			if (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(map))
			{
				return false;
			}
			if (GenLocalDate.HourInteger(map) < 4 || GenLocalDate.HourInteger(map) > 21)
			{
				return false;
			}
			if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
			{
				return false;
			}
			if (map.dangerWatcher.DangerRating != StoryDanger.None)
			{
				return false;
			}
			int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
			if (freeColonistsSpawnedCount < 4)
			{
				return false;
			}
			int num = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.health.hediffSet.BleedRateTotal > 0f)
				{
					return false;
				}
				if (pawn.Drafted)
				{
					num++;
				}
			}
			return (float)num / (float)freeColonistsSpawnedCount < 0.5f && GatheringsUtility.EnoughPotentialGuestsToStartGathering(map, gatheringDef, null);
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x00113D30 File Offset: 0x00111F30
		public static bool AcceptableGameConditionsToContinueGathering(Map map)
		{
			return map.dangerWatcher.DangerRating != StoryDanger.High;
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x00113D44 File Offset: 0x00111F44
		public static bool ValidateGatheringSpot(IntVec3 cell, GatheringDef gatheringDef, Pawn organizer, bool enjoyableOutside)
		{
			Map map = organizer.Map;
			if (!cell.Standable(map))
			{
				return false;
			}
			if (cell.GetDangerFor(organizer, map) != Danger.None)
			{
				return false;
			}
			if (!enjoyableOutside && !cell.Roofed(map))
			{
				return false;
			}
			if (cell.IsForbidden(organizer))
			{
				return false;
			}
			if (!organizer.CanReserveAndReach(cell, PathEndMode.OnCell, Danger.None, 1, -1, null, false))
			{
				return false;
			}
			Room room = cell.GetRoom(map, RegionType.Set_Passable);
			bool flag = room != null && room.isPrisonCell;
			return organizer.IsPrisoner == flag && GatheringsUtility.EnoughPotentialGuestsToStartGathering(map, gatheringDef, new IntVec3?(cell));
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x00113DD4 File Offset: 0x00111FD4
		public static bool EnoughPotentialGuestsToStartGathering(Map map, GatheringDef gatheringDef, IntVec3? gatherSpot = null)
		{
			int num = Mathf.RoundToInt((float)map.mapPawns.FreeColonistsSpawnedCount * 0.65f);
			num = Mathf.Clamp(num, 2, 10);
			int num2 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (GatheringsUtility.ShouldPawnKeepGathering(pawn, gatheringDef) && (gatherSpot == null || !gatherSpot.Value.IsForbidden(pawn)) && (gatherSpot == null || pawn.CanReach(gatherSpot.Value, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn)))
				{
					num2++;
				}
			}
			return num2 >= num;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x00113E98 File Offset: 0x00112098
		public static Pawn FindRandomGatheringOrganizer(Faction faction, Map map, GatheringDef gatheringDef)
		{
			Predicate<RoyalTitle> <>9__2;
			Predicate<Pawn> v = delegate(Pawn x)
			{
				if (!x.RaceProps.Humanlike || x.InBed() || x.InMentalState || x.GetLord() != null || !GatheringsUtility.ShouldPawnKeepGathering(x, gatheringDef) || x.Drafted)
				{
					return false;
				}
				if (gatheringDef.requiredTitleAny == null || gatheringDef.requiredTitleAny.Count == 0)
				{
					return true;
				}
				if (x.royalty != null)
				{
					List<RoyalTitle> allTitlesInEffectForReading = x.royalty.AllTitlesInEffectForReading;
					Predicate<RoyalTitle> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((RoyalTitle t) => gatheringDef.requiredTitleAny.Contains(t.def)));
					}
					return allTitlesInEffectForReading.Any(predicate);
				}
				return false;
			};
			Pawn result;
			if ((from x in map.mapPawns.SpawnedPawnsInFaction(faction)
			where v(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x00113EF0 File Offset: 0x001120F0
		public static bool InGatheringArea(IntVec3 cell, IntVec3 partySpot, Map map)
		{
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(partySpot, map) && cell.GetRoom(map, RegionType.Set_Passable) == partySpot.GetRoom(map, RegionType.Set_Passable))
			{
				return true;
			}
			if (!cell.InHorDistOf(partySpot, 10f))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.None, false);
			if (edifice != null)
			{
				return map.reachability.CanReach(partySpot, edifice, PathEndMode.ClosestTouch, traverseParams);
			}
			return map.reachability.CanReach(partySpot, cell, PathEndMode.ClosestTouch, traverseParams);
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x00113F68 File Offset: 0x00112168
		public static bool TryFindRandomCellInGatheringArea(Pawn pawn, out IntVec3 result)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false);
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(cell, pawn.Map))
			{
				return (from x in cell.GetRoom(pawn.Map, RegionType.Set_Passable).Cells
				where validator(x)
				select x).TryRandomElement(out result);
			}
			return CellFinder.TryFindRandomReachableCellNear(cell, pawn.Map, 10f, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 x) => validator(x), null, out result, 10);
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x00114020 File Offset: 0x00112220
		public static bool UseWholeRoomAsGatheringArea(IntVec3 partySpot, Map map)
		{
			Room room = partySpot.GetRoom(map, RegionType.Set_Passable);
			return room != null && !room.IsHuge && !room.PsychologicallyOutdoors && room.CellCount <= 324;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x00114059 File Offset: 0x00112259
		public static bool ShouldPawnKeepGathering(Pawn p, GatheringDef gatheringDef)
		{
			return (!gatheringDef.respectTimetable || p.timetable == null || p.timetable.CurrentAssignment.allowJoy) && GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x04001B04 RID: 6916
		private const float GatherAreaRadiusIfNotWholeRoom = 10f;

		// Token: 0x04001B05 RID: 6917
		private const int MaxRoomCellsCountToUseWholeRoom = 324;
	}
}
