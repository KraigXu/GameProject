using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B7F RID: 2943
	public class Pawn_GuestTracker : IExposable
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060044EE RID: 17646 RVA: 0x001743D0 File Offset: 0x001725D0
		public Faction HostFaction
		{
			get
			{
				return this.hostFactionInt;
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060044EF RID: 17647 RVA: 0x001743D8 File Offset: 0x001725D8
		public bool CanBeBroughtFood
		{
			get
			{
				return this.interactionMode != PrisonerInteractionModeDefOf.Execution && (this.interactionMode != PrisonerInteractionModeDefOf.Release || this.pawn.Downed);
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x060044F0 RID: 17648 RVA: 0x00174403 File Offset: 0x00172603
		public bool IsPrisoner
		{
			get
			{
				return this.isPrisonerInt;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x060044F1 RID: 17649 RVA: 0x0017440B File Offset: 0x0017260B
		public bool ScheduledForInteraction
		{
			get
			{
				return this.pawn.mindState.lastAssignedInteractTime < Find.TickManager.TicksGame - 10000 && this.pawn.mindState.interactionsToday < 2;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x060044F2 RID: 17650 RVA: 0x00174444 File Offset: 0x00172644
		// (set) Token: 0x060044F3 RID: 17651 RVA: 0x0017444C File Offset: 0x0017264C
		public bool Released
		{
			get
			{
				return this.releasedInt;
			}
			set
			{
				if (value == this.releasedInt)
				{
					return;
				}
				this.releasedInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x060044F4 RID: 17652 RVA: 0x0017446C File Offset: 0x0017266C
		public bool PrisonerIsSecure
		{
			get
			{
				if (this.Released)
				{
					return false;
				}
				if (this.pawn.HostFaction == null)
				{
					return false;
				}
				if (this.pawn.InMentalState)
				{
					return false;
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
					{
						return false;
					}
					if (PrisonBreakUtility.IsPrisonBreaking(this.pawn))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060044F5 RID: 17653 RVA: 0x001744EC File Offset: 0x001726EC
		public bool ShouldWaitInsteadOfEscaping
		{
			get
			{
				if (!this.IsPrisoner)
				{
					return false;
				}
				Map mapHeld = this.pawn.MapHeld;
				return mapHeld != null && mapHeld.mapPawns.FreeColonistsSpawnedCount != 0 && Find.TickManager.TicksGame < this.ticksWhenAllowedToEscapeAgain;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060044F6 RID: 17654 RVA: 0x00174535 File Offset: 0x00172735
		public float Resistance
		{
			get
			{
				return this.resistance;
			}
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x0017453D File Offset: 0x0017273D
		public Pawn_GuestTracker()
		{
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x0017456D File Offset: 0x0017276D
		public Pawn_GuestTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x001745A4 File Offset: 0x001727A4
		public void GuestTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(2500))
			{
				float num = PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.pawn);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					PrisonBreakUtility.StartPrisonBreak(this.pawn);
				}
			}
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x001745F4 File Offset: 0x001727F4
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.hostFactionInt, "hostFaction", false);
			Scribe_Values.Look<bool>(ref this.isPrisonerInt, "prisoner", false, false);
			Scribe_Defs.Look<PrisonerInteractionModeDef>(ref this.interactionMode, "interactionMode");
			Scribe_Values.Look<bool>(ref this.releasedInt, "released", false, false);
			Scribe_Values.Look<int>(ref this.ticksWhenAllowedToEscapeAgain, "ticksWhenAllowedToEscapeAgain", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spotToWaitInsteadOfEscaping, "spotToWaitInsteadOfEscaping", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastPrisonBreakTicks, "lastPrisonBreakTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everParticipatedInPrisonBreak, "everParticipatedInPrisonBreak", false, false);
			Scribe_Values.Look<bool>(ref this.getRescuedThoughtOnUndownedBecauseOfPlayer, "getRescuedThoughtOnUndownedBecauseOfPlayer", false, false);
			Scribe_Values.Look<float>(ref this.resistance, "resistance", -1f, false);
			Scribe_Values.Look<string>(ref this.lastRecruiterName, "lastRecruiterName", null, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterNegotiationAbilityFactor, "lastRecruiterNegotiationAbilityFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasOpinionOfLastRecruiter, "hasOpinionOfLastRecruiter", false, false);
			Scribe_Values.Look<int>(ref this.lastRecruiterOpinion, "lastRecruiterOpinion", 0, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterFinalChance, "lastRecruiterFinalChance", 0f, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterOpinionChanceFactor, "lastRecruiterOpinionChanceFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.lastRecruiterResistanceReduce, "lastRecruiterResistanceReduce", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.interactionMode.allowOnWildMan && this.pawn.IsWildMan())
			{
				this.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x0017477C File Offset: 0x0017297C
		public void ClearLastRecruiterData()
		{
			this.lastRecruiterName = null;
			this.lastRecruiterNegotiationAbilityFactor = 0f;
			this.lastRecruiterOpinion = 0;
			this.lastRecruiterOpinionChanceFactor = 0f;
			this.lastRecruiterResistanceReduce = 0f;
			this.hasOpinionOfLastRecruiter = false;
			this.lastRecruiterFinalChance = 0f;
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x001747CC File Offset: 0x001729CC
		public void SetLastRecruiterData(Pawn recruiter, float resistanceReduce)
		{
			this.lastRecruiterName = recruiter.LabelShort;
			this.lastRecruiterNegotiationAbilityFactor = RecruitUtility.RecruitChanceFactorForRecruiterNegotiationAbility(recruiter);
			this.lastRecruiterOpinionChanceFactor = RecruitUtility.RecruitChanceFactorForOpinion(recruiter, this.pawn);
			this.lastRecruiterResistanceReduce = resistanceReduce;
			this.hasOpinionOfLastRecruiter = (this.pawn.relations != null);
			this.lastRecruiterOpinion = (this.hasOpinionOfLastRecruiter ? this.pawn.relations.OpinionOf(recruiter) : 0);
			this.lastRecruiterFinalChance = this.pawn.RecruitChanceFinalByPawn(recruiter);
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x00174854 File Offset: 0x00172A54
		public void SetGuestStatus(Faction newHost, bool prisoner = false)
		{
			if (newHost != null)
			{
				this.Released = false;
			}
			if (newHost == this.HostFaction && prisoner == this.IsPrisoner)
			{
				return;
			}
			if (!prisoner && this.pawn.Faction.HostileTo(newHost))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to make ",
					this.pawn,
					" a guest of ",
					newHost,
					" but their faction ",
					this.pawn.Faction,
					" is hostile to ",
					newHost
				}), false);
				return;
			}
			if (newHost != null && newHost == this.pawn.Faction && !prisoner)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to make ",
					this.pawn,
					" a guest of their own faction ",
					this.pawn.Faction
				}), false);
				return;
			}
			bool flag = prisoner && (!this.IsPrisoner || this.HostFaction != newHost);
			this.isPrisonerInt = prisoner;
			Faction faction = this.hostFactionInt;
			this.hostFactionInt = newHost;
			this.pawn.ClearMind(newHost != null, false, prisoner);
			if (flag)
			{
				this.pawn.DropAndForbidEverything(false);
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.MadePrisoner, null);
				}
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				float x = this.pawn.RecruitDifficulty(Faction.OfPlayer);
				this.resistance = Pawn_GuestTracker.StartingResistancePerRecruitDifficultyCurve.Evaluate(x);
				this.resistance *= Pawn_GuestTracker.StartingResistanceFactorFromPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent);
				this.resistance *= Pawn_GuestTracker.StartingResistanceRandomFactorRange.RandomInRange;
				if (this.pawn.royalty != null)
				{
					RoyalTitle mostSeniorTitle = this.pawn.royalty.MostSeniorTitle;
					if (mostSeniorTitle != null)
					{
						this.resistance *= mostSeniorTitle.def.recruitmentResistanceFactor;
						this.resistance += mostSeniorTitle.def.recruitmentResistanceOffset;
					}
				}
				this.resistance = (float)GenMath.RoundRandom(this.resistance);
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
			this.pawn.health.surgeryBills.Clear();
			if (this.pawn.ownership != null)
			{
				this.pawn.ownership.Notify_ChangedGuestStatus();
			}
			ReachabilityUtility.ClearCacheFor(this.pawn);
			if (this.pawn.Spawned)
			{
				this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this.pawn);
			if (prisoner && this.pawn.playerSettings != null)
			{
				this.pawn.playerSettings.Notify_MadePrisoner();
			}
			if (faction != this.hostFactionInt)
			{
				QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ChangedHostFaction", this.pawn.Named("SUBJECT"), this.hostFactionInt.Named("FACTION"));
			}
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x00174B84 File Offset: 0x00172D84
		public void CapturedBy(Faction by, Pawn byPawn = null)
		{
			Faction factionOrExtraHomeFaction = this.pawn.FactionOrExtraHomeFaction;
			if (factionOrExtraHomeFaction != null)
			{
				factionOrExtraHomeFaction.Notify_MemberCaptured(this.pawn, by);
			}
			this.SetGuestStatus(by, true);
			if (this.IsPrisoner && byPawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
				{
					byPawn,
					this.pawn
				});
				byPawn.records.Increment(RecordDefOf.PeopleCaptured);
			}
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x00174BEE File Offset: 0x00172DEE
		public void WaitInsteadOfEscapingForDefaultTicks()
		{
			this.WaitInsteadOfEscapingFor(25000);
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00174BFB File Offset: 0x00172DFB
		public void WaitInsteadOfEscapingFor(int ticks)
		{
			if (!this.IsPrisoner)
			{
				return;
			}
			this.ticksWhenAllowedToEscapeAgain = Find.TickManager.TicksGame + ticks;
			this.spotToWaitInsteadOfEscaping = IntVec3.Invalid;
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00174C24 File Offset: 0x00172E24
		internal void Notify_PawnUndowned()
		{
			if (this.pawn.RaceProps.Humanlike && (this.HostFaction == Faction.OfPlayer || (this.pawn.IsWildMan() && this.pawn.InBed() && this.pawn.CurrentBed().Faction == Faction.OfPlayer)) && !this.IsPrisoner && this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.getRescuedThoughtOnUndownedBecauseOfPlayer && this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null);
				}
				if (this.pawn.Faction == null || this.pawn.Faction.def.rescueesCanJoin)
				{
					Map mapHeld = this.pawn.MapHeld;
					float num;
					if (!this.pawn.SafeTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp) || mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
					{
						num = 1f;
					}
					else
					{
						num = 0.5f;
					}
					if (Rand.ValueSeeded(this.pawn.thingIDNumber ^ 8976612) < num)
					{
						this.pawn.SetFaction(Faction.OfPlayer, null);
						Find.LetterStack.ReceiveLetter("LetterLabelRescueeJoins".Translate(this.pawn.Named("PAWN")), "LetterRescueeJoins".Translate(this.pawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
					}
					else
					{
						Messages.Message("MessageRescueeDidntJoin".Translate().AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
					}
				}
			}
			this.getRescuedThoughtOnUndownedBecauseOfPlayer = false;
		}

		// Token: 0x04002759 RID: 10073
		private Pawn pawn;

		// Token: 0x0400275A RID: 10074
		public PrisonerInteractionModeDef interactionMode = PrisonerInteractionModeDefOf.NoInteraction;

		// Token: 0x0400275B RID: 10075
		private Faction hostFactionInt;

		// Token: 0x0400275C RID: 10076
		public bool isPrisonerInt;

		// Token: 0x0400275D RID: 10077
		public string lastRecruiterName;

		// Token: 0x0400275E RID: 10078
		public int lastRecruiterOpinion;

		// Token: 0x0400275F RID: 10079
		public float lastRecruiterOpinionChanceFactor;

		// Token: 0x04002760 RID: 10080
		public float lastRecruiterNegotiationAbilityFactor;

		// Token: 0x04002761 RID: 10081
		public bool hasOpinionOfLastRecruiter;

		// Token: 0x04002762 RID: 10082
		public float lastRecruiterResistanceReduce;

		// Token: 0x04002763 RID: 10083
		public float lastRecruiterFinalChance;

		// Token: 0x04002764 RID: 10084
		private bool releasedInt;

		// Token: 0x04002765 RID: 10085
		private int ticksWhenAllowedToEscapeAgain;

		// Token: 0x04002766 RID: 10086
		public IntVec3 spotToWaitInsteadOfEscaping = IntVec3.Invalid;

		// Token: 0x04002767 RID: 10087
		public int lastPrisonBreakTicks = -1;

		// Token: 0x04002768 RID: 10088
		public bool everParticipatedInPrisonBreak;

		// Token: 0x04002769 RID: 10089
		public float resistance = -1f;

		// Token: 0x0400276A RID: 10090
		public bool getRescuedThoughtOnUndownedBecauseOfPlayer;

		// Token: 0x0400276B RID: 10091
		private const int DefaultWaitInsteadOfEscapingTicks = 25000;

		// Token: 0x0400276C RID: 10092
		public const int MinInteractionInterval = 10000;

		// Token: 0x0400276D RID: 10093
		public const int MaxInteractionsPerDay = 2;

		// Token: 0x0400276E RID: 10094
		private const int CheckInitiatePrisonBreakIntervalTicks = 2500;

		// Token: 0x0400276F RID: 10095
		private static readonly SimpleCurve StartingResistancePerRecruitDifficultyCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 15f),
				true
			},
			{
				new CurvePoint(0.9f, 25f),
				true
			},
			{
				new CurvePoint(1f, 50f),
				true
			}
		};

		// Token: 0x04002770 RID: 10096
		private static readonly SimpleCurve StartingResistanceFactorFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 2f),
				true
			},
			{
				new CurvePoint(0f, 1.5f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 0.8f),
				true
			}
		};

		// Token: 0x04002771 RID: 10097
		private static readonly FloatRange StartingResistanceRandomFactorRange = new FloatRange(0.8f, 1.2f);
	}
}
