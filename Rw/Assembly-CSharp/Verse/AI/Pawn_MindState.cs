using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x0200057F RID: 1407
	public class Pawn_MindState : IExposable
	{
		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060027F1 RID: 10225 RVA: 0x000EBA51 File Offset: 0x000E9C51
		public bool AvailableForGoodwillReward
		{
			get
			{
				return Find.TickManager.TicksGame >= this.noAidRelationsGainUntilTick;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x000EBA68 File Offset: 0x000E9C68
		// (set) Token: 0x060027F3 RID: 10227 RVA: 0x000EBA70 File Offset: 0x000E9C70
		public bool Active
		{
			get
			{
				return this.activeInt;
			}
			set
			{
				if (value != this.activeInt)
				{
					this.activeInt = value;
					if (this.pawn.Spawned)
					{
						this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
					}
				}
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000EBAAA File Offset: 0x000E9CAA
		public bool IsIdle
		{
			get
			{
				return !this.pawn.Downed && this.pawn.Spawned && this.lastJobTag == JobTag.Idle;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060027F5 RID: 10229 RVA: 0x000EBAD4 File Offset: 0x000E9CD4
		public bool MeleeThreatStillThreat
		{
			get
			{
				return this.meleeThreat != null && this.meleeThreat.Spawned && !this.meleeThreat.Downed && this.pawn.Spawned && Find.TickManager.TicksGame <= this.lastMeleeThreatHarmTick + 400 && (float)(this.pawn.Position - this.meleeThreat.Position).LengthHorizontalSquared <= 9f && GenSight.LineOfSight(this.pawn.Position, this.meleeThreat.Position, this.pawn.Map, false, null, 0, 0);
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000EBB85 File Offset: 0x000E9D85
		// (set) Token: 0x060027F7 RID: 10231 RVA: 0x000EBB8D File Offset: 0x000E9D8D
		public bool WildManEverReachedOutside
		{
			get
			{
				return this.wildManEverReachedOutsideInt;
			}
			set
			{
				if (this.wildManEverReachedOutsideInt == value)
				{
					return;
				}
				this.wildManEverReachedOutsideInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000EBBAB File Offset: 0x000E9DAB
		// (set) Token: 0x060027F9 RID: 10233 RVA: 0x000EBBB3 File Offset: 0x000E9DB3
		public bool WillJoinColonyIfRescued
		{
			get
			{
				return this.willJoinColonyIfRescuedInt;
			}
			set
			{
				if (this.willJoinColonyIfRescuedInt == value)
				{
					return;
				}
				this.willJoinColonyIfRescuedInt = value;
				if (this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x000EBBF0 File Offset: 0x000E9DF0
		public bool AnythingPreventsJoiningColonyIfRescued
		{
			get
			{
				return this.pawn.Faction == Faction.OfPlayer || (this.pawn.IsPrisoner && !this.pawn.HostFaction.HostileTo(Faction.OfPlayer)) || (!this.pawn.IsPrisoner && this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer) && !this.pawn.Downed);
			}
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000EBC78 File Offset: 0x000E9E78
		public Pawn_MindState()
		{
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000EBD60 File Offset: 0x000E9F60
		public Pawn_MindState(Pawn pawn)
		{
			this.pawn = pawn;
			this.mentalStateHandler = new MentalStateHandler(pawn);
			this.mentalBreaker = new MentalBreaker(pawn);
			this.inspirationHandler = new InspirationHandler(pawn);
			this.priorityWork = new PriorityWork(pawn);
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000EBE80 File Offset: 0x000EA080
		public void Reset(bool clearInspiration = false, bool clearMentalState = true)
		{
			if (clearMentalState)
			{
				this.mentalStateHandler.Reset();
				this.mentalBreaker.Reset();
			}
			if (clearInspiration)
			{
				this.inspirationHandler.Reset();
			}
			this.activeInt = true;
			this.lastJobTag = JobTag.Misc;
			this.lastIngestTick = -99999;
			this.nextApparelOptimizeTick = -99999;
			this.canFleeIndividual = true;
			this.exitMapAfterTick = -99999;
			this.lastDisturbanceTick = -99999;
			this.forcedGotoPosition = IntVec3.Invalid;
			this.knownExploder = null;
			this.wantsToTradeWithColony = false;
			this.lastMannedThing = null;
			this.canLovinTick = -99999;
			this.canSleepTick = -99999;
			this.meleeThreat = null;
			this.lastMeleeThreatHarmTick = -99999;
			this.lastEngageTargetTick = -99999;
			this.lastAttackTargetTick = -99999;
			this.lastAttackedTarget = LocalTargetInfo.Invalid;
			this.enemyTarget = null;
			this.duty = null;
			this.thinkData.Clear();
			this.lastAssignedInteractTime = -99999;
			this.interactionsToday = 0;
			this.lastInventoryRawFoodUseTick = 0;
			this.priorityWork.Clear();
			this.nextMoveOrderIsWait = true;
			this.lastTakeCombatEnhancingDrugTick = -99999;
			this.lastHarmTick = -99999;
			this.anyCloseHostilesRecently = false;
			this.WillJoinColonyIfRescued = false;
			this.WildManEverReachedOutside = false;
			this.timesGuestTendedToByPlayer = 0;
			this.lastSelfTendTick = -99999;
			this.spawnedByInfestationThingComp = false;
			this.lastPredatorHuntingPlayerNotificationTick = -99999;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000EBFF4 File Offset: 0x000EA1F4
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.meleeThreat, "meleeThreat", false);
			Scribe_References.Look<Thing>(ref this.enemyTarget, "enemyTarget", false);
			Scribe_References.Look<Thing>(ref this.knownExploder, "knownExploder", false);
			Scribe_References.Look<Thing>(ref this.lastMannedThing, "lastMannedThing", false);
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Collections.Look<int, int>(ref this.thinkData, "thinkData", LookMode.Value, LookMode.Value);
			Scribe_Values.Look<bool>(ref this.activeInt, "active", true, false);
			Scribe_Values.Look<JobTag>(ref this.lastJobTag, "lastJobTag", JobTag.Misc, false);
			Scribe_Values.Look<int>(ref this.lastIngestTick, "lastIngestTick", -99999, false);
			Scribe_Values.Look<int>(ref this.nextApparelOptimizeTick, "nextApparelOptimizeTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastEngageTargetTick, "lastEngageTargetTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canFleeIndividual, "canFleeIndividual", false, false);
			Scribe_Values.Look<int>(ref this.exitMapAfterTick, "exitMapAfterTick", -99999, false);
			Scribe_Values.Look<IntVec3>(ref this.forcedGotoPosition, "forcedGotoPosition", IntVec3.Invalid, false);
			Scribe_Values.Look<int>(ref this.lastMeleeThreatHarmTick, "lastMeleeThreatHarmTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAssignedInteractTime, "lastAssignedInteractTime", -99999, false);
			Scribe_Values.Look<int>(ref this.interactionsToday, "interactionsToday", 0, false);
			Scribe_Values.Look<int>(ref this.lastInventoryRawFoodUseTick, "lastInventoryRawFoodUseTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastDisturbanceTick, "lastDisturbanceTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.wantsToTradeWithColony, "wantsToTradeWithColony", false, false);
			Scribe_Values.Look<int>(ref this.canLovinTick, "canLovinTick", -99999, false);
			Scribe_Values.Look<int>(ref this.canSleepTick, "canSleepTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.nextMoveOrderIsWait, "nextMoveOrderIsWait", true, false);
			Scribe_Values.Look<int>(ref this.lastTakeCombatEnhancingDrugTick, "lastTakeCombatEnhancingDrugTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastHarmTick, "lastHarmTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.anyCloseHostilesRecently, "anyCloseHostilesRecently", false, false);
			Scribe_Deep.Look<PawnDuty>(ref this.duty, "duty", Array.Empty<object>());
			Scribe_Deep.Look<MentalStateHandler>(ref this.mentalStateHandler, "mentalStateHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<MentalBreaker>(ref this.mentalBreaker, "mentalBreaker", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<InspirationHandler>(ref this.inspirationHandler, "inspirationHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PriorityWork>(ref this.priorityWork, "priorityWork", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.applyBedThoughtsTick, "applyBedThoughtsTick", 0, false);
			Scribe_Values.Look<int>(ref this.applyThroneThoughtsTick, "applyThroneThoughtsTick", 0, false);
			Scribe_Values.Look<bool>(ref this.applyBedThoughtsOnLeave, "applyBedThoughtsOnLeave", false, false);
			Scribe_Values.Look<bool>(ref this.willJoinColonyIfRescuedInt, "willJoinColonyIfRescued", false, false);
			Scribe_Values.Look<bool>(ref this.wildManEverReachedOutsideInt, "wildManEverReachedOutside", false, false);
			Scribe_Values.Look<int>(ref this.timesGuestTendedToByPlayer, "timesGuestTendedToByPlayer", 0, false);
			Scribe_Values.Look<int>(ref this.noAidRelationsGainUntilTick, "noAidRelationsGainUntilTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastSelfTendTick, "lastSelfTendTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
			Scribe_Values.Look<int>(ref this.lastPredatorHuntingPlayerNotificationTick, "lastPredatorHuntingPlayerNotificationTick", -99999, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000EC35C File Offset: 0x000EA55C
		public void MindStateTick()
		{
			if (this.wantsToTradeWithColony)
			{
				TradeUtility.CheckInteractWithTradersTeachOpportunity(this.pawn);
			}
			if (this.meleeThreat != null && !this.MeleeThreatStillThreat)
			{
				this.meleeThreat = null;
			}
			this.mentalStateHandler.MentalStateHandlerTick();
			this.mentalBreaker.MentalBreakerTick();
			this.inspirationHandler.InspirationHandlerTick();
			if (!this.pawn.GetPosture().Laying())
			{
				this.applyBedThoughtsTick = 0;
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				if (this.pawn.Spawned)
				{
					int regionsToScan = this.anyCloseHostilesRecently ? 24 : 18;
					this.anyCloseHostilesRecently = PawnUtility.EnemiesAreNearby(this.pawn, regionsToScan, true);
				}
				else
				{
					this.anyCloseHostilesRecently = false;
				}
			}
			if (this.WillJoinColonyIfRescued && this.AnythingPreventsJoiningColonyIfRescued)
			{
				this.WillJoinColonyIfRescued = false;
			}
			if (this.pawn.Spawned && this.pawn.IsWildMan() && !this.WildManEverReachedOutside && this.pawn.GetRoom(RegionType.Set_Passable) != null && this.pawn.GetRoom(RegionType.Set_Passable).TouchesMapEdge)
			{
				this.WildManEverReachedOutside = true;
			}
			if (Find.TickManager.TicksGame % 123 == 0 && this.pawn.Spawned && this.pawn.RaceProps.IsFlesh && this.pawn.needs.mood != null)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain.traversedThought != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(terrain.traversedThought);
				}
				WeatherDef curWeatherLerped = this.pawn.Map.weatherManager.CurWeatherLerped;
				if (curWeatherLerped.exposedThought != null && !this.pawn.Position.Roofed(this.pawn.Map))
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(curWeatherLerped.exposedThought);
				}
			}
			if (GenLocalDate.DayTick(this.pawn) == 0)
			{
				this.interactionsToday = 0;
			}
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000EC580 File Offset: 0x000EA780
		public void JoinColonyBecauseRescuedBy(Pawn by)
		{
			this.WillJoinColonyIfRescued = false;
			if (this.AnythingPreventsJoiningColonyIfRescued)
			{
				return;
			}
			InteractionWorker_RecruitAttempt.DoRecruit(by, this.pawn, 1f, false);
			if (this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMeByOfferingHelp, by);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelRescueQuestFinished".Translate(), "LetterRescueQuestFinished".Translate(this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000EC676 File Offset: 0x000EA876
		public void ResetLastDisturbanceTick()
		{
			this.lastDisturbanceTick = -9999999;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000EC683 File Offset: 0x000EA883
		public IEnumerable<Gizmo> GetGizmos()
		{
			IEnumerator<Gizmo> enumerator;
			if (this.pawn.IsColonistPlayerControlled)
			{
				foreach (Gizmo gizmo in this.priorityWork.GetGizmos())
				{
					yield return gizmo;
				}
				enumerator = null;
			}
			foreach (Gizmo gizmo2 in CaravanFormingUtility.GetGizmos(this.pawn))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000EC693 File Offset: 0x000EA893
		public void SetNoAidRelationsGainUntilTick(int tick)
		{
			if (tick > this.noAidRelationsGainUntilTick)
			{
				this.noAidRelationsGainUntilTick = tick;
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000EC6A5 File Offset: 0x000EA8A5
		public void Notify_OutfitChanged()
		{
			this.nextApparelOptimizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000EC6B8 File Offset: 0x000EA8B8
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			this.mentalStateHandler.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn))
			{
				this.lastHarmTick = Find.TickManager.TicksGame;
				if (this.pawn.Spawned)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (!this.mentalStateHandler.InMentalState && dinfo.Instigator != null && (pawn != null || dinfo.Instigator is Building_Turret) && dinfo.Instigator.Faction != null && (dinfo.Instigator.Faction.def.humanlikeFaction || (pawn != null && pawn.def.race.intelligence >= Intelligence.ToolUser)) && this.pawn.Faction == null && (this.pawn.RaceProps.Animal || this.pawn.IsWildMan()) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.PredatorHunt || dinfo.Instigator != ((JobDriver_PredatorHunt)this.pawn.jobs.curDriver).Prey) && Rand.Chance(PawnUtility.GetManhunterOnDamageChance(this.pawn, dinfo.Instigator)))
					{
						this.StartManhunterBecauseOfPawnAction("AnimalManhunterFromDamage");
					}
					else if (dinfo.Instigator != null && dinfo.Def.makesAnimalsFlee && Pawn_MindState.CanStartFleeingBecauseOfPawnAction(this.pawn))
					{
						this.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
					}
				}
				if (this.pawn.GetPosture() != PawnPosture.Standing)
				{
					this.lastDisturbanceTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000EC874 File Offset: 0x000EAA74
		internal void Notify_EngagedTarget()
		{
			this.lastEngageTargetTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000EC886 File Offset: 0x000EAA86
		internal void Notify_AttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000EC8A0 File Offset: 0x000EAAA0
		internal bool CheckStartMentalStateBecauseRecruitAttempted(Pawn tamer)
		{
			if (!this.pawn.RaceProps.Animal && (!this.pawn.IsWildMan() || this.pawn.IsPrisoner))
			{
				return false;
			}
			if (!this.mentalStateHandler.InMentalState && this.pawn.Faction == null && Rand.Chance(this.pawn.RaceProps.manhunterOnTameFailChance))
			{
				this.StartManhunterBecauseOfPawnAction("AnimalManhunterFromTaming");
				return true;
			}
			return false;
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000EC91A File Offset: 0x000EAB1A
		internal void Notify_DangerousExploderAboutToExplode(Thing exploder)
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.Humanlike)
			{
				this.knownExploder = exploder;
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000EC948 File Offset: 0x000EAB48
		public void Notify_Explosion(Explosion explosion)
		{
			if (this.pawn.Faction != null)
			{
				return;
			}
			if (explosion.radius < 3.5f || !this.pawn.Position.InHorDistOf(explosion.Position, explosion.radius + 7f))
			{
				return;
			}
			if (Pawn_MindState.CanStartFleeingBecauseOfPawnAction(this.pawn))
			{
				this.StartFleeingBecauseOfPawnAction(explosion);
			}
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000EC9AC File Offset: 0x000EABAC
		public void Notify_TuckedIntoBed()
		{
			if (this.pawn.IsWildMan())
			{
				this.WildManEverReachedOutside = false;
			}
			this.ResetLastDisturbanceTick();
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000EC9C8 File Offset: 0x000EABC8
		public void Notify_SelfTended()
		{
			this.lastSelfTendTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000EC9DA File Offset: 0x000EABDA
		public void Notify_PredatorHuntingPlayerNotification()
		{
			this.lastPredatorHuntingPlayerNotificationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000EC9EC File Offset: 0x000EABEC
		private IEnumerable<Pawn> GetPackmates(Pawn pawn, float radius)
		{
			Room pawnRoom = pawn.GetRoom(RegionType.Set_Passable);
			List<Pawn> raceMates = pawn.Map.mapPawns.AllPawnsSpawned;
			int num;
			for (int i = 0; i < raceMates.Count; i = num + 1)
			{
				if (pawn != raceMates[i] && raceMates[i].def == pawn.def && raceMates[i].Faction == pawn.Faction && raceMates[i].Position.InHorDistOf(pawn.Position, radius) && raceMates[i].GetRoom(RegionType.Set_Passable) == pawnRoom)
				{
					yield return raceMates[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000ECA04 File Offset: 0x000EAC04
		private void StartManhunterBecauseOfPawnAction(string letterTextKey)
		{
			if (!this.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
			{
				return;
			}
			string text = letterTextKey.Translate(this.pawn.Label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			GlobalTargetInfo target = this.pawn;
			int num = 1;
			if (Find.Storyteller.difficulty.allowBigThreats && Rand.Value < 0.5f)
			{
				using (IEnumerator<Pawn> enumerator = this.GetPackmates(this.pawn, 24f).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
						{
							num++;
						}
					}
				}
				if (num > 1)
				{
					target = new TargetInfo(this.pawn.Position, this.pawn.Map, false);
					text += "\n\n";
					text += "AnimalManhunterOthers".Translate(this.pawn.kindDef.GetLabelPlural(-1), this.pawn);
				}
			}
			string value = this.pawn.RaceProps.Animal ? this.pawn.Label : this.pawn.def.label;
			string str = "LetterLabelAnimalManhunterRevenge".Translate(value).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(str, text, (num == 1) ? LetterDefOf.ThreatSmall : LetterDefOf.ThreatBig, target, null, null, null, null);
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000ECBEC File Offset: 0x000EADEC
		private static bool CanStartFleeingBecauseOfPawnAction(Pawn p)
		{
			return p.RaceProps.Animal && !p.InMentalState && !p.IsFighting() && !p.Downed && !p.Dead && !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p) && (p.jobs.curJob == null || p.jobs.curJob.def != JobDefOf.Flee || p.jobs.curJob.startTick != Find.TickManager.TicksGame);
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000ECC78 File Offset: 0x000EAE78
		public void StartFleeingBecauseOfPawnAction(Thing instigator)
		{
			List<Thing> threats = new List<Thing>
			{
				instigator
			};
			IntVec3 fleeDest = CellFinderLoose.GetFleeDest(this.pawn, threats, this.pawn.Position.DistanceTo(instigator.Position) + 14f);
			if (fleeDest != this.pawn.Position)
			{
				this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false);
			}
			if (this.pawn.RaceProps.herdAnimal && Rand.Chance(0.1f))
			{
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (Pawn_MindState.CanStartFleeingBecauseOfPawnAction(pawn))
					{
						IntVec3 fleeDest2 = CellFinderLoose.GetFleeDest(pawn, threats, pawn.Position.DistanceTo(instigator.Position) + 14f);
						if (fleeDest2 != pawn.Position)
						{
							pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest2, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false);
						}
					}
				}
			}
		}

		// Token: 0x040017F5 RID: 6133
		public Pawn pawn;

		// Token: 0x040017F6 RID: 6134
		public MentalStateHandler mentalStateHandler;

		// Token: 0x040017F7 RID: 6135
		public MentalBreaker mentalBreaker;

		// Token: 0x040017F8 RID: 6136
		public InspirationHandler inspirationHandler;

		// Token: 0x040017F9 RID: 6137
		public PriorityWork priorityWork;

		// Token: 0x040017FA RID: 6138
		private bool activeInt = true;

		// Token: 0x040017FB RID: 6139
		public JobTag lastJobTag;

		// Token: 0x040017FC RID: 6140
		public int lastIngestTick = -99999;

		// Token: 0x040017FD RID: 6141
		public int nextApparelOptimizeTick = -99999;

		// Token: 0x040017FE RID: 6142
		public bool canFleeIndividual = true;

		// Token: 0x040017FF RID: 6143
		public int exitMapAfterTick = -99999;

		// Token: 0x04001800 RID: 6144
		public int lastDisturbanceTick = -99999;

		// Token: 0x04001801 RID: 6145
		public IntVec3 forcedGotoPosition = IntVec3.Invalid;

		// Token: 0x04001802 RID: 6146
		public Thing knownExploder;

		// Token: 0x04001803 RID: 6147
		public bool wantsToTradeWithColony;

		// Token: 0x04001804 RID: 6148
		public Thing lastMannedThing;

		// Token: 0x04001805 RID: 6149
		public int canLovinTick = -99999;

		// Token: 0x04001806 RID: 6150
		public int canSleepTick = -99999;

		// Token: 0x04001807 RID: 6151
		public Pawn meleeThreat;

		// Token: 0x04001808 RID: 6152
		public int lastMeleeThreatHarmTick = -99999;

		// Token: 0x04001809 RID: 6153
		public int lastEngageTargetTick = -99999;

		// Token: 0x0400180A RID: 6154
		public int lastAttackTargetTick = -99999;

		// Token: 0x0400180B RID: 6155
		public LocalTargetInfo lastAttackedTarget;

		// Token: 0x0400180C RID: 6156
		public Thing enemyTarget;

		// Token: 0x0400180D RID: 6157
		public PawnDuty duty;

		// Token: 0x0400180E RID: 6158
		public Dictionary<int, int> thinkData = new Dictionary<int, int>();

		// Token: 0x0400180F RID: 6159
		public int lastAssignedInteractTime = -99999;

		// Token: 0x04001810 RID: 6160
		public int interactionsToday;

		// Token: 0x04001811 RID: 6161
		public int lastInventoryRawFoodUseTick;

		// Token: 0x04001812 RID: 6162
		public bool nextMoveOrderIsWait;

		// Token: 0x04001813 RID: 6163
		public int lastTakeCombatEnhancingDrugTick = -99999;

		// Token: 0x04001814 RID: 6164
		public int lastHarmTick = -99999;

		// Token: 0x04001815 RID: 6165
		public int noAidRelationsGainUntilTick = -99999;

		// Token: 0x04001816 RID: 6166
		public bool anyCloseHostilesRecently;

		// Token: 0x04001817 RID: 6167
		public int applyBedThoughtsTick;

		// Token: 0x04001818 RID: 6168
		public int applyThroneThoughtsTick;

		// Token: 0x04001819 RID: 6169
		public bool applyBedThoughtsOnLeave;

		// Token: 0x0400181A RID: 6170
		public bool willJoinColonyIfRescuedInt;

		// Token: 0x0400181B RID: 6171
		private bool wildManEverReachedOutsideInt;

		// Token: 0x0400181C RID: 6172
		public int timesGuestTendedToByPlayer;

		// Token: 0x0400181D RID: 6173
		public int lastSelfTendTick = -99999;

		// Token: 0x0400181E RID: 6174
		public bool spawnedByInfestationThingComp;

		// Token: 0x0400181F RID: 6175
		public int lastPredatorHuntingPlayerNotificationTick = -99999;

		// Token: 0x04001820 RID: 6176
		public float maxDistToSquadFlag = -1f;

		// Token: 0x04001821 RID: 6177
		private const int UpdateAnyCloseHostilesRecentlyEveryTicks = 100;

		// Token: 0x04001822 RID: 6178
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToActivate = 18;

		// Token: 0x04001823 RID: 6179
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToDeactivate = 24;

		// Token: 0x04001824 RID: 6180
		private const float HarmForgetDistance = 3f;

		// Token: 0x04001825 RID: 6181
		private const int MeleeHarmForgetDelay = 400;
	}
}
