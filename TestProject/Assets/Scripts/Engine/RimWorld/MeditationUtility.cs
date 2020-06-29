using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class MeditationUtility
	{
		
		public static Job GetMeditationJob(Pawn pawn, bool forJoy = false)
		{
			MeditationSpotAndFocus meditationSpotAndFocus = MeditationUtility.FindMeditationSpot(pawn);
			if (meditationSpotAndFocus.IsValid)
			{
				Building_Throne t;
				Job job;
				if ((t = (meditationSpotAndFocus.focus.Thing as Building_Throne)) != null)
				{
					job = JobMaker.MakeJob(JobDefOf.Reign, t, null, t);
				}
				else
				{
					job = JobMaker.MakeJob(JobDefOf.Meditate, meditationSpotAndFocus.spot, null, meditationSpotAndFocus.focus);
				}
				job.ignoreJoyTimeAssignment = !forJoy;
				return job;
			}
			return null;
		}

		
		public static MeditationSpotAndFocus FindMeditationSpot(Pawn pawn)
		{
			float num = float.MinValue;
			LocalTargetInfo spot = LocalTargetInfo.Invalid;
			LocalTargetInfo focus = LocalTargetInfo.Invalid;
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psyfocus meditation is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657324, false);
				return new MeditationSpotAndFocus(spot, focus);
			}
			Room ownedRoom = pawn.ownership.OwnedRoom;
			foreach (LocalTargetInfo localTargetInfo in MeditationUtility.AllMeditationSpotCandidates(pawn, true))
			{
				if (MeditationUtility.SafeEnvironmentalConditions(pawn, localTargetInfo.Cell, pawn.Map))
				{
					LocalTargetInfo localTargetInfo2 = (localTargetInfo.Thing is Building_Throne) ? localTargetInfo.Thing : MeditationUtility.BestFocusAt(localTargetInfo, pawn);
					float num2 = 1f / Mathf.Max((float)localTargetInfo.Cell.DistanceToSquared(pawn.Position), 0.1f);
					if (pawn.HasPsylink && localTargetInfo2.IsValid)
					{
						num2 += localTargetInfo2.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) * 100f;
					}
					Room room = localTargetInfo.Cell.GetRoom(pawn.Map, RegionType.Set_Passable);
					if (room != null && ownedRoom == room)
					{
						num2 += 1f;
					}
					Building building;
					if (localTargetInfo.Thing != null && (building = (localTargetInfo.Thing as Building)) != null && building.GetAssignedPawn() == pawn)
					{
						num2 = float.PositiveInfinity;
					}
					if (!localTargetInfo.Cell.Standable(pawn.Map))
					{
						num2 = float.NegativeInfinity;
					}
					if (num2 > num)
					{
						spot = localTargetInfo;
						focus = localTargetInfo2;
						num = num2;
					}
				}
			}
			return new MeditationSpotAndFocus(spot, focus);
		}

		
		public static IEnumerable<LocalTargetInfo> AllMeditationSpotCandidates(Pawn pawn, bool allowFallbackSpots = true)
		{
			bool flag = false;
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0 && !pawn.IsPrisonerOfColony)
			{
				Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(pawn);
				if (building_Throne != null)
				{
					yield return building_Throne;
					flag = true;
				}
			}
			if (!pawn.IsPrisonerOfColony)
			{
				IEnumerable<Building> source = pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MeditationSpot);
				
				Func<Building, bool> predicate;
				if ((predicate=default ) == null)
				{
					predicate = ( delegate(Building s)
					{
						if (s.IsForbidden(pawn) || !s.Position.Standable(s.Map))
						{
							return false;
						}
						if (s.GetAssignedPawn() != null && s.GetAssignedPawn() != pawn)
						{
							return false;
						}
						Room room3 = s.GetRoom(RegionType.Set_Passable);
						return (room3 == null || MeditationUtility.CanUseRoomToMeditate(room3, pawn)) && pawn.CanReserveAndReach(s, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false);
					});
				}
				foreach (Building t in source.Where(predicate))
				{
					yield return t;
					flag = true;
				}
				IEnumerator<Building> enumerator = null;
			}
			if (flag || !allowFallbackSpots)
			{
				yield break;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.MeditationFocus);
			foreach (Thing thing in list)
			{
				if (thing.def != ThingDefOf.Wall)
				{
					Room room = thing.GetRoom(RegionType.Set_Passable);
					if ((room == null || MeditationUtility.CanUseRoomToMeditate(room, pawn)) && thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) > 0f)
					{
						LocalTargetInfo localTargetInfo = MeditationUtility.MeditationSpotForFocus(thing, pawn, null);
						if (localTargetInfo.IsValid)
						{
							yield return localTargetInfo;
						}
					}
				}
			}
			List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
			Building_Bed bed = pawn.ownership.OwnedBed;
			Building_Bed building_Bed = bed;
			Room room2 = (building_Bed != null) ? building_Bed.GetRoom(RegionType.Set_Passable) : null;
			IntVec3 c2;
			if (room2 != null && !room2.PsychologicallyOutdoors && pawn.CanReserveAndReach(bed, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				foreach (LocalTargetInfo localTargetInfo2 in MeditationUtility.FocusSpotsInTheRoom(pawn, room2))
				{
					yield return localTargetInfo2;
				}
				IEnumerator<LocalTargetInfo> enumerator3 = null;
				c2 = RCellFinder.RandomWanderDestFor(pawn, bed.Position, MeditationUtility.WanderRadius, (Pawn p, IntVec3 c, IntVec3 r) => c.Standable(p.Map) && c.GetDoor(p.Map) == null && WanderRoomUtility.IsValidWanderDest(p, c, r), pawn.NormalMaxDanger());
				if (c2.IsValid)
				{
					yield return c2;
				}
			}
			if (pawn.IsPrisonerOfColony)
			{
				yield break;
			}
			IntVec3 colonyWanderRoot = WanderUtility.GetColonyWanderRoot(pawn);
			c2 = RCellFinder.RandomWanderDestFor(pawn, colonyWanderRoot, MeditationUtility.WanderRadius, delegate(Pawn p, IntVec3 c, IntVec3 r)
			{
				if (!c.Standable(p.Map) || c.GetDoor(p.Map) != null || !p.CanReserveAndReach(c, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false))
				{
					return false;
				}
				Room room3 = c.GetRoom(p.Map, RegionType.Set_Passable);
				return room3 == null || MeditationUtility.CanUseRoomToMeditate(room3, pawn);
			}, pawn.NormalMaxDanger());
			if (c2.IsValid)
			{
				yield return c2;
			}
			yield break;
			yield break;
		}

		
		public static bool SafeEnvironmentalConditions(Pawn pawn, IntVec3 cell, Map map)
		{
			return (!map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) || cell.Roofed(map)) && cell.GetDangerFor(pawn, map) == Danger.None;
		}

		
		public static bool CanMeditateNow(Pawn pawn)
		{
			return (pawn.needs.rest == null || pawn.needs.rest.CurCategory < RestCategory.VeryTired) && !pawn.needs.food.Starving && pawn.Awake() && pawn.health.hediffSet.BleedRateTotal <= 0f;
		}

		
		public static LocalTargetInfo BestFocusAt(LocalTargetInfo spot, Pawn pawn)
		{
			float num = 0f;
			LocalTargetInfo result = LocalTargetInfo.Invalid;
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(spot.Cell, pawn.MapHeld, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (GenSight.LineOfSightToThing(spot.Cell, thing, pawn.Map, false, null) && !(thing is Building_Throne))
				{
					CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
					if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn))
					{
						float statValueForPawn = thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
						if (statValueForPawn > num)
						{
							result = thing;
							num = statValueForPawn;
						}
					}
				}
			}
			return result;
		}

		
		public static IEnumerable<LocalTargetInfo> FocusSpotsInTheRoom(Pawn pawn, Room r)
		{
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn) && !(thing is Building_Throne) && thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) > 0f)
				{
					LocalTargetInfo localTargetInfo = MeditationUtility.MeditationSpotForFocus(thing, pawn, new Func<IntVec3, bool>(r.ContainsCell));
					if (localTargetInfo.IsValid)
					{
						yield return localTargetInfo;
					}
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		
		public static LocalTargetInfo MeditationSpotForFocus(Thing t, Pawn p, Func<IntVec3, bool> validator = null)
		{
			return (from cell in t.OccupiedRect().ExpandedBy(2).AdjacentCellsCardinal
			where (validator == null || validator(cell)) && !cell.IsForbidden(p) && p.CanReserveAndReach(cell, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false) && cell.Standable(p.Map)
			select cell).RandomElementWithFallback(IntVec3.Invalid);
		}

		
		public static IEnumerable<MeditationFocusDef> FocusTypesAvailableForPawn(Pawn pawn)
		{
			int num;
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i = num + 1)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					yield return meditationFocusDef;
				}
				num = i;
			}
			yield break;
		}

		
		public static string FocusTypesAvailableForPawnString(Pawn pawn)
		{
			return (from f in MeditationUtility.FocusTypesAvailableForPawn(pawn)
			select f.label).ToCommaList(false);
		}

		
		public static string FocusTypeAvailableExplanation(Pawn pawn)
		{
			string text = "";
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i++)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					text = string.Concat(new string[]
					{
						text,
						"MeditationFocusCanUse".Translate(meditationFocusDef.label).RawText,
						":\n",
						meditationFocusDef.EnablingThingsExplanation(pawn),
						"\n\n"
					});
					if (!MeditationUtility.focusObjectsPerTypeCache.ContainsKey(meditationFocusDef))
					{
						List<string> list = new List<string>();
						foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
						{
							CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
							if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
							{
								list.AddDistinct(thingDef.label);
							}
						}
						MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] = list.ToLineList("  - ", true);
					}
					text += "MeditationFocusObjects".Translate(meditationFocusDef.label).CapitalizeFirst() + ":\n" + MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] + "\n\n";
				}
			}
			return text;
		}

		
		public static void DrawMeditationSpotOverlay(IntVec3 center, Map map)
		{
			GenDraw.DrawRadiusRing(center, MeditationUtility.FocusObjectSearchRadius);
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(center, map, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (!(thing is Building_Throne) && thing.def != ThingDefOf.Wall && thing.TryGetComp<CompMeditationFocus>() != null && GenSight.LineOfSight(center, thing.Position, map, false, null, 0, 0))
				{
					GenDraw.DrawLineBetween(center.ToVector3() + new Vector3(0.5f, 0f, 0.5f), thing.TrueCenter(), SimpleColor.White);
				}
			}
		}

		
		public static bool CanUseRoomToMeditate(Room r, Pawn p)
		{
			return (r.Owners.EnumerableNullOrEmpty<Pawn>() || r.Owners.Contains(p)) && (!r.isPrisonCell || p.IsPrisoner);
		}

		
		public static IEnumerable<Thing> GetMeditationFociAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			if (!MeditationUtility.CountsAsArtificialBuilding(def, faction))
			{
				yield break;
			}
			foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MeditationFocus)))
			{
				CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.WillBeAffectedBy(def, faction, pos, rotation))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		
		public static void DrawMeditationFociAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			int num = 0;
			foreach (Thing thing in MeditationUtility.GetMeditationFociAffectedByBuilding(map, def, faction, pos, rotation))
			{
				if (num++ > 10)
				{
					break;
				}
				CompToggleDrawAffectedMeditationFoci compToggleDrawAffectedMeditationFoci = thing.TryGetComp<CompToggleDrawAffectedMeditationFoci>();
				if (compToggleDrawAffectedMeditationFoci == null || compToggleDrawAffectedMeditationFoci.Enabled)
				{
					GenAdj.OccupiedRect(pos, rotation, def.size);
					GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), thing.TrueCenter(), SimpleColor.Red);
				}
			}
		}

		
		public static bool CountsAsArtificialBuilding(ThingDef def, Faction faction)
		{
			return typeof(Building).IsAssignableFrom(def.thingClass) && faction != null && def.building.artificialForMeditationPurposes;
		}

		
		public static bool CountsAsArtificialBuilding(Thing t)
		{
			return MeditationUtility.CountsAsArtificialBuilding(t.def, t.Faction);
		}

		
		public static void DrawArtificialBuildingOverlay(IntVec3 pos, ThingDef def, Map map, float radius)
		{
			GenDraw.DrawRadiusRing(pos, radius);
			int num = 0;
			foreach (Thing t in map.listerArtificialBuildingsForMeditation.GetForCell(pos, radius))
			{
				if (num++ > 10)
				{
					break;
				}
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, Rot4.North, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red);
			}
		}

		
		public static float PsyfocusGainPerTick(Pawn pawn, Thing focus = null)
		{
			float num = pawn.GetStatValue(StatDefOf.MeditationFocusGain, true);
			if (focus != null && !focus.Destroyed)
			{
				num += focus.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
			}
			return num / 60000f;
		}

		
		public static void CheckMeditationScheduleTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead || !pawn.Spawned || !pawn.HasPsylink)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer || pawn.IsQuestLodger())
			{
				return;
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationSchedule, pawn, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationDesiredPsyfocus, pawn, OpportunityType.GoodToKnow);
		}

		
		public static float FocusObjectSearchRadius = 3.9f;

		
		private static float WanderRadius = 10f;

		
		private static Dictionary<MeditationFocusDef, string> focusObjectsPerTypeCache = new Dictionary<MeditationFocusDef, string>();
	}
}
