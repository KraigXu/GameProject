using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200017E RID: 382
	public sealed class MapPawns
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00039C2C File Offset: 0x00037E2C
		public List<Pawn> AllPawns
		{
			get
			{
				List<Pawn> allPawnsUnspawned = this.AllPawnsUnspawned;
				if (allPawnsUnspawned.Count == 0)
				{
					return this.pawnsSpawned;
				}
				this.allPawnsResult.Clear();
				this.allPawnsResult.AddRange(this.pawnsSpawned);
				this.allPawnsResult.AddRange(allPawnsUnspawned);
				return this.allPawnsResult;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00039C80 File Offset: 0x00037E80
		public List<Pawn> AllPawnsUnspawned
		{
			get
			{
				this.allPawnsUnspawnedResult.Clear();
				ThingOwnerUtility.GetAllThingsRecursively<Pawn>(this.map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), this.allPawnsUnspawnedResult, true, null, false);
				for (int i = this.allPawnsUnspawnedResult.Count - 1; i >= 0; i--)
				{
					if (this.allPawnsUnspawnedResult[i].Dead)
					{
						this.allPawnsUnspawnedResult.RemoveAt(i);
					}
				}
				return this.allPawnsUnspawnedResult;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00039CF0 File Offset: 0x00037EF0
		public List<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00039D00 File Offset: 0x00037F00
		public List<Pawn> PrisonersOfColony
		{
			get
			{
				this.prisonersOfColonyResult.Clear();
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsPrisonerOfColony)
					{
						this.prisonersOfColonyResult.Add(allPawns[i]);
					}
				}
				return this.prisonersOfColonyResult;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00039D58 File Offset: 0x00037F58
		public List<Pawn> FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> freeColonists = this.FreeColonists;
				List<Pawn> prisonersOfColony = this.PrisonersOfColony;
				if (prisonersOfColony.Count == 0)
				{
					return freeColonists;
				}
				this.freeColonistsAndPrisonersResult.Clear();
				this.freeColonistsAndPrisonersResult.AddRange(freeColonists);
				this.freeColonistsAndPrisonersResult.AddRange(prisonersOfColony);
				return this.freeColonistsAndPrisonersResult;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00039DA8 File Offset: 0x00037FA8
		public int ColonistCount
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.", false);
					return 3;
				}
				int num = 0;
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00039DF7 File Offset: 0x00037FF7
		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00039E04 File Offset: 0x00038004
		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00039E11 File Offset: 0x00038011
		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00039E1E File Offset: 0x0003801E
		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00039E1E File Offset: 0x0003801E
		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00039E2C File Offset: 0x0003802C
		public bool AnyPawnBlockingMapRemoval
		{
			get
			{
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (!this.pawnsSpawned[i].Downed && this.pawnsSpawned[i].IsColonist)
					{
						return true;
					}
					if (this.pawnsSpawned[i].relations != null && this.pawnsSpawned[i].relations.relativeInvolvedInRescueQuest != null)
					{
						return true;
					}
					if (this.pawnsSpawned[i].Faction == ofPlayer || this.pawnsSpawned[i].HostFaction == ofPlayer)
					{
						Job curJob = this.pawnsSpawned[i].CurJob;
						if (curJob != null && curJob.exitMapOnArrival)
						{
							return true;
						}
					}
					if (CaravanExitMapUtility.FindCaravanToJoinFor(this.pawnsSpawned[i]) != null && !this.pawnsSpawned[i].Downed)
					{
						return true;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j] is IActiveDropPod || list[j].TryGetComp<CompTransporter>() != null)
					{
						IThingHolder thingHolder = list[j].TryGetComp<CompTransporter>();
						IThingHolder holder = thingHolder ?? ((IThingHolder)list[j]);
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(holder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && !pawn.Downed && pawn.IsColonist)
							{
								this.tmpThings.Clear();
								return true;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return false;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0003A00E File Offset: 0x0003820E
		public List<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0003A016 File Offset: 0x00038216
		public List<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0003A023 File Offset: 0x00038223
		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0003A02C File Offset: 0x0003822C
		public List<Pawn> FreeColonistsAndPrisonersSpawned
		{
			get
			{
				List<Pawn> freeColonistsSpawned = this.FreeColonistsSpawned;
				List<Pawn> list = this.PrisonersOfColonySpawned;
				if (list.Count == 0)
				{
					return freeColonistsSpawned;
				}
				this.freeColonistsAndPrisonersSpawnedResult.Clear();
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(freeColonistsSpawned);
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(list);
				return this.freeColonistsAndPrisonersSpawnedResult;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0003A07C File Offset: 0x0003827C
		public List<Pawn> SpawnedPawnsWithAnyHediff
		{
			get
			{
				this.spawnedPawnsWithAnyHediffResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedPawnsWithAnyHediffResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithAnyHediffResult;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0003A0E4 File Offset: 0x000382E4
		public List<Pawn> SpawnedHungryPawns
		{
			get
			{
				this.spawnedHungryPawnsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (FeedPatientUtility.IsHungry(allPawnsSpawned[i]))
					{
						this.spawnedHungryPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedHungryPawnsResult;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0003A13C File Offset: 0x0003833C
		public List<Pawn> SpawnedDownedPawns
		{
			get
			{
				this.spawnedDownedPawnsResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Downed)
					{
						this.spawnedDownedPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedDownedPawnsResult;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0003A194 File Offset: 0x00038394
		public List<Pawn> SpawnedPawnsWhoShouldHaveSurgeryDoneNow
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
					{
						this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0003A1EC File Offset: 0x000383EC
		public List<Pawn> SpawnedPawnsWhoShouldHaveInventoryUnloaded
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Clear();
				List<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].inventory.UnloadEverything)
					{
						this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0003A247 File Offset: 0x00038447
		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0003A254 File Offset: 0x00038454
		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0003A261 File Offset: 0x00038461
		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0003A26E File Offset: 0x0003846E
		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0003A27C File Offset: 0x0003847C
		public int ColonistsSpawnedCount
		{
			get
			{
				int num = 0;
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0003A2BC File Offset: 0x000384BC
		public int FreeColonistsSpawnedOrInPlayerEjectablePodsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (this.pawnsSpawned[i].IsFreeColonist)
					{
						num++;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = list[j] as Building_CryptosleepCasket;
					if ((building_CryptosleepCasket != null && building_CryptosleepCasket.def.building.isPlayerEjectable) || list[j] is IActiveDropPod || list[j].TryGetComp<CompTransporter>() != null)
					{
						IThingHolder thingHolder = list[j].TryGetComp<CompTransporter>();
						IThingHolder holder = thingHolder ?? ((IThingHolder)list[j]);
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(holder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && pawn.IsFreeColonist)
							{
								num++;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return num;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x0003A3F4 File Offset: 0x000385F4
		public bool AnyColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x0003A430 File Offset: 0x00038630
		public bool AnyFreeColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsFreeColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003A46C File Offset: 0x0003866C
		public MapPawns(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003A544 File Offset: 0x00038744
		private void EnsureFactionsListsInit()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (!this.pawnsInFactionSpawned.ContainsKey(allFactionsListForReading[i]))
				{
					this.pawnsInFactionSpawned.Add(allFactionsListForReading[i], new List<Pawn>());
				}
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003A598 File Offset: 0x00038798
		public List<Pawn> PawnsInFaction(Faction faction)
		{
			if (faction == null)
			{
				Log.Error("Called PawnsInFaction with null faction.", false);
				return new List<Pawn>();
			}
			List<Pawn> list;
			if (!this.pawnsInFactionResult.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				this.pawnsInFactionResult.Add(faction, list);
			}
			list.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction)
				{
					list.Add(allPawns[i]);
				}
			}
			return list;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003A617 File Offset: 0x00038817
		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			if (faction == null)
			{
				Log.Error("Called SpawnedPawnsInFaction with null faction.", false);
				return new List<Pawn>();
			}
			return this.pawnsInFactionSpawned[faction];
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0003A640 File Offset: 0x00038840
		public List<Pawn> FreeHumanlikesOfFaction(Faction faction)
		{
			List<Pawn> list;
			if (!this.freeHumanlikesOfFactionResult.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				this.freeHumanlikesOfFactionResult.Add(faction, list);
			}
			list.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction && allPawns[i].HostFaction == null && allPawns[i].RaceProps.Humanlike)
				{
					list.Add(allPawns[i]);
				}
			}
			return list;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0003A6CC File Offset: 0x000388CC
		public List<Pawn> FreeHumanlikesSpawnedOfFaction(Faction faction)
		{
			List<Pawn> list;
			if (!this.freeHumanlikesSpawnedOfFactionResult.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				this.freeHumanlikesSpawnedOfFactionResult.Add(faction, list);
			}
			list.Clear();
			List<Pawn> list2 = this.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list2.Count; i++)
			{
				if (list2[i].HostFaction == null && list2[i].RaceProps.Humanlike)
				{
					list.Add(list2[i]);
				}
			}
			return list;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0003A74C File Offset: 0x0003894C
		public void RegisterPawn(Pawn p)
		{
			if (p.Dead)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register dead pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}), false);
				return;
			}
			if (!p.Spawned)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register despawned pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}), false);
				return;
			}
			if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + p + " but his Map is not this one.", false);
				return;
			}
			if (!p.mindState.Active)
			{
				return;
			}
			this.EnsureFactionsListsInit();
			if (!this.pawnsSpawned.Contains(p))
			{
				this.pawnsSpawned.Add(p);
			}
			if (p.Faction != null && !this.pawnsInFactionSpawned[p.Faction].Contains(p))
			{
				this.pawnsInFactionSpawned[p.Faction].Add(p);
				if (p.Faction == Faction.OfPlayer)
				{
					this.pawnsInFactionSpawned[Faction.OfPlayer].InsertionSort(delegate(Pawn a, Pawn b)
					{
						int num = (a.playerSettings != null) ? a.playerSettings.joinTick : 0;
						int value = (b.playerSettings != null) ? b.playerSettings.joinTick : 0;
						return num.CompareTo(value);
					});
				}
			}
			if (p.IsPrisonerOfColony && !this.prisonersOfColonySpawned.Contains(p))
			{
				this.prisonersOfColonySpawned.Add(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0003A8CC File Offset: 0x00038ACC
		public void DeRegisterPawn(Pawn p)
		{
			this.EnsureFactionsListsInit();
			this.pawnsSpawned.Remove(p);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				Faction key = allFactionsListForReading[i];
				this.pawnsInFactionSpawned[key].Remove(p);
			}
			this.prisonersOfColonySpawned.Remove(p);
			this.DoListChangedNotifications();
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0003A936 File Offset: 0x00038B36
		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0003A962 File Offset: 0x00038B62
		private void DoListChangedNotifications()
		{
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0003A97C File Offset: 0x00038B7C
		public void LogListedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MapPawns:");
			stringBuilder.AppendLine("pawnsSpawned");
			foreach (Pawn pawn in this.pawnsSpawned)
			{
				stringBuilder.AppendLine("    " + pawn.ToString());
			}
			stringBuilder.AppendLine("AllPawnsUnspawned");
			foreach (Pawn pawn2 in this.AllPawnsUnspawned)
			{
				stringBuilder.AppendLine("    " + pawn2.ToString());
			}
			foreach (KeyValuePair<Faction, List<Pawn>> keyValuePair in this.pawnsInFactionSpawned)
			{
				stringBuilder.AppendLine("pawnsInFactionSpawned[" + keyValuePair.Key.ToString() + "]");
				foreach (Pawn pawn3 in keyValuePair.Value)
				{
					stringBuilder.AppendLine("    " + pawn3.ToString());
				}
			}
			stringBuilder.AppendLine("prisonersOfColonySpawned");
			foreach (Pawn pawn4 in this.prisonersOfColonySpawned)
			{
				stringBuilder.AppendLine("    " + pawn4.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04000895 RID: 2197
		private Map map;

		// Token: 0x04000896 RID: 2198
		private List<Pawn> pawnsSpawned = new List<Pawn>();

		// Token: 0x04000897 RID: 2199
		private Dictionary<Faction, List<Pawn>> pawnsInFactionSpawned = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04000898 RID: 2200
		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		// Token: 0x04000899 RID: 2201
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400089A RID: 2202
		private List<Pawn> allPawnsResult = new List<Pawn>();

		// Token: 0x0400089B RID: 2203
		private List<Pawn> allPawnsUnspawnedResult = new List<Pawn>();

		// Token: 0x0400089C RID: 2204
		private List<Pawn> prisonersOfColonyResult = new List<Pawn>();

		// Token: 0x0400089D RID: 2205
		private List<Pawn> freeColonistsAndPrisonersResult = new List<Pawn>();

		// Token: 0x0400089E RID: 2206
		private List<Pawn> freeColonistsAndPrisonersSpawnedResult = new List<Pawn>();

		// Token: 0x0400089F RID: 2207
		private List<Pawn> spawnedPawnsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x040008A0 RID: 2208
		private List<Pawn> spawnedHungryPawnsResult = new List<Pawn>();

		// Token: 0x040008A1 RID: 2209
		private List<Pawn> spawnedDownedPawnsResult = new List<Pawn>();

		// Token: 0x040008A2 RID: 2210
		private List<Pawn> spawnedPawnsWhoShouldHaveSurgeryDoneNowResult = new List<Pawn>();

		// Token: 0x040008A3 RID: 2211
		private List<Pawn> spawnedPawnsWhoShouldHaveInventoryUnloadedResult = new List<Pawn>();

		// Token: 0x040008A4 RID: 2212
		private Dictionary<Faction, List<Pawn>> pawnsInFactionResult = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x040008A5 RID: 2213
		private Dictionary<Faction, List<Pawn>> freeHumanlikesOfFactionResult = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x040008A6 RID: 2214
		private Dictionary<Faction, List<Pawn>> freeHumanlikesSpawnedOfFactionResult = new Dictionary<Faction, List<Pawn>>();
	}
}
