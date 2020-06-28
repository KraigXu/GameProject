using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020003F1 RID: 1009
	public static class BackCompatibility
	{
		// Token: 0x06001DF9 RID: 7673 RVA: 0x000B7E50 File Offset: 0x000B6050
		public static bool IsSaveCompatibleWith(string version)
		{
			if (VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) == VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) >= 1 && VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) <= VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) == 0 && VersionControl.CurrentMajor == 0)
			{
				int num = VersionControl.MinorFromVersionString(version);
				int currentMinor = VersionControl.CurrentMinor;
				for (int i = 0; i < BackCompatibility.SaveCompatibleMinorVersions.Length; i++)
				{
					if (BackCompatibility.SaveCompatibleMinorVersions[i].First == num && BackCompatibility.SaveCompatibleMinorVersions[i].Second == currentMinor)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x000B7EF8 File Offset: 0x000B60F8
		public static void PreLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PreLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PreLoadSavegame of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000B7F8C File Offset: 0x000B618C
		public static void PostLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostLoadSavegame of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000B8020 File Offset: 0x000B6220
		public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
			{
				return defName;
			}
			string text = defName;
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (Scribe.mode == LoadSaveMode.Inactive || BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						string text2 = BackCompatibility.conversionChain[i].BackCompatibleDefName(defType, text, forDefInjections, node);
						if (text2 != null)
						{
							text = text2;
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in BackCompatibleDefName of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
			return text;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000B80D8 File Offset: 0x000B62D8
		public static object BackCompatibleEnum(Type enumType, string enumName)
		{
			if (enumType == typeof(QualityCategory))
			{
				if (enumName == "Shoddy")
				{
					return QualityCategory.Poor;
				}
				if (enumName == "Superior")
				{
					return QualityCategory.Excellent;
				}
			}
			return null;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000B8118 File Offset: 0x000B6318
		public static Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, node);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in GetBackCompatibleType of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000B81C8 File Offset: 0x000B63C8
		public static int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						index = BackCompatibility.conversionChain[i].GetBackCompatibleBodyPartIndex(body, index);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in GetBackCompatibleBodyPartIndex of ",
							body,
							"\n",
							ex
						}), false);
					}
				}
			}
			return index;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000B8254 File Offset: 0x000B6454
		public static bool WasDefRemoved(string defName, Type type)
		{
			foreach (Tuple<string, Type> tuple in BackCompatibility.RemovedDefs)
			{
				if (tuple.Item1 == defName && tuple.Item2 == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000B82C4 File Offset: 0x000B64C4
		public static void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostExposeData(obj);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostExposeData of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000B8364 File Offset: 0x000B6564
		public static void PostCouldntLoadDef(string defName)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostCouldntLoadDef(defName);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostCouldntLoadDef of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000B83F8 File Offset: 0x000B65F8
		public static void PawnTrainingTrackerPostLoadInit(Pawn_TrainingTracker tracker, ref DefMap<TrainableDef, bool> wantedTrainables, ref DefMap<TrainableDef, int> steps, ref DefMap<TrainableDef, bool> learned)
		{
			if (wantedTrainables == null)
			{
				wantedTrainables = new DefMap<TrainableDef, bool>();
			}
			if (steps == null)
			{
				steps = new DefMap<TrainableDef, int>();
			}
			if (learned == null)
			{
				learned = new DefMap<TrainableDef, bool>();
			}
			if (tracker.GetSteps(TrainableDefOf.Tameness) == 0 && DefDatabase<TrainableDef>.AllDefsListForReading.Any((TrainableDef td) => tracker.GetSteps(td) != 0))
			{
				tracker.Train(TrainableDefOf.Tameness, null, true);
			}
			foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefsListForReading)
			{
				if (tracker.GetSteps(trainableDef) == trainableDef.steps)
				{
					tracker.Train(trainableDef, null, true);
				}
			}
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x000B84D0 File Offset: 0x000B66D0
		public static void TriggerDataFractionColonyDamageTakenNull(Trigger_FractionColonyDamageTaken trigger, Map map)
		{
			trigger.data = new TriggerData_FractionColonyDamageTaken();
			((TriggerData_FractionColonyDamageTaken)trigger.data).startColonyDamage = map.damageWatcher.DamageTakenEver;
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000B84F8 File Offset: 0x000B66F8
		public static void TriggerDataPawnCycleIndNull(Trigger_KidnapVictimPresent trigger)
		{
			trigger.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x000B8505 File Offset: 0x000B6705
		public static void TriggerDataTicksPassedNull(Trigger_TicksPassed trigger)
		{
			trigger.data = new TriggerData_TicksPassed();
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x000B8512 File Offset: 0x000B6712
		public static TerrainDef BackCompatibleTerrainWithShortHash(ushort hash)
		{
			if (hash == 16442)
			{
				return TerrainDefOf.WaterMovingChestDeep;
			}
			return null;
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000B8523 File Offset: 0x000B6723
		public static ThingDef BackCompatibleThingDefWithShortHash(ushort hash)
		{
			if (hash == 62520)
			{
				return ThingDefOf.MineableComponentsIndustrial;
			}
			return null;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000B8534 File Offset: 0x000B6734
		public static ThingDef BackCompatibleThingDefWithShortHash_Force(ushort hash, int major, int minor)
		{
			if (major == 0 && minor <= 18 && hash == 27292)
			{
				return ThingDefOf.MineableSteel;
			}
			return null;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000B8550 File Offset: 0x000B6750
		public static bool CheckSpawnBackCompatibleThingAfterLoading(Thing thing, Map map)
		{
			if (VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == 0 && VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) <= 18 && thing.stackCount > thing.def.stackLimit && thing.stackCount != 1 && thing.def.stackLimit != 1)
			{
				BackCompatibility.tmpThingsToSpawnLater.Add(thing);
				return true;
			}
			return false;
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000B85AF File Offset: 0x000B67AF
		public static void PreCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000B85BC File Offset: 0x000B67BC
		public static void PostCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			for (int i = 0; i < BackCompatibility.tmpThingsToSpawnLater.Count; i++)
			{
				GenPlace.TryPlaceThing(BackCompatibility.tmpThingsToSpawnLater[i], BackCompatibility.tmpThingsToSpawnLater[i].Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
			}
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000B8618 File Offset: 0x000B6818
		public static void FactionManagerPostLoadInit()
		{
			if (ModsConfig.RoyaltyActive && Find.FactionManager.FirstFactionOfDef(FactionDefOf.Empire) == null)
			{
				Faction faction = FactionGenerator.NewGeneratedFaction(FactionDefOf.Empire);
				Find.FactionManager.Add(faction);
			}
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000B8654 File Offset: 0x000B6854
		public static void ResearchManagerPostLoadInit()
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if ((allDefsListForReading[i].IsFinished || allDefsListForReading[i].ProgressReal > 0f) && allDefsListForReading[i].TechprintsApplied < allDefsListForReading[i].techprintCount)
				{
					Find.ResearchManager.AddTechprints(allDefsListForReading[i], allDefsListForReading[i].techprintCount - allDefsListForReading[i].TechprintsApplied);
				}
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000B86DD File Offset: 0x000B68DD
		public static void PrefsDataPostLoad(PrefsData prefsData)
		{
			if (prefsData.pauseOnUrgentLetter != null)
			{
				if (prefsData.pauseOnUrgentLetter.Value)
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.MajorThreat;
					return;
				}
				prefsData.automaticPauseMode = AutomaticPauseMode.Never;
			}
		}

		// Token: 0x04001264 RID: 4708
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[]
		{
			new Pair<int, int>(17, 18)
		};

		// Token: 0x04001265 RID: 4709
		private static List<BackCompatibilityConverter> conversionChain = new List<BackCompatibilityConverter>
		{
			new BackCompatibilityConverter_0_17_AndLower(),
			new BackCompatibilityConverter_0_18(),
			new BackCompatibilityConverter_0_19(),
			new BackCompatibilityConverter_1_0(),
			new BackCompatibilityConverter_Universal()
		};

		// Token: 0x04001266 RID: 4710
		private static readonly List<Tuple<string, Type>> RemovedDefs = new List<Tuple<string, Type>>
		{
			new Tuple<string, Type>("PsychicSilencer", typeof(ThingDef)),
			new Tuple<string, Type>("PsychicSilencer", typeof(HediffDef))
		};

		// Token: 0x04001267 RID: 4711
		private static List<Thing> tmpThingsToSpawnLater = new List<Thing>();
	}
}
