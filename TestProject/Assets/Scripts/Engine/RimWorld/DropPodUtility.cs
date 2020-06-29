using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public static class DropPodUtility
	{
		
		public static void MakeDropPodAt(IntVec3 c, Map map, ActiveDropPodInfo info)
		{
			ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			activeDropPod.Contents = info;
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.DropPodIncoming, activeDropPod, c, map);
			IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)activeDropPod.Contents.innerContainer).GetEnumerator();
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn;
					if ((pawn = (enumerator.Current as Pawn)) != null && pawn.IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(pawn);
						Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
						if (psychicEntropy != null)
						{
							psychicEntropy.SetInitialPsyfocusLevel();
						}
					}
				}
			}
		}

		
		public static void DropThingsNear(IntVec3 dropCenter, Map map, IEnumerable<Thing> things, int openDelay = 110, bool canInstaDropDuringInit = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true)
		{
			DropPodUtility.tempList.Clear();
			foreach (Thing item in things)
			{
				List<Thing> list = new List<Thing>();
				list.Add(item);
				DropPodUtility.tempList.Add(list);
			}
			DropPodUtility.DropThingGroupsNear(dropCenter, map, DropPodUtility.tempList, openDelay, canInstaDropDuringInit, leaveSlag, canRoofPunch, forbid);
			DropPodUtility.tempList.Clear();
		}

		
		public static void DropThingGroupsNear_NewTmp(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true, bool allowFogged = true)
		{

			foreach (List<Thing> list in thingsGroups)
			{
				IntVec3 intVec;
				if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, canRoofPunch, true, null) && (canRoofPunch || !DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, true, true, null)))
				{
					Log.Warning(string.Concat(new object[]
					{
						"DropThingsNear failed to find a place to drop ",
						list.FirstOrDefault<Thing>(),
						" near ",
						dropCenter,
						". Dropping on random square instead."
					}), false);
					Predicate<IntVec3> validator = (((IntVec3 c) => c.Walkable(map)));

					intVec = CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
				if (forbid)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].SetForbidden(true, false);
					}
				}
				if (instaDrop)
				{
					List<Thing>.Enumerator enumerator2 = list.GetEnumerator();
					{
						while (enumerator2.MoveNext())
						{
							Thing thing = enumerator2.Current;
							GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Near, null, null, default(Rot4));
						}
						continue;
					}
				}
				ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
				foreach (Thing item in list)
				{
					activeDropPodInfo.innerContainer.TryAdd(item, true);
				}
				activeDropPodInfo.openDelay = openDelay;
				activeDropPodInfo.leaveSlag = leaveSlag;
				DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);
			}
		}

		
		public static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true)
		{
			DropPodUtility.DropThingGroupsNear_NewTmp(dropCenter, map, thingsGroups, openDelay, instaDrop, leaveSlag, canRoofPunch, forbid, true);
		}

		
		private static List<List<Thing>> tempList = new List<List<Thing>>();
	}
}
