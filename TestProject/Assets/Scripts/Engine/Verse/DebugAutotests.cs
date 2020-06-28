using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200033A RID: 826
	public static class DebugAutotests
	{
		// Token: 0x0600185E RID: 6238 RVA: 0x0008C076 File Offset: 0x0008A276
		[DebugAction("Autotests", "Make colony (full)", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyFull()
		{
			Autotests_ColonyMaker.MakeColony_Full();
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0008C07D File Offset: 0x0008A27D
		[DebugAction("Autotests", "Make colony (animals)", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyAnimals()
		{
			Autotests_ColonyMaker.MakeColony_Animals();
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0008C084 File Offset: 0x0008A284
		[DebugAction("Autotests", "Test force downed x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceDownedx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDowned(pawn, true);
				if (pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died while force downing: ",
						pawn,
						" at ",
						pawn.Position
					}), false);
					return;
				}
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0008C138 File Offset: 0x0008A338
		[DebugAction("Autotests", "Test force kill x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceKillx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDead(pawn);
				if (!pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died not die: ",
						pawn,
						" at ",
						pawn.Position
					}), false);
					return;
				}
			}
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0008C1EC File Offset: 0x0008A3EC
		[DebugAction("Autotests", "Test Surgery fail catastrophic x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestSurgeryFailCatastrophicx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				pawn.health.forceIncap = true;
				BodyPartRecord part = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).RandomElement<BodyPartRecord>();
				HealthUtility.GiveInjuriesOperationFailureCatastrophic(pawn, part);
				pawn.health.forceIncap = false;
				if (pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died: ",
						pawn,
						" at ",
						pawn.Position
					}), false);
				}
			}
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0008C2D4 File Offset: 0x0008A4D4
		[DebugAction("Autotests", "Test Surgery fail ridiculous x100", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestSurgeryFailRidiculousx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				pawn.health.forceIncap = true;
				HealthUtility.GiveInjuriesOperationFailureRidiculous(pawn);
				pawn.health.forceIncap = false;
				if (pawn.Dead)
				{
					Log.Error(string.Concat(new object[]
					{
						"Pawn died: ",
						pawn,
						" at ",
						pawn.Position
					}), false);
				}
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0008C3A0 File Offset: 0x0008A5A0
		[DebugAction("Autotests", "Test generate pawn x1000", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestGeneratePawnx1000()
		{
			float[] array = new float[]
			{
				10f,
				20f,
				50f,
				100f,
				200f,
				500f,
				1000f,
				2000f,
				5000f,
				1E+20f
			};
			int[] array2 = new int[array.Length];
			for (int i = 0; i < 1000; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				PerfLogger.Reset();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
				float ms = PerfLogger.Duration() * 1000f;
				array2[array.FirstIndexOf((float time) => ms <= time)]++;
				if (pawn.Dead)
				{
					Log.Error("Pawn is dead", false);
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Pawn creation time histogram:");
			for (int j = 0; j < array2.Length; j++)
			{
				stringBuilder.AppendLine(string.Format("<{0}ms: {1}", array[j], array2[j]));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x0008C4A0 File Offset: 0x0008A6A0
		[DebugAction("Autotests", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckRegionListers()
		{
			Autotests_RegionListers.CheckBugs(Find.CurrentMap);
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0008C4AC File Offset: 0x0008A6AC
		[DebugAction("Autotests", "Test time-to-down", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestTimeToDown()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<PawnKindDef> enumerator = (from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kindDef = enumerator.Current;
					list.Add(new DebugMenuOption(kindDef.label, DebugMenuOptionMode.Action, delegate
					{
						if (kindDef == PawnKindDefOf.Colonist)
						{
							Log.Message("Current colonist TTD reference point: 22.3 seconds, stddev 8.35 seconds", false);
						}
						List<float> results = new List<float>();
						List<PawnKindDef> list2 = new List<PawnKindDef>();
						List<PawnKindDef> list3 = new List<PawnKindDef>();
						list2.Add(kindDef);
						list3.Add(kindDef);
						ArenaUtility.BeginArenaFightSet(1000, list2, list3, delegate(ArenaUtility.ArenaResult result)
						{
							if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
							{
								results.Add(result.tickDuration.TicksToSeconds());
							}
						}, delegate
						{
							string format = "Finished {0} tests; time-to-down {1}, stddev {2}\n\nraw: {3}";
							object[] array = new object[4];
							array[0] = results.Count;
							array[1] = results.Average();
							array[2] = GenMath.Stddev(results);
							array[3] = (from res in results
							select res.ToString()).ToLineList(null, false);
							Log.Message(string.Format(format, array), false);
						});
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x0008C55C File Offset: 0x0008A75C
		[DebugAction("Autotests", "Battle Royale All PawnKinds", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleAllPawnKinds()
		{
			ArenaUtility.PerformBattleRoyale(DefDatabase<PawnKindDef>.AllDefs);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0008C568 File Offset: 0x0008A768
		[DebugAction("Autotests", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleHumanlikes()
		{
			ArenaUtility.PerformBattleRoyale(from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			select k);
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0008C598 File Offset: 0x0008A798
		[DebugAction("Autotests", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleByDamagetype()
		{
			PawnKindDef[] array = new PawnKindDef[]
			{
				PawnKindDefOf.Colonist,
				PawnKindDefOf.Muffalo
			};
			IEnumerable<ToolCapacityDef> enumerable = from tc in DefDatabase<ToolCapacityDef>.AllDefsListForReading
			where tc != ToolCapacityDefOf.KickMaterialInEyes
			select tc;
			Func<PawnKindDef, ToolCapacityDef, string> func = (PawnKindDef pkd, ToolCapacityDef dd) => string.Format("{0}_{1}", pkd.label, dd.defName);
			if (DebugAutotests.pawnKindsForDamageTypeBattleRoyale == null)
			{
				DebugAutotests.pawnKindsForDamageTypeBattleRoyale = new List<PawnKindDef>();
				foreach (PawnKindDef pawnKindDef in array)
				{
					using (IEnumerator<ToolCapacityDef> enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ToolCapacityDef toolType = enumerator.Current;
							string text = func(pawnKindDef, toolType);
							ThingDef thingDef = Gen.MemberwiseClone<ThingDef>(pawnKindDef.race);
							thingDef.defName = text;
							thingDef.label = text;
							thingDef.tools = new List<Tool>(pawnKindDef.race.tools.Select(delegate(Tool tool)
							{
								Tool tool2 = Gen.MemberwiseClone<Tool>(tool);
								tool2.capacities = new List<ToolCapacityDef>();
								tool2.capacities.Add(toolType);
								return tool2;
							}));
							PawnKindDef pawnKindDef2 = Gen.MemberwiseClone<PawnKindDef>(pawnKindDef);
							pawnKindDef2.defName = text;
							pawnKindDef2.label = text;
							pawnKindDef2.race = thingDef;
							DebugAutotests.pawnKindsForDamageTypeBattleRoyale.Add(pawnKindDef2);
						}
					}
				}
			}
			ArenaUtility.PerformBattleRoyale(DebugAutotests.pawnKindsForDamageTypeBattleRoyale);
		}

		// Token: 0x04000F0D RID: 3853
		private static List<PawnKindDef> pawnKindsForDamageTypeBattleRoyale;
	}
}
