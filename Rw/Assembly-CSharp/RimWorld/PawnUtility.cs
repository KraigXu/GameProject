using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B2F RID: 2863
	public static class PawnUtility
	{
		// Token: 0x06004354 RID: 17236 RVA: 0x0016AE6C File Offset: 0x0016906C
		public static Faction GetFactionLeaderFaction(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].leader == pawn)
				{
					return allFactionsListForReading[i];
				}
			}
			return null;
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x0016AEAD File Offset: 0x001690AD
		public static bool IsFactionLeader(Pawn pawn)
		{
			return PawnUtility.GetFactionLeaderFaction(pawn) != null;
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x0016AEB8 File Offset: 0x001690B8
		public static bool IsInteractionBlocked(this Pawn pawn, InteractionDef interaction, bool isInitiator, bool isRandom)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			if (mentalStateDef == null)
			{
				return false;
			}
			if (isRandom)
			{
				return mentalStateDef.blockRandomInteraction;
			}
			if (interaction == null)
			{
				return false;
			}
			List<InteractionDef> list = isInitiator ? mentalStateDef.blockInteractionInitiationExcept : mentalStateDef.blockInteractionRecipientExcept;
			return list != null && !list.Contains(interaction);
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x0016AF04 File Offset: 0x00169104
		public static bool IsKidnappedPawn(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x0016AF49 File Offset: 0x00169149
		public static bool IsTravelingInTransportPodWorldObject(Pawn pawn)
		{
			return (pawn.IsWorldPawn() && ThingOwnerUtility.AnyParentIs<ActiveDropPodInfo>(pawn)) || ThingOwnerUtility.AnyParentIs<TravelingTransportPods>(pawn);
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x0016AF63 File Offset: 0x00169163
		public static bool ForSaleBySettlement(Pawn pawn)
		{
			return pawn.ParentHolder is Settlement_TraderTracker;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x0016AF74 File Offset: 0x00169174
		public static bool IsInvisible(this Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].TryGetComp<HediffComp_Invisibility>() != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0016AFB4 File Offset: 0x001691B4
		public static void TryDestroyStartingColonistFamily(Pawn pawn)
		{
			if (!pawn.relations.RelatedPawns.Any((Pawn x) => Find.GameInitData.startingAndOptionalPawns.Contains(x)))
			{
				PawnUtility.DestroyStartingColonistFamily(pawn);
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0016AFF0 File Offset: 0x001691F0
		public static void DestroyStartingColonistFamily(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns.ToList<Pawn>())
			{
				if (!Find.GameInitData.startingAndOptionalPawns.Contains(pawn2))
				{
					WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn2);
					if (situation == WorldPawnSituation.Free || situation == WorldPawnSituation.Dead)
					{
						Find.WorldPawns.RemovePawn(pawn2);
						Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					}
				}
			}
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x0016B084 File Offset: 0x00169284
		public static bool EnemiesAreNearby(Pawn pawn, int regionsToScan = 9, bool passDoors = false)
		{
			TraverseParms tp = passDoors ? TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false) : TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			bool foundEnemy = false;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].HostileTo(pawn))
					{
						foundEnemy = true;
						return true;
					}
				}
				return foundEnemy;
			}, regionsToScan, RegionType.Set_Passable);
			return foundEnemy;
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0016B104 File Offset: 0x00169304
		public static bool WillSoonHaveBasicNeed(Pawn p)
		{
			return p.needs != null && ((p.needs.rest != null && p.needs.rest.CurLevel < 0.33f) || (p.needs.food != null && p.needs.food.CurLevelPercentage < p.needs.food.PercentageThreshHungry + 0.05f));
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x0016B179 File Offset: 0x00169379
		public static float AnimalFilthChancePerCell(ThingDef def, float bodySize)
		{
			return bodySize * 0.00125f * (1f - def.race.petness);
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x0016B194 File Offset: 0x00169394
		public static float HumanFilthChancePerCell(ThingDef def, float bodySize)
		{
			return bodySize * 0.00125f * 4f;
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0016B1A4 File Offset: 0x001693A4
		public static bool CanCasuallyInteractNow(this Pawn p, bool twoWayInteraction = false)
		{
			if (p.Drafted)
			{
				return false;
			}
			if (p.IsInvisible())
			{
				return false;
			}
			if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p))
			{
				return false;
			}
			if (p.InAggroMentalState)
			{
				return false;
			}
			if (!p.Awake())
			{
				return false;
			}
			if (p.IsFormingCaravan())
			{
				return false;
			}
			Job curJob = p.CurJob;
			return curJob == null || !twoWayInteraction || (curJob.def.casualInterruptible && curJob.playerForced);
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x0016B212 File Offset: 0x00169412
		public static IEnumerable<Pawn> SpawnedMasteredPawns(Pawn master)
		{
			if (Current.ProgramState != ProgramState.Playing || master.Faction == null || !master.RaceProps.Humanlike)
			{
				yield break;
			}
			if (!master.Spawned)
			{
				yield break;
			}
			List<Pawn> pawns = master.Map.mapPawns.SpawnedPawnsInFaction(master.Faction);
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].playerSettings != null && pawns[i].playerSettings.Master == master)
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0016B222 File Offset: 0x00169422
		public static bool InValidState(Pawn p)
		{
			return p.health != null && (p.Dead || (p.stances != null && p.mindState != null && p.needs != null && p.ageTracker != null));
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0016B25C File Offset: 0x0016945C
		public static PawnPosture GetPosture(this Pawn p)
		{
			if (p.Dead)
			{
				return PawnPosture.LayingOnGroundNormal;
			}
			if (p.Downed)
			{
				if (p.jobs != null && p.jobs.posture.Laying())
				{
					return p.jobs.posture;
				}
				return PawnPosture.LayingOnGroundNormal;
			}
			else
			{
				if (p.jobs == null)
				{
					return PawnPosture.Standing;
				}
				return p.jobs.posture;
			}
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0016B2B8 File Offset: 0x001694B8
		public static void ForceWait(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false)
		{
			if (ticks <= 0)
			{
				Log.ErrorOnce("Forcing a wait for zero ticks", 47045639, false);
			}
			Job job = JobMaker.MakeJob(maintainPosture ? JobDefOf.Wait_MaintainPosture : JobDefOf.Wait, faceTarget);
			job.expiryInterval = ticks;
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false);
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x0016B318 File Offset: 0x00169518
		public static void GiveNameBecauseOfNuzzle(Pawn namer, Pawn namee)
		{
			string value = (namee.Name == null) ? namee.LabelIndefinite() : namee.Name.ToStringFull;
			namee.Name = PawnBioAndNameGenerator.GeneratePawnName(namee, NameStyle.Full, null);
			if (namer.Faction == Faction.OfPlayer)
			{
				Messages.Message("MessageNuzzledPawnGaveNameTo".Translate(namer.Named("NAMER"), value, namee.Name.ToStringFull, namee.Named("NAMEE")), namee, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0016B3A8 File Offset: 0x001695A8
		public static void GainComfortFromCellIfPossible(this Pawn p, bool chairsOnly = false)
		{
			if (Find.TickManager.TicksGame % 10 == 0)
			{
				Building edifice = p.Position.GetEdifice(p.Map);
				if (edifice != null && (!chairsOnly || (edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable)))
				{
					PawnUtility.GainComfortFromThingIfPossible(p, edifice);
				}
			}
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0016B404 File Offset: 0x00169604
		public static void GainComfortFromThingIfPossible(Pawn p, Thing from)
		{
			if (Find.TickManager.TicksGame % 10 == 0)
			{
				float statValue = from.GetStatValue(StatDefOf.Comfort, true);
				if (statValue >= 0f && p.needs != null && p.needs.comfort != null)
				{
					p.needs.comfort.ComfortUsed(statValue);
				}
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0016B45C File Offset: 0x0016965C
		public static float BodyResourceGrowthSpeed(Pawn pawn)
		{
			if (pawn.needs != null && pawn.needs.food != null)
			{
				switch (pawn.needs.food.CurCategory)
				{
				case HungerCategory.Fed:
					return 1f;
				case HungerCategory.Hungry:
					return 0.666f;
				case HungerCategory.UrgentlyHungry:
					return 0.333f;
				case HungerCategory.Starving:
					return 0f;
				}
			}
			return 1f;
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0016B4C4 File Offset: 0x001696C4
		public static bool FertileMateTarget(Pawn male, Pawn female)
		{
			if (female.gender != Gender.Female || !female.ageTracker.CurLifeStage.reproductive)
			{
				return false;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			if (compEggLayer != null)
			{
				return !compEggLayer.FullyFertilized;
			}
			return !female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false);
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0016B51C File Offset: 0x0016971C
		public static void Mated(Pawn male, Pawn female)
		{
			if (!female.ageTracker.CurLifeStage.reproductive)
			{
				return;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			if (compEggLayer != null)
			{
				compEggLayer.Fertilize(male);
				return;
			}
			if (Rand.Value < 0.5f && !female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false))
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, female, null);
				hediff_Pregnant.father = male;
				female.health.AddHediff(hediff_Pregnant, null, null, null);
			}
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x0016B5A4 File Offset: 0x001697A4
		public static bool PlayerForcedJobNowOrSoon(Pawn pawn)
		{
			if (pawn.jobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob != null)
			{
				return curJob.playerForced;
			}
			return pawn.jobs.jobQueue.Any<QueuedJob>() && pawn.jobs.jobQueue.Peek().job.playerForced;
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0016B5FC File Offset: 0x001697FC
		public static bool TrySpawnHatchedOrBornPawn(Pawn pawn, Thing motherOrEgg)
		{
			if (motherOrEgg.SpawnedOrAnyParentSpawned)
			{
				return GenSpawn.Spawn(pawn, motherOrEgg.PositionHeld, motherOrEgg.MapHeld, WipeMode.Vanish) != null;
			}
			Pawn pawn2 = motherOrEgg as Pawn;
			if (pawn2 != null)
			{
				if (pawn2.IsCaravanMember())
				{
					pawn2.GetCaravan().AddPawn(pawn, true);
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
				if (pawn2.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
			}
			else if (motherOrEgg.ParentHolder != null)
			{
				Pawn_InventoryTracker pawn_InventoryTracker = motherOrEgg.ParentHolder as Pawn_InventoryTracker;
				if (pawn_InventoryTracker != null)
				{
					if (pawn_InventoryTracker.pawn.IsCaravanMember())
					{
						pawn_InventoryTracker.pawn.GetCaravan().AddPawn(pawn, true);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
					if (pawn_InventoryTracker.pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0016B6CC File Offset: 0x001698CC
		public static ByteGrid GetAvoidGrid(this Pawn p, bool onlyIfLordAllows = true)
		{
			if (!p.Spawned)
			{
				return null;
			}
			if (p.Faction == null)
			{
				return null;
			}
			if (!p.Faction.def.canUseAvoidGrid)
			{
				return null;
			}
			if (p.Faction == Faction.OfPlayer || !p.Faction.HostileTo(Faction.OfPlayer))
			{
				return null;
			}
			if (!onlyIfLordAllows)
			{
				return p.Map.avoidGrid.Grid;
			}
			Lord lord = p.GetLord();
			if (lord != null && lord.CurLordToil.useAvoidGrid)
			{
				return lord.Map.avoidGrid.Grid;
			}
			return null;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0016B75F File Offset: 0x0016995F
		public static bool ShouldCollideWithPawns(Pawn p)
		{
			return !p.Downed && !p.Dead && p.mindState.anyCloseHostilesRecently;
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0016B783 File Offset: 0x00169983
		public static bool AnyPawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false)
		{
			return PawnUtility.PawnBlockingPathAt(c, forPawn, actAsIfHadCollideWithPawnsJob, collideOnlyWithStandingPawns, forPathFinder) != null;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0016B794 File Offset: 0x00169994
		public static Pawn PawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false)
		{
			List<Thing> thingList = c.GetThingList(forPawn.Map);
			if (thingList.Count == 0)
			{
				return null;
			}
			bool flag = false;
			if (actAsIfHadCollideWithPawnsJob)
			{
				flag = true;
			}
			else
			{
				Job curJob = forPawn.CurJob;
				if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns || forPawn.jobs.curDriver.collideWithPawns))
				{
					flag = true;
				}
				else if (forPawn.Drafted)
				{
					bool moving = forPawn.pather.Moving;
				}
			}
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null && pawn != forPawn && !pawn.Downed && (!collideOnlyWithStandingPawns || (!pawn.pather.MovingNow && (!pawn.pather.Moving || !pawn.pather.MovedRecently(60)))) && !PawnUtility.PawnsCanShareCellBecauseOfBodySize(pawn, forPawn))
				{
					if (pawn.HostileTo(forPawn))
					{
						return pawn;
					}
					if (flag && (forPathFinder || !forPawn.Drafted || !pawn.RaceProps.Animal))
					{
						Job curJob2 = pawn.CurJob;
						if (curJob2 != null && (curJob2.collideWithPawns || curJob2.def.collideWithPawns || pawn.jobs.curDriver.collideWithPawns))
						{
							return pawn;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x0016B8EC File Offset: 0x00169AEC
		private static bool PawnsCanShareCellBecauseOfBodySize(Pawn p1, Pawn p2)
		{
			if (p1.BodySize >= 1.5f || p2.BodySize >= 1.5f)
			{
				return false;
			}
			float num = p1.BodySize / p2.BodySize;
			if (num < 1f)
			{
				num = 1f / num;
			}
			return num > 3.57f;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x0016B93C File Offset: 0x00169B3C
		public static bool KnownDangerAt(IntVec3 c, Map map, Pawn forPawn)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.IsDangerousFor(forPawn);
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x0016B960 File Offset: 0x00169B60
		public static bool ShouldSendNotificationAbout(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (PawnGenerator.IsBeingGenerated(p))
			{
				return false;
			}
			if (p.IsWorldPawn() && (!p.IsCaravanMember() || !p.GetCaravan().IsPlayerControlled) && !PawnUtility.IsTravelingInTransportPodWorldObject(p) && !p.IsBorrowedByAnyFaction() && p.Corpse.DestroyedOrNull())
			{
				return false;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				if (p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (p.RaceProps.Humanlike && p.guest.Released && !p.Downed && !p.InBed())
				{
					return false;
				}
				if (p.CurJob != null && p.CurJob.exitMapOnArrival && !PrisonBreakUtility.IsPrisonBreaking(p))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0016BA25 File Offset: 0x00169C25
		public static bool ShouldGetThoughtAbout(Pawn pawn, Pawn subject)
		{
			return pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn());
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x0016BA4A File Offset: 0x00169C4A
		public static bool IsTeetotaler(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) < 0;
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x0016BA6E File Offset: 0x00169C6E
		public static bool IsProsthophobe(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.BodyPurist);
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0016BA8F File Offset: 0x00169C8F
		public static bool IsPrisonerInPrisonCell(this Pawn pawn)
		{
			return pawn.IsPrisoner && pawn.Spawned && pawn.Position.IsInPrisonCell(pawn.Map);
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x0016BAB4 File Offset: 0x00169CB4
		public static string PawnKindsToCommaList(IEnumerable<Pawn> pawns, bool useAnd = false)
		{
			PawnUtility.tmpPawns.Clear();
			PawnUtility.tmpPawns.AddRange(pawns);
			if (PawnUtility.tmpPawns.Count >= 2)
			{
				PawnUtility.tmpPawns.SortBy((Pawn x) => !x.RaceProps.Humanlike, (Pawn x) => x.GetKindLabelPlural(-1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawns.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawns[i].kindDef))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawns[i].kindDef);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawns.Count; j++)
					{
						if (PawnUtility.tmpPawns[j].kindDef == PawnUtility.tmpPawns[i].kindDef)
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + PawnUtility.tmpPawns[i].KindLabel);
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num + " " + PawnUtility.tmpPawns[i].GetKindLabelPlural(num));
					}
				}
			}
			PawnUtility.tmpPawns.Clear();
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd);
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0016BC3C File Offset: 0x00169E3C
		public static List<string> PawnKindsToList(IEnumerable<PawnKindDef> pawnKinds)
		{
			PawnUtility.tmpPawnKinds.Clear();
			PawnUtility.tmpPawnKinds.AddRange(pawnKinds);
			if (PawnUtility.tmpPawnKinds.Count >= 2)
			{
				PawnUtility.tmpPawnKinds.SortBy((PawnKindDef x) => !x.RaceProps.Humanlike, (PawnKindDef x) => GenLabel.BestKindLabel(x, Gender.None, true, -1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawnKinds.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawnKinds[i]))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawnKinds[i]);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawnKinds.Count; j++)
					{
						if (PawnUtility.tmpPawnKinds[j] == PawnUtility.tmpPawnKinds[i])
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, false, -1));
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num + " " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, true, num));
					}
				}
			}
			return PawnUtility.tmpPawnKindsStr;
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0016BDA4 File Offset: 0x00169FA4
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0016BDB8 File Offset: 0x00169FB8
		public static string PawnKindsToCommaList(IEnumerable<PawnKindDef> pawnKinds, bool useAnd = false)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd);
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x0016BDCC File Offset: 0x00169FCC
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.locomotion != LocomotionUrgency.None)
			{
				return pawn.mindState.duty.locomotion;
			}
			return secondPriority;
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x0016BE08 File Offset: 0x0016A008
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority, LocomotionUrgency thirdPriority)
		{
			LocomotionUrgency locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, secondPriority);
			if (locomotionUrgency != LocomotionUrgency.None)
			{
				return locomotionUrgency;
			}
			return thirdPriority;
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0016BE23 File Offset: 0x0016A023
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.maxDanger != Danger.Unspecified)
			{
				return pawn.mindState.duty.maxDanger;
			}
			return secondPriority;
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x0016BE60 File Offset: 0x0016A060
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority, Danger thirdPriority)
		{
			Danger danger = PawnUtility.ResolveMaxDanger(pawn, secondPriority);
			if (danger != Danger.Unspecified)
			{
				return danger;
			}
			return thirdPriority;
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0016BE7C File Offset: 0x0016A07C
		public static bool IsFighting(this Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic || pawn.CurJob.def == JobDefOf.Wait_Combat || pawn.CurJob.def == JobDefOf.PredatorHunt);
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0016BEDD File Offset: 0x0016A0DD
		public static Hediff_Psylink GetMainPsylinkSource(this Pawn pawn)
		{
			return (Hediff_Psylink)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0016BEFC File Offset: 0x0016A0FC
		public static int GetPsylinkLevel(this Pawn pawn)
		{
			Hediff_Psylink mainPsylinkSource = pawn.GetMainPsylinkSource();
			if (mainPsylinkSource == null)
			{
				return 0;
			}
			return mainPsylinkSource.level;
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x0016BF1B File Offset: 0x0016A11B
		public static int GetMaxPsylinkLevel(this Pawn pawn)
		{
			return (int)HediffDefOf.PsychicAmplifier.maxSeverity;
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x0016BF28 File Offset: 0x0016A128
		public static void ChangePsylinkLevel(this Pawn pawn, int levelOffset)
		{
			Hediff_Psylink hediff_Psylink = pawn.GetMainPsylinkSource();
			if (hediff_Psylink == null)
			{
				hediff_Psylink = (Hediff_Psylink)pawn.health.AddHediff(HediffDefOf.PsychicAmplifier, pawn.health.hediffSet.GetBrain(), null, null);
				return;
			}
			hediff_Psylink.ChangeLevel(levelOffset);
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x0016BF78 File Offset: 0x0016A178
		public static float RecruitDifficulty(this Pawn pawn, Faction recruiterFaction)
		{
			float num = pawn.kindDef.baseRecruitDifficulty;
			Rand.PushState();
			Rand.Seed = pawn.HashOffset();
			num += Rand.Gaussian(0f, 0.15f);
			Rand.PopState();
			if (pawn.Faction != null)
			{
				int num2 = Mathf.Min((int)pawn.Faction.def.techLevel, 4);
				int num3 = Mathf.Min((int)recruiterFaction.def.techLevel, 4);
				int num4 = Mathf.Abs(num2 - num3);
				num += (float)num4 * 0.16f;
			}
			if (pawn.royalty != null)
			{
				RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
				if (mostSeniorTitle != null)
				{
					num += mostSeniorTitle.def.recruitmentDifficultyOffset;
				}
			}
			return Mathf.Clamp(num, 0.1f, 0.99f);
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x0016C034 File Offset: 0x0016A234
		public static void GiveAllStartingPlayerPawnsThought(ThoughtDef thought)
		{
			foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
			{
				if (pawn.needs.mood != null)
				{
					if (thought.IsSocial)
					{
						using (List<Pawn>.Enumerator enumerator2 = Find.GameInitData.startingAndOptionalPawns.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Pawn pawn2 = enumerator2.Current;
								if (pawn2 != pawn)
								{
									pawn.needs.mood.thoughts.memories.TryGainMemory(thought, pawn2);
								}
							}
							continue;
						}
					}
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought, null);
				}
			}
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0016C11C File Offset: 0x0016A31C
		public static IntVec3 DutyLocation(this Pawn pawn)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid)
			{
				return pawn.mindState.duty.focus.Cell;
			}
			return pawn.Position;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x0016C169 File Offset: 0x0016A369
		public static bool EverBeenColonistOrTameAnimal(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsColonistOrColonyAnimal) > 0;
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x0016C17E File Offset: 0x0016A37E
		public static bool EverBeenPrisoner(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsPrisoner) > 0;
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x0016C193 File Offset: 0x0016A393
		public static bool EverBeenQuestLodger(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsQuestLodger) > 0;
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0016C1A8 File Offset: 0x0016A3A8
		public static void RecoverFromUnwalkablePositionOrKill(IntVec3 c, Map map)
		{
			if (!c.InBounds(map) || c.Walkable(map))
			{
				return;
			}
			PawnUtility.tmpThings.Clear();
			PawnUtility.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < PawnUtility.tmpThings.Count; i++)
			{
				Pawn pawn = PawnUtility.tmpThings[i] as Pawn;
				if (pawn != null)
				{
					IntVec3 position;
					if (CellFinder.TryFindBestPawnStandCell(pawn, out position, false))
					{
						pawn.Position = position;
						pawn.Notify_Teleported(true, false);
					}
					else
					{
						DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, pawn.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.Collapse, null);
						pawn.TakeDamage(damageInfo);
						if (!pawn.Dead)
						{
							pawn.Kill(new DamageInfo?(damageInfo), null);
						}
					}
				}
			}
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0016C27C File Offset: 0x0016A47C
		public static float GetManhunterOnDamageChance(Pawn pawn, float distance, Thing instigator)
		{
			float num = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			num *= GenMath.LerpDoubleClamped(1f, 30f, 3f, 1f, distance);
			if (instigator != null)
			{
				num *= 1f - instigator.GetStatValue(StatDefOf.HuntingStealth, true);
			}
			return num;
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x0016C2CB File Offset: 0x0016A4CB
		public static float GetManhunterOnDamageChance(Pawn pawn, Thing instigator = null)
		{
			if (instigator != null)
			{
				return PawnUtility.GetManhunterOnDamageChance(pawn, pawn.Position.DistanceTo(instigator.Position), instigator);
			}
			return PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0016C2F4 File Offset: 0x0016A4F4
		public static float GetManhunterOnDamageChance(PawnKindDef kind)
		{
			return kind.RaceProps.manhunterOnDamageChance * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0016C311 File Offset: 0x0016A511
		public static float GetManhunterOnDamageChance(RaceProperties race)
		{
			return race.manhunterOnDamageChance * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		// Token: 0x040026B4 RID: 9908
		private const float HumanFilthFactor = 4f;

		// Token: 0x040026B5 RID: 9909
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040026B6 RID: 9910
		private static List<string> tmpPawnKindsStr = new List<string>();

		// Token: 0x040026B7 RID: 9911
		private static HashSet<PawnKindDef> tmpAddedPawnKinds = new HashSet<PawnKindDef>();

		// Token: 0x040026B8 RID: 9912
		private static List<PawnKindDef> tmpPawnKinds = new List<PawnKindDef>();

		// Token: 0x040026B9 RID: 9913
		private const float RecruitDifficultyMin = 0.1f;

		// Token: 0x040026BA RID: 9914
		private const float RecruitDifficultyMax = 0.99f;

		// Token: 0x040026BB RID: 9915
		private const float RecruitDifficultyGaussianWidthFactor = 0.15f;

		// Token: 0x040026BC RID: 9916
		private const float RecruitDifficultyOffsetPerTechDiff = 0.16f;

		// Token: 0x040026BD RID: 9917
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
