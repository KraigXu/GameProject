using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public static class PawnsFinder
	{
		
		
		public static List<Pawn> AllMapsWorldAndTemporary_AliveOrDead
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.AllMapsWorldAndTemporary_Alive);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(Find.WorldPawns.AllPawnsDead);
				}
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.Temporary_Dead);
				return PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsWorldAndTemporary_Alive
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.Temporary_Alive);
				return PawnsFinder.allMapsWorldAndTemporary_Alive_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsAndWorld_Alive
		{
			get
			{
				PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
				PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				return PawnsFinder.allMapsAndWorld_Alive_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps
		{
			get
			{
				PawnsFinder.allMaps_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawns;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Result.AddRange(maps[i].mapPawns.AllPawns);
					}
				}
				return PawnsFinder.allMaps_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps_Spawned
		{
			get
			{
				PawnsFinder.allMaps_Spawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawnsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Spawned_Result.AddRange(maps[i].mapPawns.AllPawnsSpawned);
					}
				}
				return PawnsFinder.allMaps_Spawned_Result;
			}
		}

		
		
		public static List<Pawn> All_AliveOrDead
		{
			get
			{
				List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
				List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead = PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead;
				if (allCaravansAndTravelingTransportPods_AliveOrDead.Count == 0)
				{
					return allMapsWorldAndTemporary_AliveOrDead;
				}
				PawnsFinder.all_AliveOrDead_Result.Clear();
				PawnsFinder.all_AliveOrDead_Result.AddRange(allMapsWorldAndTemporary_AliveOrDead);
				PawnsFinder.all_AliveOrDead_Result.AddRange(allCaravansAndTravelingTransportPods_AliveOrDead);
				return PawnsFinder.all_AliveOrDead_Result;
			}
		}

		
		
		public static List<Pawn> Temporary
		{
			get
			{
				PawnsFinder.temporary_Result.Clear();
				List<List<Pawn>> pawnsBeingGeneratedNow = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int i = 0; i < pawnsBeingGeneratedNow.Count; i++)
				{
					PawnsFinder.temporary_Result.AddRange(pawnsBeingGeneratedNow[i]);
				}
				List<List<Thing>> thingsBeingGeneratedNow = ThingSetMaker.thingsBeingGeneratedNow;
				for (int j = 0; j < thingsBeingGeneratedNow.Count; j++)
				{
					List<Thing> list = thingsBeingGeneratedNow[j];
					for (int k = 0; k < list.Count; k++)
					{
						Pawn pawn = list[k] as Pawn;
						if (pawn != null)
						{
							PawnsFinder.temporary_Result.Add(pawn);
						}
					}
				}
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null)
				{
					List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
					for (int l = 0; l < startingAndOptionalPawns.Count; l++)
					{
						if (startingAndOptionalPawns[l] != null)
						{
							PawnsFinder.temporary_Result.Add(startingAndOptionalPawns[l]);
						}
					}
				}
				if (Find.World != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int m = 0; m < sites.Count; m++)
					{
						for (int n = 0; n < sites[m].parts.Count; n++)
						{
							if (sites[m].parts[n].things != null && sites[m].parts[n].things.contentsLookMode == LookMode.Deep)
							{
								ThingOwner things = sites[m].parts[n].things;
								for (int num = 0; num < things.Count; num++)
								{
									Pawn pawn2 = things[num] as Pawn;
									if (pawn2 != null)
									{
										PawnsFinder.temporary_Result.Add(pawn2);
									}
								}
							}
						}
					}
				}
				if (Find.World != null)
				{
					List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
					for (int num2 = 0; num2 < allWorldObjects.Count; num2++)
					{
						DownedRefugeeComp component = allWorldObjects[num2].GetComponent<DownedRefugeeComp>();
						if (component != null && component.pawn != null && component.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component.pawn[0]);
						}
						PrisonerWillingToJoinComp component2 = allWorldObjects[num2].GetComponent<PrisonerWillingToJoinComp>();
						if (component2 != null && component2.pawn != null && component2.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component2.pawn[0]);
						}
					}
				}
				return PawnsFinder.temporary_Result;
			}
		}

		
		
		public static List<Pawn> Temporary_Alive
		{
			get
			{
				PawnsFinder.temporary_Alive_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (!temporary[i].Dead)
					{
						PawnsFinder.temporary_Alive_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Alive_Result;
			}
		}

		
		
		public static List<Pawn> Temporary_Dead
		{
			get
			{
				PawnsFinder.temporary_Dead_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (temporary[i].Dead)
					{
						PawnsFinder.temporary_Dead_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Dead_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive
		{
			get
			{
				List<Pawn> allMaps = PawnsFinder.AllMaps;
				List<Pawn> allCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllCaravansAndTravelingTransportPods_Alive;
				if (allCaravansAndTravelingTransportPods_Alive.Count == 0)
				{
					return allMaps;
				}
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.AddRange(allMaps);
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.AddRange(allCaravansAndTravelingTransportPods_Alive);
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result;
			}
		}

		
		
		public static List<Pawn> AllCaravansAndTravelingTransportPods_Alive
		{
			get
			{
				PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result.Clear();
				List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead = PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead;
				for (int i = 0; i < allCaravansAndTravelingTransportPods_AliveOrDead.Count; i++)
				{
					if (!allCaravansAndTravelingTransportPods_AliveOrDead[i].Dead)
					{
						PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result.Add(allCaravansAndTravelingTransportPods_AliveOrDead[i]);
					}
				}
				return PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result;
			}
		}

		
		
		public static List<Pawn> AllCaravansAndTravelingTransportPods_AliveOrDead
		{
			get
			{
				PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.Clear();
				if (Find.World != null)
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int i = 0; i < caravans.Count; i++)
					{
						PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.AddRange(caravans[i].PawnsListForReading);
					}
					List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
					for (int j = 0; j < travelingTransportPods.Count; j++)
					{
						PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.AddRange(travelingTransportPods[j].Pawns);
					}
				}
				return PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_Colonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsColonist)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsFreeColonist)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsFreeColonist && !allMapsCaravansAndTravelingTransportPods_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].Faction == ofPlayer)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].Faction == ofPlayer && !allMapsCaravansAndTravelingTransportPods_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravelingTransportPods_Alive[i].IsPrisonerOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony;
				if (allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count == 0)
				{
					return allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
				}
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists);
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony);
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result;
			}
		}

		
		
		public static List<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners;
				for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners.Count; i++)
				{
					if (!allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Add(allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColonySpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.AddRange(maps[i].mapPawns.PrisonersOfColonySpawned);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColonySpawned_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColony;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColony_Result.AddRange(maps[i].mapPawns.PrisonersOfColony);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColony_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps_FreeColonists
		{
			get
			{
				PawnsFinder.allMaps_FreeColonists_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonists;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonists_Result.AddRange(maps[i].mapPawns.FreeColonists);
					}
				}
				return PawnsFinder.allMaps_FreeColonists_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsSpawned_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisonersSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisonersSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result;
			}
		}

		
		
		public static List<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisoners;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisoners);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result;
			}
		}

		
		public static List<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			List<Pawn> list;
			if (!PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Add(faction, list);
			}
			list.Clear();
			if (Current.ProgramState != ProgramState.Entry)
			{
				List<Map> maps = Find.Maps;
				if (maps.Count == 1)
				{
					return maps[0].mapPawns.SpawnedPawnsInFaction(faction);
				}
				for (int i = 0; i < maps.Count; i++)
				{
					list.AddRange(maps[i].mapPawns.SpawnedPawnsInFaction(faction));
				}
			}
			return list;
		}

		
		
		public static List<Pawn> HomeMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						if (!maps[0].IsPlayerHome)
						{
							return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
						}
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					else
					{
						for (int i = 0; i < maps.Count; i++)
						{
							if (maps[i].IsPlayerHome)
							{
								PawnsFinder.homeMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
							}
						}
					}
				}
				return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
			}
		}

		
		public static void Clear()
		{
			PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
			PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
			PawnsFinder.allMaps_Result.Clear();
			PawnsFinder.allMaps_Spawned_Result.Clear();
			PawnsFinder.all_AliveOrDead_Result.Clear();
			PawnsFinder.temporary_Result.Clear();
			PawnsFinder.temporary_Alive_Result.Clear();
			PawnsFinder.temporary_Dead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravelingTransportPods_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravelingTransportPods_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMaps_FreeColonists_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Clear();
			PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
		}

		
		private static List<Pawn> allMapsWorldAndTemporary_AliveOrDead_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsWorldAndTemporary_Alive_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsAndWorld_Alive_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_Spawned_Result = new List<Pawn>();

		
		private static List<Pawn> all_AliveOrDead_Result = new List<Pawn>();

		
		private static List<Pawn> temporary_Result = new List<Pawn>();

		
		private static List<Pawn> temporary_Alive_Result = new List<Pawn>();

		
		private static List<Pawn> temporary_Dead_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		
		private static List<Pawn> allCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		
		private static List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_PrisonersOfColonySpawned_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_PrisonersOfColony_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_FreeColonists_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_FreeColonistsSpawned_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_FreeColonistsAndPrisonersSpawned_Result = new List<Pawn>();

		
		private static List<Pawn> allMaps_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		
		private static Dictionary<Faction, List<Pawn>> allMaps_SpawnedPawnsInFaction_Result = new Dictionary<Faction, List<Pawn>>();

		
		private static List<Pawn> homeMaps_FreeColonistsSpawned_Result = new List<Pawn>();
	}
}
