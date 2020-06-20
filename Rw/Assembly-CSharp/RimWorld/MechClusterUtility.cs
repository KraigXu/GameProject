﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AA2 RID: 2722
	public static class MechClusterUtility
	{
		// Token: 0x06004015 RID: 16405 RVA: 0x00155DAC File Offset: 0x00153FAC
		public static IntVec3 FindClusterPosition(Map map, MechClusterSketch sketch, int maxTries = 100, float spawnCloseToColonyChance = 0f)
		{
			IntVec3 result = IntVec3.Invalid;
			float num = float.MinValue;
			if (Rand.Chance(spawnCloseToColonyChance))
			{
				int num2 = 0;
				IntVec3 intVec;
				while (num2 < 20 && DropCellFinder.TryFindRaidDropCenterClose(out intVec, map, true, true, false, 40))
				{
					float clusterPositionScore = MechClusterUtility.GetClusterPositionScore(intVec, map, sketch);
					if (clusterPositionScore >= 100f || Mathf.Approximately(clusterPositionScore, 100f))
					{
						return intVec;
					}
					if (clusterPositionScore > num)
					{
						result = intVec;
						num = clusterPositionScore;
					}
					num2++;
				}
			}
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < maxTries; i++)
			{
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 x) => x.Standable(map)));
				}
				IntVec3 intVec2 = CellFinderLoose.RandomCellWith(validator, map, 1000);
				if (intVec2.IsValid)
				{
					IntVec3 intVec3 = RCellFinder.FindSiegePositionFrom_NewTemp(intVec2, map, false);
					if (intVec3.IsValid)
					{
						float clusterPositionScore2 = MechClusterUtility.GetClusterPositionScore(intVec3, map, sketch);
						if (clusterPositionScore2 >= 100f || Mathf.Approximately(clusterPositionScore2, 100f))
						{
							return intVec3;
						}
						if (clusterPositionScore2 > num)
						{
							result = intVec2;
							num = clusterPositionScore2;
						}
					}
				}
			}
			if (!result.IsValid)
			{
				return CellFinder.RandomCell(map);
			}
			return result;
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x00155EE8 File Offset: 0x001540E8
		public static float GetClusterPositionScore(IntVec3 center, Map map, MechClusterSketch sketch)
		{
			MechClusterUtility.<>c__DisplayClass9_0 <>c__DisplayClass9_;
			<>c__DisplayClass9_.map = map;
			if (sketch.buildingsSketch.AnyThingOutOfBounds(<>c__DisplayClass9_.map, center, Sketch.SpawnPosType.Unchanged))
			{
				return -100f;
			}
			if (sketch.pawns != null)
			{
				for (int i = 0; i < sketch.pawns.Count; i++)
				{
					if (!(sketch.pawns[i].position + center).InBounds(<>c__DisplayClass9_.map))
					{
						return -100f;
					}
				}
			}
			<>c__DisplayClass9_.colonyBuildings = <>c__DisplayClass9_.map.listerBuildings.allBuildingsColonist;
			<>c__DisplayClass9_.colonists = <>c__DisplayClass9_.map.mapPawns.FreeColonistsAndPrisonersSpawned;
			int num = 0;
			<>c__DisplayClass9_.fogged = 0;
			<>c__DisplayClass9_.roofed = 0;
			<>c__DisplayClass9_.indoors = 0;
			<>c__DisplayClass9_.tooCloseToColony = false;
			List<SketchEntity> entities = sketch.buildingsSketch.Entities;
			for (int j = 0; j < entities.Count; j++)
			{
				if (entities[j].IsSpawningBlocked(center, <>c__DisplayClass9_.map, null, false))
				{
					num++;
				}
				MechClusterUtility.<GetClusterPositionScore>g__CheckCell|9_0(entities[j].pos + center, ref <>c__DisplayClass9_);
			}
			if (sketch.pawns != null)
			{
				for (int k = 0; k < sketch.pawns.Count; k++)
				{
					if (!(sketch.pawns[k].position + center).Standable(<>c__DisplayClass9_.map))
					{
						num++;
					}
					MechClusterUtility.<GetClusterPositionScore>g__CheckCell|9_0(sketch.pawns[k].position + center, ref <>c__DisplayClass9_);
				}
			}
			int num2 = sketch.buildingsSketch.Entities.Count + ((sketch.pawns != null) ? sketch.pawns.Count : 0);
			float num3 = (float)num / (float)num2;
			float num4 = (float)<>c__DisplayClass9_.fogged / (float)num2;
			float a = (float)<>c__DisplayClass9_.roofed / (float)num2;
			float b = (float)<>c__DisplayClass9_.indoors / (float)num2;
			return 100f * (1f - num3) * (1f - Mathf.Max(a, b)) * (1f - num4) * (<>c__DisplayClass9_.tooCloseToColony ? 0.5f : 1f);
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x00010306 File Offset: 0x0000E506
		[Obsolete]
		public static bool CanSpawnClusterAt(MechClusterSketch sketch, IntVec3 center, Map map, bool desperate = false)
		{
			return false;
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x000F4A48 File Offset: 0x000F2C48
		[Obsolete]
		public static IntVec3 FindDropPodLocation(Map map, Predicate<IntVec3> validator, int maxTries = 100, float spawnCloseToColonyChance = 0f)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00156105 File Offset: 0x00154305
		[Obsolete]
		private static bool TryFindRaidDropCenterClose(out IntVec3 result, Map map, Predicate<IntVec3> validator, int maxTries = 100)
		{
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x000F4A48 File Offset: 0x000F2C48
		[Obsolete]
		public static IntVec3 TryFindMechClusterPosInRect(CellRect rect, Map map, MechClusterSketch sketch)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x00156114 File Offset: 0x00154314
		public static List<Thing> SpawnCluster(IntVec3 center, Map map, MechClusterSketch sketch, bool dropInPods = true, bool canAssaultColony = false, string questTag = null)
		{
			List<Thing> spawnedThings = new List<Thing>();
			Sketch.SpawnMode spawnMode = dropInPods ? Sketch.SpawnMode.TransportPod : Sketch.SpawnMode.Normal;
			sketch.buildingsSketch.Spawn(map, center, Faction.OfMechanoids, Sketch.SpawnPosType.Unchanged, spawnMode, false, false, spawnedThings, sketch.startDormant, false, null, delegate(IntVec3 spot, SketchEntity entity)
			{
				SketchThing sketchThing;
				if ((sketchThing = (entity as SketchThing)) == null || sketchThing.def == ThingDefOf.Wall || sketchThing.def == ThingDefOf.Barricade)
				{
					return;
				}
				entity.SpawnNear(spot, map, 12f, Faction.OfMechanoids, spawnMode, false, spawnedThings, sketch.startDormant);
			});
			float defendRadius = Mathf.Sqrt((float)(sketch.buildingsSketch.OccupiedSize.x * sketch.buildingsSketch.OccupiedSize.x + sketch.buildingsSketch.OccupiedSize.z * sketch.buildingsSketch.OccupiedSize.z)) / 2f + 6f;
			LordJob_MechanoidDefendBase lordJob_MechanoidDefendBase;
			if (sketch.startDormant)
			{
				lordJob_MechanoidDefendBase = new LordJob_SleepThenMechanoidsDefend(spawnedThings, Faction.OfMechanoids, defendRadius, center, canAssaultColony, true);
			}
			else
			{
				lordJob_MechanoidDefendBase = new LordJob_MechanoidsDefend(spawnedThings, Faction.OfMechanoids, defendRadius, center, canAssaultColony, true);
			}
			Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob_MechanoidDefendBase, map, null);
			QuestUtility.AddQuestTag(lord, questTag);
			bool flag = Rand.Chance(0.6f);
			float randomInRange = MechClusterUtility.InitiationDelay.RandomInRange;
			int num = (int)(MechClusterUtility.MechAssemblerInitialDelayDays.RandomInRange * 60000f);
			for (int i = 0; i < spawnedThings.Count; i++)
			{
				Thing thing = spawnedThings[i];
				CompSpawnerPawn compSpawnerPawn = thing.TryGetComp<CompSpawnerPawn>();
				if (compSpawnerPawn != null)
				{
					compSpawnerPawn.CalculateNextPawnSpawnTick((float)num);
				}
				if (thing.TryGetComp<CompProjectileInterceptor>() != null)
				{
					lordJob_MechanoidDefendBase.AddThingToNotifyOnDefeat(thing);
				}
				if (flag)
				{
					CompInitiatable compInitiatable = thing.TryGetComp<CompInitiatable>();
					if (compInitiatable != null)
					{
						compInitiatable.initiationDelayTicksOverride = (int)(60000f * randomInRange);
					}
				}
				Building b;
				if ((b = (thing as Building)) != null && MechClusterUtility.IsBuildingThreat(b))
				{
					lord.AddBuilding(b);
				}
				thing.SetFaction(Faction.OfMechanoids, null);
			}
			if (!sketch.pawns.NullOrEmpty<MechClusterSketch.Mech>())
			{
				Predicate<IntVec3> <>9__1;
				foreach (MechClusterSketch.Mech mech in sketch.pawns)
				{
					IntVec3 intVec = mech.position + center;
					if (!intVec.Standable(map))
					{
						IntVec3 root = intVec;
						Map map2 = map;
						int squareRadius = 12;
						Predicate<IntVec3> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 x) => x.Standable(map)));
						}
						if (!CellFinder.TryFindRandomCellNear(root, map2, squareRadius, validator, out intVec, -1))
						{
							continue;
						}
					}
					Pawn pawn = PawnGenerator.GeneratePawn(mech.kindDef, Faction.OfMechanoids);
					CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						if (sketch.startDormant)
						{
							compCanBeDormant.ToSleep();
						}
						else
						{
							compCanBeDormant.WakeUp();
						}
					}
					lord.AddPawn(pawn);
					spawnedThings.Add(pawn);
					if (dropInPods)
					{
						ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
						activeDropPodInfo.innerContainer.TryAdd(pawn, 1, true);
						activeDropPodInfo.openDelay = 60;
						activeDropPodInfo.leaveSlag = false;
						activeDropPodInfo.despawnPodBeforeSpawningThing = true;
						activeDropPodInfo.spawnWipeMode = new WipeMode?(WipeMode.Vanish);
						DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);
					}
					else
					{
						GenSpawn.Spawn(pawn, intVec, map, WipeMode.Vanish);
					}
				}
			}
			foreach (Thing thing2 in spawnedThings)
			{
				if (!sketch.startDormant)
				{
					CompWakeUpDormant compWakeUpDormant = thing2.TryGetComp<CompWakeUpDormant>();
					if (compWakeUpDormant != null)
					{
						compWakeUpDormant.Activate(true, true);
					}
				}
			}
			return spawnedThings;
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00156510 File Offset: 0x00154710
		private static bool IsBuildingThreat(Thing b)
		{
			CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = b.TryGetComp<CompPawnSpawnOnWakeup>();
			if (compPawnSpawnOnWakeup != null && compPawnSpawnOnWakeup.CanSpawn)
			{
				return true;
			}
			CompSpawnerPawn compSpawnerPawn = b.TryGetComp<CompSpawnerPawn>();
			return (compSpawnerPawn != null && compSpawnerPawn.pawnsLeftToSpawn != 0) || b.def.building.IsTurret || b.TryGetComp<CompCauseGameCondition>() != null;
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x00156564 File Offset: 0x00154764
		public static bool AnyThreatBuilding(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Thing thing = things[i];
				if (thing is Building && !thing.Destroyed && MechClusterUtility.IsBuildingThreat(thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002549 RID: 9545
		private const int CloseToColonyRadius = 40;

		// Token: 0x0400254A RID: 9546
		private const int MinDistanceToColony = 5;

		// Token: 0x0400254B RID: 9547
		private const float RetrySpawnEntityRadius = 12f;

		// Token: 0x0400254C RID: 9548
		private const float MaxClusterPositionScore = 100f;

		// Token: 0x0400254D RID: 9549
		private const float InitiationChance = 0.6f;

		// Token: 0x0400254E RID: 9550
		private static readonly FloatRange InitiationDelay = new FloatRange(0.1f, 15f);

		// Token: 0x0400254F RID: 9551
		private static readonly FloatRange MechAssemblerInitialDelayDays = new FloatRange(0.5f, 1.5f);

		// Token: 0x04002550 RID: 9552
		public const string DefeatedSignal = "MechClusterDefeated";
	}
}
