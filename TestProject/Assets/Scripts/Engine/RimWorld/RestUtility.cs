using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007BE RID: 1982
	public static class RestUtility
	{
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003358 RID: 13144 RVA: 0x0011C8B1 File Offset: 0x0011AAB1
		public static List<ThingDef> AllBedDefBestToWorst
		{
			get
			{
				return RestUtility.bedDefsBestToWorst_RestEffectiveness;
			}
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x0011C8B8 File Offset: 0x0011AAB8
		public static void Reset()
		{
			RestUtility.bedDefsBestToWorst_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x0011C9E0 File Offset: 0x0011ABE0
		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, bool ignoreOtherReservations = false)
		{
			Building_Bed building_Bed = bedThing as Building_Bed;
			if (building_Bed == null)
			{
				return false;
			}
			if (!traveler.CanReserveAndReach(building_Bed, PathEndMode.OnCell, Danger.Some, building_Bed.SleepingSlotsCount, -1, null, ignoreOtherReservations))
			{
				return false;
			}
			if (traveler.HasReserved(building_Bed, new LocalTargetInfo?(sleeper), null, null))
			{
				return false;
			}
			if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
			{
				return false;
			}
			if (!building_Bed.AnyUnoccupiedSleepingSlot && (!sleeper.InBed() || sleeper.CurrentBed() != building_Bed) && !building_Bed.CompAssignableToPawn.AssignedPawns.Contains(sleeper))
			{
				return false;
			}
			if (building_Bed.IsForbidden(traveler))
			{
				return false;
			}
			if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, sleeperWillBePrisoner, false))
			{
				return false;
			}
			if (building_Bed.IsBurning())
			{
				return false;
			}
			if (sleeperWillBePrisoner)
			{
				if (!building_Bed.ForPrisoners)
				{
					return false;
				}
				if (!building_Bed.Position.IsInPrisonCell(building_Bed.Map))
				{
					return false;
				}
			}
			else
			{
				if (building_Bed.Faction != traveler.Faction && (traveler.HostFaction == null || building_Bed.Faction != traveler.HostFaction))
				{
					return false;
				}
				if (building_Bed.ForPrisoners)
				{
					return false;
				}
			}
			if (building_Bed.Medical)
			{
				if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(sleeper))
				{
					return false;
				}
				if (!HealthAIUtility.ShouldSeekMedicalRest(sleeper))
				{
					return false;
				}
			}
			else if (building_Bed.OwnersForReading.Any<Pawn>() && !building_Bed.OwnersForReading.Contains(sleeper))
			{
				if (sleeper.IsPrisoner || sleeperWillBePrisoner)
				{
					if (!building_Bed.AnyUnownedSleepingSlot)
					{
						return false;
					}
				}
				else
				{
					if (!RestUtility.IsAnyOwnerLovePartnerOf(building_Bed, sleeper))
					{
						return false;
					}
					if (!building_Bed.AnyUnownedSleepingSlot)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x0011CB60 File Offset: 0x0011AD60
		private static bool IsAnyOwnerLovePartnerOf(Building_Bed bed, Pawn sleeper)
		{
			for (int i = 0; i < bed.OwnersForReading.Count; i++)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(sleeper, bed.OwnersForReading[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x0011CB9A File Offset: 0x0011AD9A
		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, p.IsPrisoner, true, false);
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x0011CBAC File Offset: 0x0011ADAC
		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool ignoreOtherReservations = false)
		{
			if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
			{
				if (sleeper.InBed() && sleeper.CurrentBed().Medical && RestUtility.IsValidBedFor(sleeper.CurrentBed(), sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations))
				{
					return sleeper.CurrentBed();
				}
				for (int i = 0; i < RestUtility.bedDefsBestToWorst_Medical.Count; i++)
				{
					ThingDef thingDef = RestUtility.bedDefsBestToWorst_Medical[i];
					if (RestUtility.CanUseBedEver(sleeper, thingDef))
					{
						for (int j = 0; j < 2; j++)
						{
							Danger maxDanger = (j == 0) ? Danger.None : Danger.Deadly;
							Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing b) => ((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations), null, 0, -1, false, RegionType.Set_Passable, false);
							if (building_Bed != null)
							{
								return building_Bed;
							}
						}
					}
				}
			}
			if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null && RestUtility.IsValidBedFor(sleeper.ownership.OwnedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations))
			{
				return sleeper.ownership.OwnedBed;
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null && RestUtility.IsValidBedFor(ownedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations))
				{
					return ownedBed;
				}
			}
			for (int k = 0; k < 2; k++)
			{
				Danger maxDanger = (k == 0) ? Danger.None : Danger.Deadly;
				Predicate<Thing> <>9__1;
				for (int l = 0; l < RestUtility.bedDefsBestToWorst_RestEffectiveness.Count; l++)
				{
					ThingDef thingDef2 = RestUtility.bedDefsBestToWorst_RestEffectiveness[l];
					if (RestUtility.CanUseBedEver(sleeper, thingDef2))
					{
						IntVec3 position = sleeper.Position;
						Map map = sleeper.Map;
						ThingRequest thingReq = ThingRequest.ForDef(thingDef2);
						PathEndMode peMode = PathEndMode.OnCell;
						TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false);
						float maxDistance = 9999f;
						Predicate<Thing> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((Thing b) => !((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations)));
						}
						Building_Bed building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false);
						if (building_Bed2 != null)
						{
							return building_Bed2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x0011CED4 File Offset: 0x0011B0D4
		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> medBedValidator = delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return building_Bed2 != null && (building_Bed2.Medical || !building_Bed2.def.building.bed_humanlike) && RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, pawn.IsPrisoner, false, true, false);
			};
			if (pawn.InBed() && medBedValidator(pawn.CurrentBed()))
			{
				return pawn.CurrentBed();
			}
			for (int i = 0; i < 2; i++)
			{
				Danger maxDanger = (i == 0) ? Danger.None : Danger.Deadly;
				Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Bed), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing b) => b.Position.GetDangerFor(pawn, pawn.Map) <= maxDanger && medBedValidator(b), null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_Bed != null)
				{
					return building_Bed;
				}
			}
			return RestUtility.FindBedFor(pawn);
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x0011CFCC File Offset: 0x0011B1CC
		public static IntVec3 GetBedSleepingSlotPosFor(Pawn pawn, Building_Bed bed)
		{
			for (int i = 0; i < bed.OwnersForReading.Count; i++)
			{
				if (bed.OwnersForReading[i] == pawn)
				{
					return bed.GetSleepingSlotPos(i);
				}
			}
			for (int j = 0; j < bed.SleepingSlotsCount; j++)
			{
				Pawn curOccupant = bed.GetCurOccupant(j);
				if ((j >= bed.OwnersForReading.Count || bed.OwnersForReading[j] == null) && curOccupant == pawn)
				{
					return bed.GetSleepingSlotPos(j);
				}
			}
			for (int k = 0; k < bed.SleepingSlotsCount; k++)
			{
				Pawn curOccupant2 = bed.GetCurOccupant(k);
				if ((k >= bed.OwnersForReading.Count || bed.OwnersForReading[k] == null) && curOccupant2 == null)
				{
					return bed.GetSleepingSlotPos(k);
				}
			}
			Log.Error("Could not find good sleeping slot position for " + pawn + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.", false);
			return bed.GetSleepingSlotPos(0);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x0011D0A6 File Offset: 0x0011B2A6
		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			return p.BodySize <= bedDef.building.bed_maxBodySize && p.RaceProps.Humanlike == bedDef.building.bed_humanlike;
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x0011D0D8 File Offset: 0x0011B2D8
		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			return pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest && pawn.needs.rest.CurLevel >= 0.2f;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x0011D10E File Offset: 0x0011B30E
		public static bool DisturbancePreventsLyingDown(Pawn pawn)
		{
			return !pawn.Downed && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x0011D138 File Offset: 0x0011B338
		public static bool Awake(this Pawn p)
		{
			return p.health.capacities.CanBeAwake && (!p.Spawned || p.CurJob == null || p.jobs.curDriver == null || !p.jobs.curDriver.asleep);
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x0011D190 File Offset: 0x0011B390
		public static Building_Bed CurrentBed(this Pawn p)
		{
			if (!p.Spawned || p.CurJob == null || p.GetPosture() != PawnPosture.LayingInBed)
			{
				return null;
			}
			Building_Bed building_Bed = null;
			List<Thing> thingList = p.Position.GetThingList(p.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				building_Bed = (thingList[i] as Building_Bed);
				if (building_Bed != null)
				{
					break;
				}
			}
			if (building_Bed == null)
			{
				return null;
			}
			for (int j = 0; j < building_Bed.SleepingSlotsCount; j++)
			{
				if (building_Bed.GetCurOccupant(j) == p)
				{
					return building_Bed;
				}
			}
			return null;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x0011D210 File Offset: 0x0011B410
		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x0011D21C File Offset: 0x0011B41C
		public static void WakeUp(Pawn p)
		{
			if (p.CurJob != null && (p.GetPosture().Laying() || p.CurJobDef == JobDefOf.LayDown) && !p.Downed)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			CompCanBeDormant comp = p.GetComp<CompCanBeDormant>();
			if (comp == null)
			{
				return;
			}
			comp.WakeUpWithDelay();
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x0011D274 File Offset: 0x0011B474
		public static float WakeThreshold(Pawn p)
		{
			Lord lord = p.GetLord();
			if (lord != null && lord.CurLordToil != null && lord.CurLordToil.CustomWakeThreshold != null)
			{
				return lord.CurLordToil.CustomWakeThreshold.Value;
			}
			return 1f;
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x0011D2C1 File Offset: 0x0011B4C1
		public static float FallAsleepMaxLevel(Pawn p)
		{
			return Mathf.Min(0.75f, RestUtility.WakeThreshold(p) - 0.01f);
		}

		// Token: 0x04001BA1 RID: 7073
		private static List<ThingDef> bedDefsBestToWorst_RestEffectiveness;

		// Token: 0x04001BA2 RID: 7074
		private static List<ThingDef> bedDefsBestToWorst_Medical;
	}
}
