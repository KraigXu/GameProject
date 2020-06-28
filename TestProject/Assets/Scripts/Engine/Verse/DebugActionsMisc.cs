using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000334 RID: 820
	public static class DebugActionsMisc
	{
		// Token: 0x0600181E RID: 6174 RVA: 0x00089B48 File Offset: 0x00087D48
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllPlants()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				if (thing is Plant)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00089BB4 File Offset: 0x00087DB4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllThings()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00089C14 File Offset: 0x00087E14
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishAllResearch()
		{
			Find.ResearchManager.DebugSetAllProjectsFinished();
			Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00089C30 File Offset: 0x00087E30
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReplaceAllTradeShips()
		{
			Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
			for (int i = 0; i < 5; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.CurrentMap;
				IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
			}
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x00089C7C File Offset: 0x00087E7C
		[DebugAction("General", "Change weather...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeWeather()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WeatherDef localWeather2 in DefDatabase<WeatherDef>.AllDefs)
			{
				WeatherDef localWeather = localWeather2;
				list.Add(new DebugMenuOption(localWeather.LabelCap, DebugMenuOptionMode.Action, delegate
				{
					Find.CurrentMap.weatherManager.TransitionTo(localWeather);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00089D0C File Offset: 0x00087F0C
		[DebugAction("General", "Play song...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PlaySong()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SongDef localSong2 in DefDatabase<SongDef>.AllDefs)
			{
				SongDef localSong = localSong2;
				list.Add(new DebugMenuOption(localSong.defName, DebugMenuOptionMode.Action, delegate
				{
					Find.MusicManagerPlay.ForceStartSong(localSong, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00089D98 File Offset: 0x00087F98
		[DebugAction("General", "Play sound...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PlaySound()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SoundDef localSd2 in from s in DefDatabase<SoundDef>.AllDefs
			where !s.sustain
			select s)
			{
				SoundDef localSd = localSd2;
				list.Add(new DebugMenuOption(localSd.defName, DebugMenuOptionMode.Action, delegate
				{
					if (localSd.subSounds.Any((SubSoundDef sub) => sub.onCamera))
					{
						localSd.PlayOneShotOnCamera(null);
						return;
					}
					localSd.PlayOneShot(SoundInfo.InMap(new TargetInfo(Find.CameraDriver.MapPosition, Find.CurrentMap, false), MaintenanceType.None));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00089E48 File Offset: 0x00088048
		[DebugAction("General", "End game condition...", allowedGameStates = (AllowedGameStates.Playing | AllowedGameStates.IsCurrentlyOnMap | AllowedGameStates.HasGameCondition))]
		private static void EndGameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameCondition localMc2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
			{
				GameCondition localMc = localMc2;
				list.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, delegate
				{
					localMc.End();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x00089EE4 File Offset: 0x000880E4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddPrisoner()
		{
			DebugActionsMisc.AddGuest(true);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00089EEC File Offset: 0x000880EC
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddGuest()
		{
			DebugActionsMisc.AddGuest(false);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x00089EF4 File Offset: 0x000880F4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyAssault()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				LordToil_Stage lordToil_Stage = lord.CurLordToil as LordToil_Stage;
				if (lordToil_Stage != null)
				{
					foreach (Transition transition in lord.Graph.transitions)
					{
						if (transition.sources.Contains(lordToil_Stage) && transition.target is LordToil_AssaultColony)
						{
							Messages.Message("Debug forcing to assault toil: " + lord.faction, MessageTypeDefOf.TaskCompletion, false);
							lord.GotoToil(transition.target);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x00089FE8 File Offset: 0x000881E8
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceEnemyFlee()
		{
			foreach (Lord lord in Find.CurrentMap.lordManager.lords)
			{
				if (lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer) && lord.faction.def.autoFlee)
				{
					LordToil lordToil = lord.Graph.lordToils.FirstOrDefault((LordToil st) => st is LordToil_PanicFlee);
					if (lordToil != null)
					{
						lord.GotoToil(lordToil);
					}
				}
			}
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0008A0A4 File Offset: 0x000882A4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AdaptionProgress10Days()
		{
			Find.StoryWatcher.watcherAdaptation.Debug_OffsetAdaptDays(10f);
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0008A0BA File Offset: 0x000882BA
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UnloadUnusedAssets()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0008A0C4 File Offset: 0x000882C4
		[DebugAction("General", "Name settlement...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void NameSettlement()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, delegate
			{
				Find.WindowStack.Add(new Dialog_NamePlayerFaction());
			}));
			if (Find.CurrentMap != null && Find.CurrentMap.IsPlayerHome && Find.CurrentMap.Parent is Settlement)
			{
				Settlement factionBase = (Settlement)Find.CurrentMap.Parent;
				list.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, delegate
				{
					Find.WindowStack.Add(new Dialog_NamePlayerSettlement(factionBase));
				}));
				list.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, delegate
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(factionBase));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0008A190 File Offset: 0x00088390
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void NextLesson()
		{
			LessonAutoActivator.DebugForceInitiateBestLessonNow();
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0008A197 File Offset: 0x00088397
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenAllMapMeshSections()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0008A1A8 File Offset: 0x000883A8
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeCameraConfig()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(CameraMapConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("CameraMapConfig_"))
				{
					text = text.Substring("CameraMapConfig_".Length);
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
				{
					Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0008A264 File Offset: 0x00088464
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceShipCountdown()
		{
			ShipCountdown.InitiateCountdown(null);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0008A26C File Offset: 0x0008846C
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceStartShip()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			Building_ShipComputerCore building_ShipComputerCore = (Building_ShipComputerCore)currentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Ship_ComputerCore).FirstOrDefault<Building>();
			if (building_ShipComputerCore == null)
			{
				Messages.Message("Could not find any compute core on current map!", MessageTypeDefOf.NeutralEvent, true);
			}
			building_ShipComputerCore.ForceLaunch();
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0008A2B8 File Offset: 0x000884B8
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashTradeDropSpot()
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
			Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
			Log.Message("trade drop spot: " + intVec, false);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0008A300 File Offset: 0x00088500
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillFactionLeader()
		{
			Pawn leader = (from x in Find.FactionManager.AllFactions
			where x.leader != null
			select x).RandomElement<Faction>().leader;
			int num = 0;
			while (!leader.Dead)
			{
				if (++num > 1000)
				{
					Log.Warning("Could not kill faction leader.", false);
					return;
				}
				leader.TakeDamage(new DamageInfo(DamageDefOf.Bullet, 30f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0008A390 File Offset: 0x00088590
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillKidnappedPawn()
		{
			IEnumerable<Pawn> pawnsBySituation = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Kidnapped);
			if (pawnsBySituation.Any<Pawn>())
			{
				Pawn pawn = pawnsBySituation.RandomElement<Pawn>();
				pawn.Kill(null, null);
				Messages.Message("Killed " + pawn.LabelCap, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0008A3E4 File Offset: 0x000885E4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetFactionRelations()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactionsVisibleInViewOrder)
			{
				Faction localFac = localFac2;
				foreach (object obj in Enum.GetValues(typeof(FactionRelationKind)))
				{
					FactionRelationKind localRk2 = (FactionRelationKind)obj;
					FactionRelationKind localRk = localRk2;
					FloatMenuOption item = new FloatMenuOption(localFac + " - " + localRk, delegate
					{
						localFac.TrySetRelationKind(Faction.OfPlayer, localRk, true, null, null);
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0008A50C File Offset: 0x0008870C
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void VisitorGift()
		{
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawnsSpawned)
			{
				if (pawn.Faction != null && !pawn.Faction.IsPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					list.Add(pawn);
					break;
				}
			}
			VisitorGiftForPlayerUtility.GiveGift(list, list[0].Faction);
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x0008A5AC File Offset: 0x000887AC
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RefogMap()
		{
			FloodFillerFog.DebugRefogMap(Find.CurrentMap);
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x0008A5B8 File Offset: 0x000887B8
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UseGenStep()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localGenStep2 in typeof(GenStep).AllSubclassesNonAbstract())
			{
				Type localGenStep = localGenStep2;
				list.Add(new DebugMenuOption(localGenStep.Name, DebugMenuOptionMode.Action, delegate
				{
					((GenStep)Activator.CreateInstance(localGenStep)).Generate(Find.CurrentMap, default(GenStepParams));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x0008A650 File Offset: 0x00088850
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Hour()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 2500);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x0008A66C File Offset: 0x0008886C
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime6Hours()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 15000);
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0008A688 File Offset: 0x00088888
		[DebugAction("General", "Increment time 1 day", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Day()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 60000);
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0008A6A4 File Offset: 0x000888A4
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void IncrementTime1Season()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 900000);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0008A6C0 File Offset: 0x000888C0
		[DebugAction("General", "Storywatcher tick 1 day", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StorywatcherTick1Day()
		{
			for (int i = 0; i < 60000; i++)
			{
				Find.StoryWatcher.StoryWatcherTick();
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
			}
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0008A700 File Offset: 0x00088900
		[DebugAction("General", "Add techprint to project", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate
				{
					Find.ResearchManager.AddTechprints(localProject, localProject.techprintCount - Find.ResearchManager.GetTechprints(localProject));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0008A7B4 File Offset: 0x000889B4
		[DebugAction("General", "Apply techprint on project", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyTechprintsForProject()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ResearchProjectDef localProject2 in from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.TechprintRequirementMet
			select p)
			{
				ResearchProjectDef localProject = localProject2;
				Action <>9__2;
				list.Add(new DebugMenuOption(localProject.LabelCap, DebugMenuOptionMode.Action, delegate
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "None";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
					Action method;
					if ((method = <>9__2) == null)
					{
						method = (<>9__2 = delegate
						{
							Find.ResearchManager.ApplyTechprint(localProject, null);
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					foreach (Pawn localColonist2 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
					{
						Pawn localColonist = localColonist2;
						list2.Add(new DebugMenuOption(localColonist.LabelCap, DebugMenuOptionMode.Action, delegate
						{
							Find.ResearchManager.ApplyTechprint(localProject, localColonist);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0008A868 File Offset: 0x00088A68
		private static void AddGuest(bool prisoner)
		{
			foreach (Building_Bed building_Bed in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if (building_Bed.ForPrisoners == prisoner && (!building_Bed.OwnersForReading.Any<Pawn>() || (prisoner && building_Bed.AnyUnownedSleepingSlot)))
				{
					PawnKindDef pawnKindDef;
					if (!prisoner)
					{
						pawnKindDef = PawnKindDefOf.SpaceRefugee;
					}
					else
					{
						pawnKindDef = (from pk in DefDatabase<PawnKindDef>.AllDefs
						where pk.defaultFactionType != null && !pk.defaultFactionType.isPlayer && pk.RaceProps.Humanlike
						select pk).RandomElement<PawnKindDef>();
					}
					Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction);
					GenSpawn.Spawn(pawn, building_Bed.Position, Find.CurrentMap, WipeMode.Vanish);
					foreach (ThingWithComps eq in pawn.equipment.AllEquipmentListForReading.ToList<ThingWithComps>())
					{
						ThingWithComps thingWithComps;
						if (pawn.equipment.TryDropEquipment(eq, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
					pawn.inventory.innerContainer.Clear();
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					pawn.guest.SetGuestStatus(Faction.OfPlayer, prisoner);
					break;
				}
			}
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0008AA00 File Offset: 0x00088C00
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void KillRandomLentColonist()
		{
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				DebugActionsMisc.tmpLentColonists.Clear();
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (questsListForReading[i].State == QuestState.Ongoing)
					{
						List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
						for (int j = 0; j < partsListForReading.Count; j++)
						{
							QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction;
							if ((questPart_LendColonistsToFaction = (partsListForReading[j] as QuestPart_LendColonistsToFaction)) != null)
							{
								List<Thing> lentColonistsListForReading = questPart_LendColonistsToFaction.LentColonistsListForReading;
								for (int k = 0; k < lentColonistsListForReading.Count; k++)
								{
									Pawn pawn;
									if ((pawn = (lentColonistsListForReading[k] as Pawn)) != null && !pawn.Dead)
									{
										DebugActionsMisc.tmpLentColonists.Add(pawn);
									}
								}
							}
						}
					}
				}
				Pawn pawn2 = DebugActionsMisc.tmpLentColonists.RandomElement<Pawn>();
				bool flag = pawn2.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad);
				pawn2.Kill(null, flag ? pawn2.health.hediffSet.hediffs.RandomElement<Hediff>() : null);
			}
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0008AB3C File Offset: 0x00088D3C
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyAllHats()
		{
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.RaceProps.Humanlike)
				{
					for (int i = pawn.apparel.WornApparel.Count - 1; i >= 0; i--)
					{
						Apparel apparel = pawn.apparel.WornApparel[i];
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
						{
							apparel.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0008AC08 File Offset: 0x00088E08
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PawnKindApparelCheck()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.race == ThingDefOf.Human
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Action, delegate
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					bool flag = false;
					for (int i = 0; i < 100; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(localKindDef, faction);
						if (pawn.royalty != null)
						{
							RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
							if (mostSeniorTitle != null && !mostSeniorTitle.def.requiredApparel.NullOrEmpty<RoyalTitleDef.ApparelRequirement>())
							{
								for (int j = 0; j < mostSeniorTitle.def.requiredApparel.Count; j++)
								{
									if (!mostSeniorTitle.def.requiredApparel[j].IsMet(pawn))
									{
										Log.Error(string.Concat(new object[]
										{
											localKindDef,
											" (",
											mostSeniorTitle.def.label,
											")  does not have its title requirements met. index=",
											j,
											DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|41_0(pawn)
										}), false);
										flag = true;
									}
								}
							}
						}
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int k = 0; k < wornApparel.Count; k++)
						{
							string text = DebugActionsMisc.<PawnKindApparelCheck>g__apparelOkayToWear|41_1(pawn, wornApparel[k]);
							if (text != "OK")
							{
								Log.Error(text + " - " + wornApparel[k].Label + DebugActionsMisc.<PawnKindApparelCheck>g__logApparel|41_0(pawn), false);
								flag = true;
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (!flag)
					{
						Log.Message("No errors for " + localKindDef.defName, false);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x0008ACDC File Offset: 0x00088EDC
		[DebugAction("General", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PawnKindAbilityCheck()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			StringBuilder sb = new StringBuilder();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			where kd.titleRequired != null || !kd.titleSelectOne.NullOrEmpty<RoyalTitleDef>()
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Action, delegate
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					for (int i = 0; i < 100; i++)
					{
						RoyalTitleDef fixedTitle = null;
						if (localKindDef.titleRequired != null)
						{
							fixedTitle = localKindDef.titleRequired;
						}
						else if (!localKindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(localKindDef.royalTitleChance))
						{
							fixedTitle = localKindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
						}
						Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(localKindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, fixedTitle));
						RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							Hediff_Psylink mainPsylinkSource = pawn.GetMainPsylinkSource();
							if (mainPsylinkSource == null)
							{
								if (mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def) > 0)
								{
									string text = mostSeniorTitle.def.LabelCap + " - No psylink.";
									if (pawn.abilities.abilities.Any((Ability x) => x.def.level > 0))
									{
										text += " Has psycasts without psylink.";
									}
									sb.AppendLine(text);
								}
							}
							else if (mainPsylinkSource.level < mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								sb.AppendLine(string.Concat(new object[]
								{
									"Psylink at level ",
									mainPsylinkSource.level,
									", but requires ",
									mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def)
								}));
							}
							else if (mainPsylinkSource.level > mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def))
							{
								sb.AppendLine(string.Concat(new object[]
								{
									"Psylink at level ",
									mainPsylinkSource.level,
									". Max is ",
									mostSeniorTitle.def.MaxAllowedPsylinkLevel(faction.def)
								}));
							}
						}
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					if (sb.Length == 0)
					{
						Log.Message("No errors for " + localKindDef.defName, false);
						return;
					}
					Log.Error("Errors:\n" + sb.ToString(), false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x04000F07 RID: 3847
		private static List<Pawn> tmpLentColonists = new List<Pawn>();

		// Token: 0x04000F08 RID: 3848
		private const string NoErrorString = "OK";

		// Token: 0x04000F09 RID: 3849
		private const string RoyalApparelTag = "Royal";

		// Token: 0x04000F0A RID: 3850
		private const int PawnsToGenerate = 100;
	}
}
