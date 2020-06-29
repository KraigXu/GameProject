using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	
	public static class DebugActionsMapManagement
	{
		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMap()
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = TileFinder.RandomStartingTile();
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void DestroyMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate
				{
					Current.Game.DeinitAndRemoveMap(map);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void LeakMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate
				{
					DebugActionsMapManagement.mapLeak = map;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void PrintLeakedMap()
		{
			Log.Message(string.Format("Leaked map {0}", DebugActionsMapManagement.mapLeak), false);
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void AddGameCondition()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugActionsMapManagement.Options_Add_GameCondition()));
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void RemoveGameCondition()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugActionsMapManagement.Options_Remove_GameCondition()));
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing, actionType = DebugActionType.ToolMap)]
		private static void Transfer()
		{
			List<Thing> toTransfer = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (!toTransfer.Any<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					Predicate<IntVec3> 9__1;
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate
					{
						for (int j = 0; j < toTransfer.Count; j++)
						{
							Map map;
							IntVec3 center = map.Center;
							map = map;
							int squareRadius = Mathf.Max(map.Size.x, map.Size.z);
							Predicate<IntVec3> validator;
							if ((validator ) == null)
							{
								validator = (9__1 = ((IntVec3 x) => !x.Fogged(map) && x.Standable(map)));
							}
							IntVec3 center2;
							if (CellFinder.TryFindRandomCellNear(center, map, squareRadius, validator, out center2, -1))
							{
								toTransfer[j].DeSpawn(DestroyMode.Vanish);
								GenPlace.TryPlaceThing(toTransfer[j], center2, map, ThingPlaceMode.Near, null, null, default(Rot4));
							}
							else
							{
								Log.Error("Could not find spawn cell.", false);
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void ChangeMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate
					{
						Current.Game.CurrentMap = map;
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void RegenerateCurrentMap()
		{
			RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
			int tile = Find.CurrentMap.Tile;
			MapParent parent = Find.CurrentMap.Parent;
			IntVec3 size = Find.CurrentMap.Size;
			Current.Game.DeinitAndRemoveMap(Find.CurrentMap);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMapWithCaves()
		{
			int tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, false, (int x) => Find.World.HasCaves(x));
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.Parent.Destroy();
			}
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = tile;
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, null);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void RunMapGenerator()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<MapGeneratorDef>.Enumerator enumerator = DefDatabase<MapGeneratorDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MapGeneratorDef mapgen = enumerator.Current;
					list.Add(new DebugMenuOption(mapgen.defName, DebugMenuOptionMode.Action, delegate
					{
						MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
						mapParent.Tile = (from tile in Enumerable.Range(0, Find.WorldGrid.TilesCount)
						where Find.WorldGrid[tile].biome.canBuildBase
						select tile).RandomElement<int>();
						mapParent.SetFaction(Faction.OfPlayer);
						Find.WorldObjects.Add(mapParent);
						Map currentMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, mapgen, null, null);
						Current.Game.CurrentMap = currentMap;
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void ForceReformInCurrentMap()
		{
			if (Find.CurrentMap != null)
			{
				TimedForcedExit.ForceReform(Find.CurrentMap.Parent);
			}
		}

		
		public static List<DebugMenuOption> Options_Add_GameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate
				{
					Find.CurrentMap.GameConditionManager.RegisterCondition(GameConditionMaker.MakeCondition(localDef, -1));
				}));
			}
			return list;
		}

		
		public static List<DebugMenuOption> Options_Remove_GameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameCondition localCondition2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
			{
				GameCondition localCondition = localCondition2;
				list.Add(new DebugMenuOption(localCondition.def.LabelCap, DebugMenuOptionMode.Tool, delegate
				{
					localCondition.Duration = 0;
				}));
			}
			return list;
		}

		
		private static Map mapLeak;
	}
}
