using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200033E RID: 830
	public static class DebugToolsSpawning
	{
		// Token: 0x060018DE RID: 6366 RVA: 0x0008F994 File Offset: 0x0008DB94
		private static IEnumerable<float> PointsMechCluster()
		{
			for (float points = 50f; points <= 10000f; points += 50f)
			{
				yield return points;
			}
			yield break;
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0008F9A0 File Offset: 0x0008DBA0
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Tool, delegate
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					Pawn newPawn = PawnGenerator.GeneratePawn(localKindDef, faction);
					GenSpawn.Spawn(newPawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
					if (faction != null && faction != Faction.OfPlayer)
					{
						Lord lord = null;
						if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn))
						{
							lord = ((Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, (Thing p) => p != newPawn && ((Pawn)p).GetLord() != null, null)).GetLord();
						}
						if (lord == null)
						{
							LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn.Position);
							lord = LordMaker.MakeNewLord(faction, lordJob, Find.CurrentMap, null);
						}
						lord.AddPawn(newPawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x0008FA50 File Offset: 0x0008DC50
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnWeapon()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0008FB24 File Offset: 0x0008DD24
		[DebugAction("Spawning", "Spawn apparel...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnApparel()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.IsApparel
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0008FBF8 File Offset: 0x0008DDF8
		[DebugAction("Spawning", "Try place near thing...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearThing()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)));
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0008FC10 File Offset: 0x0008DE10
		[DebugAction("Spawning", "Try place near stack of 25...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearStacksOf25()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)));
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0008FC29 File Offset: 0x0008DE29
		[DebugAction("Spawning", "Try place near stack of 75...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearStacksOf75()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)));
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x0008FC42 File Offset: 0x0008DE42
		[DebugAction("Spawning", "Try place direct thing...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceDirectThing()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)));
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x0008FC5A File Offset: 0x0008DE5A
		[DebugAction("Spawning", "Try place direct stack of 25...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceDirectStackOf25()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)));
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x0008FC74 File Offset: 0x0008DE74
		[DebugAction("Spawning", "Spawn thing with wipe mode...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnThingWithWipeMode()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			WipeMode[] array = (WipeMode[])Enum.GetValues(typeof(WipeMode));
			for (int i = 0; i < array.Length; i++)
			{
				WipeMode localWipeMode2 = array[i];
				WipeMode localWipeMode = localWipeMode2;
				list.Add(new DebugMenuOption(localWipeMode2.ToString(), DebugMenuOptionMode.Action, delegate
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.SpawnOptions(localWipeMode)));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0008FCF4 File Offset: 0x0008DEF4
		[DebugAction("Spawning", "Set terrain...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetTerrain()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (TerrainDef localDef2 in DefDatabase<TerrainDef>.AllDefs)
			{
				TerrainDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate
				{
					if (UI.MouseCell().InBounds(Find.CurrentMap))
					{
						Find.CurrentMap.terrainGrid.SetTerrain(UI.MouseCell(), localDef);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0008FD84 File Offset: 0x0008DF84
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnMechCluster()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float num in DebugToolsSpawning.PointsMechCluster())
			{
				float localPoints = num;

				list.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "In pods, click place";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Tool;
					Action method = delegate
					{
						MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true);
						MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, true, false, null);
					};

					list3.Add(new DebugMenuOption(label, mode, method));
					List<DebugMenuOption> list4 = list2;
					string label2 = "In pods, autoplace";
					DebugMenuOptionMode mode2 = DebugMenuOptionMode.Action;
					Action method2= delegate
					{
						MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true);
						MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, true, false, null);
					};
	
					list4.Add(new DebugMenuOption(label2, mode2, method2));
					List<DebugMenuOption> list5 = list2;
					string label3 = "Direct spawn, click place";
					DebugMenuOptionMode mode3 = DebugMenuOptionMode.Tool;
					Action method3 = delegate
					{
						MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true);
						MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, false, false, null);
					};

					list5.Add(new DebugMenuOption(label3, mode3, method3));
					List<DebugMenuOption> list6 = list2;
					string label4 = "Direct spawn, autoplace";
					DebugMenuOptionMode mode4 = DebugMenuOptionMode.Action;
					Action method4= delegate
					{
						MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true);
						MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, false, false, null);
					};

					list6.Add(new DebugMenuOption(label4, mode4, method4));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0008FE14 File Offset: 0x0008E014
		[DebugAction("Spawning", "Make filth x100", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeFilthx100()
		{
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = UI.MouseCell() + GenRadial.RadialPattern[i];
				if (c.InBounds(Find.CurrentMap) && c.Walkable(Find.CurrentMap))
				{
					FilthMaker.TryMakeFilth(c, Find.CurrentMap, ThingDefOf.Filth_Dirt, 2, FilthSourceFlags.None);
					MoteMaker.ThrowMetaPuff(c.ToVector3Shifted(), Find.CurrentMap);
				}
			}
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0008FE84 File Offset: 0x0008E084
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnFactionLeader()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				if (localFac.leader != null)
				{
					list.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, delegate
					{
						GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0008FF4C File Offset: 0x0008E14C
		[DebugAction("Spawning", "Spawn world pawn...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action<Pawn> act = delegate(Pawn p)
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> predicate=x => x.race == p.def;

				foreach (PawnKindDef kLocal2 in allDefs.Where(predicate))
				{
					PawnKindDef kLocal = kLocal2;
					list2.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate
					{
						PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
						PawnGenerator.RedressPawn(p, request);
						GenSpawn.Spawn(p, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugTools.curTool = null;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			};
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort, DebugMenuOptionMode.Action, delegate
				{
					act(pLocal);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00090014 File Offset: 0x0008E214
		[DebugAction("Spawning", "Spawn thing set...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnThingSet()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<ThingSetMakerDef> allDefsListForReading = DefDatabase<ThingSetMakerDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingSetMakerDef localGenerator = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localGenerator.defName, DebugMenuOptionMode.Tool, delegate
				{
					if (!UI.MouseCell().InBounds(Find.CurrentMap))
					{
						return;
					}
					StringBuilder stringBuilder = new StringBuilder();
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localGenerator.debugParams);
					List<Thing> list2 = localGenerator.root.Generate(localGenerator.debugParams);
					stringBuilder.Append(string.Concat(new object[]
					{
						localGenerator.defName,
						" generated ",
						list2.Count,
						" things"
					}));
					if (!nonNullFieldsDebugInfo.NullOrEmpty())
					{
						stringBuilder.Append(" (used custom debug params: " + nonNullFieldsDebugInfo + ")");
					}
					stringBuilder.AppendLine(":");
					float num = 0f;
					float num2 = 0f;
					for (int j = 0; j < list2.Count; j++)
					{
						stringBuilder.AppendLine("   - " + list2[j].LabelCap);
						num += list2[j].MarketValue * (float)list2[j].stackCount;
						if (!(list2[j] is Pawn))
						{
							num2 += list2[j].GetStatValue(StatDefOf.Mass, true) * (float)list2[j].stackCount;
						}
						if (!GenPlace.TryPlaceThing(list2[j], UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4)))
						{
							list2[j].Destroy(DestroyMode.Vanish);
						}
					}
					stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
					stringBuilder.AppendLine("Total mass: " + num2.ToStringMass());
					Log.Message(stringBuilder.ToString(), false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00090084 File Offset: 0x0008E284
		[DebugAction("Spawning", "Trigger effecter...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TriggerEffecter()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<EffecterDef> allDefsListForReading = DefDatabase<EffecterDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				EffecterDef localDef = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate
				{
					Effecter effecter = localDef.Spawn();
					effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), new TargetInfo(UI.MouseCell(), Find.CurrentMap, false));
					effecter.Cleanup();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x000900F4 File Offset: 0x0008E2F4
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnShuttle()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Incoming", DebugMenuOptionMode.Tool, delegate
			{
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, ThingMaker.MakeThing(ThingDefOf.Shuttle, null)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}));
			list.Add(new DebugMenuOption("Stationary", DebugMenuOptionMode.Tool, delegate
			{
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Shuttle, null), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}));
			List<DebugMenuOption> options = list;
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(options));
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00090178 File Offset: 0x0008E378
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomCaravan()
		{
			int num = GenWorld.MouseTile(false);
			if (Find.WorldGrid[num].biome.impassable)
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			int num2 = Rand.RangeInclusive(1, 10);
			for (int i = 0; i < num2; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				list.Add(pawn);
				if (!pawn.WorkTagIsDisabled(WorkTags.Violent) && Rand.Value < 0.9f)
				{
					ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefs
					where def.IsWeapon && def.PlayerAcquirable
					select def).RandomElementWithFallback(null);
					pawn.equipment.AddEquipment((ThingWithComps)ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
				}
			}
			int num3 = Rand.RangeInclusive(-4, 10);
			for (int j = 0; j < num3; j++)
			{
				Pawn item = PawnGenerator.GeneratePawn((from d in DefDatabase<PawnKindDef>.AllDefs
				where d.RaceProps.Animal && d.RaceProps.wildness < 1f
				select d).RandomElement<PawnKindDef>(), Faction.OfPlayer);
				list.Add(item);
			}
			Caravan caravan = CaravanMaker.MakeCaravan(list, Faction.OfPlayer, num, true);
			List<Thing> list2 = ThingSetMakerDefOf.DebugCaravanInventory.root.Generate();
			for (int k = 0; k < list2.Count; k++)
			{
				Thing thing = list2[k];
				if (thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount > caravan.MassCapacity - caravan.MassUsage)
				{
					break;
				}
				CaravanInventoryUtility.GiveThing(caravan, thing);
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00090320 File Offset: 0x0008E520
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomFactionBase()
		{
			Faction faction;
			if ((from x in Find.FactionManager.AllFactions
			where !x.IsPlayer && !x.def.hidden
			select x).TryRandomElement(out faction))
			{
				int num = GenWorld.MouseTile(false);
				if (Find.WorldGrid[num].biome.impassable)
				{
					return;
				}
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
				settlement.SetFaction(faction);
				settlement.Tile = num;
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x000903BC File Offset: 0x0008E5BC
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSite()
		{
			int tile = GenWorld.MouseTile(false);
			if (tile < 0 || Find.World.Impassable(tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}

			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			addPart = delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method= delegate
				{
					Site site = SiteMaker.TryMakeSite(parts, tile, true, null, true, null);
					if (site == null)
					{
						Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
						return;
					}
					Find.WorldObjects.Add(site);
				};

				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			};
			addPart();
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00090444 File Offset: 0x0008E644
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void DestroySite()
		{
			int tileID = GenWorld.MouseTile(false);
			foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(tileID).ToList<WorldObject>())
			{
				worldObject.Destroy();
			}
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x000904A8 File Offset: 0x0008E6A8
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSiteWithPoints()
		{
			int tile = GenWorld.MouseTile(false);
			if (tile < 0 || Find.World.Impassable(tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			addPart = delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method= delegate
				{
					List<DebugMenuOption> list3 = new List<DebugMenuOption>();
					foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = localPoints2;
						list3.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate
						{
							Site site = SiteMaker.TryMakeSite(parts,tile, true, null, true, new float?(localPoints));
							if (site == null)
							{
								Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
								return;
							}
							Find.WorldObjects.Add(site);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
				};

				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			};
			addPart();
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00090530 File Offset: 0x0008E730
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnWorldObject()
		{
			int tile = GenWorld.MouseTile(false);
			if (tile < 0 || Find.World.Impassable(tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WorldObjectDef localDef2 in DefDatabase<WorldObjectDef>.AllDefs)
			{
				WorldObjectDef localDef = localDef2;
				Action method = delegate
				{
					WorldObject worldObject = WorldObjectMaker.MakeWorldObject(localDef);
					worldObject.Tile = tile;
					Find.WorldObjects.Add(worldObject);
				};
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, method));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0009060C File Offset: 0x0008E80C
		[DebugAction("General", "Change camera config...", allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void ChangeCameraConfigWorld()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(WorldCameraConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("WorldCameraConfig_"))
				{
					text = text.Substring("WorldCameraConfig_".Length);
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
				{
					Find.WorldCameraDriver.config = (WorldCameraConfig)Activator.CreateInstance(localType);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}
