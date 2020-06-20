using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2B RID: 2859
	public static class PawnsFinder
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600432C RID: 17196 RVA: 0x00169AB0 File Offset: 0x00167CB0
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

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x0600432D RID: 17197 RVA: 0x00169B08 File Offset: 0x00167D08
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

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600432E RID: 17198 RVA: 0x00169B5D File Offset: 0x00167D5D
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

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600432F RID: 17199 RVA: 0x00169B98 File Offset: 0x00167D98
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

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004330 RID: 17200 RVA: 0x00169C08 File Offset: 0x00167E08
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

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004331 RID: 17201 RVA: 0x00169C78 File Offset: 0x00167E78
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

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06004332 RID: 17202 RVA: 0x00169CC0 File Offset: 0x00167EC0
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

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06004333 RID: 17203 RVA: 0x00169F4C File Offset: 0x0016814C
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

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06004334 RID: 17204 RVA: 0x00169FA0 File Offset: 0x001681A0
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

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06004335 RID: 17205 RVA: 0x00169FF4 File Offset: 0x001681F4
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

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06004336 RID: 17206 RVA: 0x0016A03C File Offset: 0x0016823C
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

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004337 RID: 17207 RVA: 0x0016A090 File Offset: 0x00168290
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

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06004338 RID: 17208 RVA: 0x0016A118 File Offset: 0x00168318
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

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004339 RID: 17209 RVA: 0x0016A16C File Offset: 0x0016836C
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

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600433A RID: 17210 RVA: 0x0016A1C0 File Offset: 0x001683C0
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

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x0600433B RID: 17211 RVA: 0x0016A220 File Offset: 0x00168420
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

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x0600433C RID: 17212 RVA: 0x0016A27C File Offset: 0x0016847C
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

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x0600433D RID: 17213 RVA: 0x0016A2E4 File Offset: 0x001684E4
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

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600433E RID: 17214 RVA: 0x0016A338 File Offset: 0x00168538
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

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x0600433F RID: 17215 RVA: 0x0016A380 File Offset: 0x00168580
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

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004340 RID: 17216 RVA: 0x0016A3D4 File Offset: 0x001685D4
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

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06004341 RID: 17217 RVA: 0x0016A444 File Offset: 0x00168644
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

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06004342 RID: 17218 RVA: 0x0016A4B4 File Offset: 0x001686B4
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

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06004343 RID: 17219 RVA: 0x0016A524 File Offset: 0x00168724
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

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06004344 RID: 17220 RVA: 0x0016A594 File Offset: 0x00168794
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

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06004345 RID: 17221 RVA: 0x0016A604 File Offset: 0x00168804
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

		// Token: 0x06004346 RID: 17222 RVA: 0x0016A674 File Offset: 0x00168874
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

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004347 RID: 17223 RVA: 0x0016A6FC File Offset: 0x001688FC
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

		// Token: 0x06004348 RID: 17224 RVA: 0x0016A790 File Offset: 0x00168990
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

		// Token: 0x04002692 RID: 9874
		private static List<Pawn> allMapsWorldAndTemporary_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x04002693 RID: 9875
		private static List<Pawn> allMapsWorldAndTemporary_Alive_Result = new List<Pawn>();

		// Token: 0x04002694 RID: 9876
		private static List<Pawn> allMapsAndWorld_Alive_Result = new List<Pawn>();

		// Token: 0x04002695 RID: 9877
		private static List<Pawn> allMaps_Result = new List<Pawn>();

		// Token: 0x04002696 RID: 9878
		private static List<Pawn> allMaps_Spawned_Result = new List<Pawn>();

		// Token: 0x04002697 RID: 9879
		private static List<Pawn> all_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x04002698 RID: 9880
		private static List<Pawn> temporary_Result = new List<Pawn>();

		// Token: 0x04002699 RID: 9881
		private static List<Pawn> temporary_Alive_Result = new List<Pawn>();

		// Token: 0x0400269A RID: 9882
		private static List<Pawn> temporary_Dead_Result = new List<Pawn>();

		// Token: 0x0400269B RID: 9883
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		// Token: 0x0400269C RID: 9884
		private static List<Pawn> allCaravansAndTravelingTransportPods_Alive_Result = new List<Pawn>();

		// Token: 0x0400269D RID: 9885
		private static List<Pawn> allCaravansAndTravelingTransportPods_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x0400269E RID: 9886
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_Colonists_Result = new List<Pawn>();

		// Token: 0x0400269F RID: 9887
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_Result = new List<Pawn>();

		// Token: 0x040026A0 RID: 9888
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040026A1 RID: 9889
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_Result = new List<Pawn>();

		// Token: 0x040026A2 RID: 9890
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040026A3 RID: 9891
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x040026A4 RID: 9892
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x040026A5 RID: 9893
		private static List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040026A6 RID: 9894
		private static List<Pawn> allMaps_PrisonersOfColonySpawned_Result = new List<Pawn>();

		// Token: 0x040026A7 RID: 9895
		private static List<Pawn> allMaps_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x040026A8 RID: 9896
		private static List<Pawn> allMaps_FreeColonists_Result = new List<Pawn>();

		// Token: 0x040026A9 RID: 9897
		private static List<Pawn> allMaps_FreeColonistsSpawned_Result = new List<Pawn>();

		// Token: 0x040026AA RID: 9898
		private static List<Pawn> allMaps_FreeColonistsAndPrisonersSpawned_Result = new List<Pawn>();

		// Token: 0x040026AB RID: 9899
		private static List<Pawn> allMaps_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x040026AC RID: 9900
		private static Dictionary<Faction, List<Pawn>> allMaps_SpawnedPawnsInFaction_Result = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x040026AD RID: 9901
		private static List<Pawn> homeMaps_FreeColonistsSpawned_Result = new List<Pawn>();
	}
}
