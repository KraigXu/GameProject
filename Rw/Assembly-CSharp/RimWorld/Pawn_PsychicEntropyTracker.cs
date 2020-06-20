using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BAF RID: 2991
	public class Pawn_PsychicEntropyTracker : IExposable
	{
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x0017B6DC File Offset: 0x001798DC
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x0017B6E4 File Offset: 0x001798E4
		public float MaxEntropy
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true);
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004654 RID: 18004 RVA: 0x0017B6F7 File Offset: 0x001798F7
		public float MaxPotentialEntropy
		{
			get
			{
				return Mathf.Max(this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true), this.MaxEntropy);
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x0017B715 File Offset: 0x00179915
		public float PainMultiplier
		{
			get
			{
				return 1f + this.pawn.health.hediffSet.PainTotal * 3f;
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06004656 RID: 18006 RVA: 0x0017B738 File Offset: 0x00179938
		public float RecoveryRate
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyRecoveryRate, true) * this.PainMultiplier;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x0017B752 File Offset: 0x00179952
		public float EntropyValue
		{
			get
			{
				return this.currentEntropy;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06004658 RID: 18008 RVA: 0x0017B75A File Offset: 0x0017995A
		public float CurrentPsyfocus
		{
			get
			{
				return this.currentPsyfocus;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004659 RID: 18009 RVA: 0x0017B762 File Offset: 0x00179962
		public float TargetPsyfocus
		{
			get
			{
				return this.targetPsyfocus;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x0600465A RID: 18010 RVA: 0x0017B76A File Offset: 0x0017996A
		public int MaxAbilityLevel
		{
			get
			{
				return Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x0600465B RID: 18011 RVA: 0x0017B77C File Offset: 0x0017997C
		public bool IsCurrentlyMeditating
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastMeditationTick + 10;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x0600465C RID: 18012 RVA: 0x0017B794 File Offset: 0x00179994
		public float EntropyRelativeValue
		{
			get
			{
				if (this.currentEntropy < 1.401298E-45f)
				{
					return 0f;
				}
				if (this.currentEntropy < this.MaxEntropy)
				{
					if (this.MaxEntropy <= 1.401298E-45f)
					{
						return 0f;
					}
					return this.currentEntropy / this.MaxEntropy;
				}
				else
				{
					if (this.MaxPotentialEntropy <= 1.401298E-45f)
					{
						return 0f;
					}
					return 1f + (this.currentEntropy - this.MaxEntropy) / this.MaxPotentialEntropy;
				}
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x0600465D RID: 18013 RVA: 0x0017B810 File Offset: 0x00179A10
		public PsychicEntropySeverity Severity
		{
			get
			{
				PsychicEntropySeverity result = PsychicEntropySeverity.Safe;
				foreach (PsychicEntropySeverity psychicEntropySeverity in Pawn_PsychicEntropyTracker.EntropyThresholds.Keys)
				{
					if (Pawn_PsychicEntropyTracker.EntropyThresholds[psychicEntropySeverity] >= this.EntropyRelativeValue)
					{
						break;
					}
					result = psychicEntropySeverity;
				}
				return result;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x0600465E RID: 18014 RVA: 0x0017B878 File Offset: 0x00179A78
		public int PsyfocusBand
		{
			get
			{
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[1])
				{
					return 0;
				}
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[2])
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x0600465F RID: 18015 RVA: 0x0017B8A5 File Offset: 0x00179AA5
		public Hediff_Psylink Psylink
		{
			get
			{
				if (this.psylinkCachedForTick != Find.TickManager.TicksGame)
				{
					this.psylinkCached = this.pawn.GetMainPsylinkSource();
					this.psylinkCachedForTick = Find.TickManager.TicksGame;
				}
				return this.psylinkCached;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004660 RID: 18016 RVA: 0x0017B8E0 File Offset: 0x00179AE0
		public bool NeedsPsyfocus
		{
			get
			{
				return this.Psylink != null && !this.pawn.Suspended && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06004661 RID: 18017 RVA: 0x0017B918 File Offset: 0x00179B18
		private float PsyfocusFallPerDay
		{
			get
			{
				if (this.pawn.GetPsylinkLevel() == 0)
				{
					return 0f;
				}
				return Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x0017B93D File Offset: 0x00179B3D
		public Pawn_PsychicEntropyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x0017B977 File Offset: 0x00179B77
		public static void ResetStaticData()
		{
			Pawn_PsychicEntropyTracker.EntropyThresholdSounds = new Dictionary<PsychicEntropySeverity, SoundDef>
			{
				{
					PsychicEntropySeverity.Overloaded,
					SoundDefOf.PsychicEntropyOverloaded
				},
				{
					PsychicEntropySeverity.Hyperloaded,
					SoundDefOf.PsychicEntropyHyperloaded
				},
				{
					PsychicEntropySeverity.BrainCharring,
					SoundDefOf.PsychicEntropyBrainCharring
				},
				{
					PsychicEntropySeverity.BrainRoasting,
					SoundDefOf.PsychicEntropyBrainRoasting
				}
			};
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x0017B9B4 File Offset: 0x00179BB4
		public void PsychicEntropyTrackerTick()
		{
			if (this.currentEntropy > 1.401298E-45f)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy - 1.TicksToSeconds() * this.RecoveryRate, 0f);
			}
			if (this.currentEntropy > 1.401298E-45f && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicEntropy, false))
			{
				this.pawn.health.AddHediff(HediffDefOf.PsychicEntropy, null, null, null);
			}
			if (this.currentEntropy > 1.401298E-45f)
			{
				if (this.ticksSinceLastMote >= Pawn_PsychicEntropyTracker.TicksBetweenMotes[(int)this.Severity])
				{
					if (this.pawn.Spawned)
					{
						MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_EntropyPulse, Vector3.zero, 1f, -1f);
					}
					this.ticksSinceLastMote = 0;
				}
				else
				{
					this.ticksSinceLastMote++;
				}
			}
			else
			{
				this.ticksSinceLastMote = 0;
			}
			if (this.NeedsPsyfocus && this.pawn.IsHashIntervalTick(150))
			{
				float num = 400f;
				if (!this.IsCurrentlyMeditating)
				{
					this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus - this.PsyfocusFallPerDay / num, 0f, 1f);
				}
				MeditationUtility.CheckMeditationScheduleTeachOpportunity(this.pawn);
			}
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x0017BB02 File Offset: 0x00179D02
		public bool WouldOverflowEntropy(float value)
		{
			return this.limitEntropyAmount && this.currentEntropy + value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true) > this.MaxEntropy;
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x0017BB30 File Offset: 0x00179D30
		public bool TryAddEntropy(float value, Thing source = null, bool scale = true, bool overLimit = false)
		{
			PsychicEntropySeverity severity = this.Severity;
			float num = scale ? (value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true)) : value;
			if (!this.WouldOverflowEntropy(num) || overLimit)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy + num, 0f);
				foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
				{
					hediff.Notify_EntropyGained(value, num, source);
				}
				if (severity != this.Severity && num > 0f && this.Severity != PsychicEntropySeverity.Safe)
				{
					Pawn_PsychicEntropyTracker.EntropyThresholdSounds[this.Severity].PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
					if (severity < PsychicEntropySeverity.Overloaded && this.Severity >= PsychicEntropySeverity.Overloaded)
					{
						Messages.Message("MessageWentOverPsychicEntropyLimit".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x0017BC78 File Offset: 0x00179E78
		public void RemoveAllEntropy()
		{
			this.currentEntropy = 0f;
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x00002681 File Offset: 0x00000881
		[Obsolete("Only used for mod compatibility")]
		private void GiveHangoverIfNeeded()
		{
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x00002681 File Offset: 0x00000881
		[Obsolete("Only used for mod compatibility")]
		private void GiveHangoverIfNeeded_NewTemp(float entropyChange)
		{
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x0017BC88 File Offset: 0x00179E88
		public void GainPsyfocus(Thing focus = null)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + MeditationUtility.PsyfocusGainPerTick(this.pawn, focus), 0f, 1f);
			if (focus != null && !focus.Destroyed)
			{
				CompMeditationFocus compMeditationFocus = focus.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null)
				{
					compMeditationFocus.Used(this.pawn);
				}
			}
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x0017BCDE File Offset: 0x00179EDE
		public void Notify_Meditated()
		{
			this.lastMeditationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x0017BCF0 File Offset: 0x00179EF0
		public void OffsetPsyfocusDirectly(float offset)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + offset, 0f, 1f);
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x0017BD0F File Offset: 0x00179F0F
		public void SetInitialPsyfocusLevel()
		{
			if (this.pawn.IsColonist && !this.pawn.IsQuestLodger())
			{
				this.currentPsyfocus = 0.75f;
				return;
			}
			this.currentPsyfocus = Rand.Range(0.5f, 0.7f);
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x0017BD4C File Offset: 0x00179F4C
		public void SetPsyfocusTarget(float val)
		{
			this.targetPsyfocus = Mathf.Clamp(val, 0f, 1f);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x0017BD64 File Offset: 0x00179F64
		public void Notify_GainedPsylink()
		{
			this.currentPsyfocus = Mathf.Max(this.currentPsyfocus, 0.75f);
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x0017BD7C File Offset: 0x00179F7C
		public void Notify_PawnDied()
		{
			this.currentEntropy = 0f;
			this.currentPsyfocus = 0f;
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x0017BD94 File Offset: 0x00179F94
		public bool NeedToShowGizmo()
		{
			return ModsConfig.RoyaltyActive && this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) > float.Epsilon && (this.EntropyValue > float.Epsilon || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false));
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x0017BDEE File Offset: 0x00179FEE
		public Gizmo GetGizmo()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new PsychicEntropyGizmo(this);
			}
			return this.gizmo;
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x0017BE0C File Offset: 0x0017A00C
		public string PsyfocusTipString()
		{
			if (Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached == null)
			{
				for (int i = 0; i < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; i++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoPsycasts".Translate(Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i]) + "\n";
				}
				Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "\n";
				for (int j = 0; j < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; j++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoFallRate".Translate(Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[j].ToStringPercent()) + "\n";
				}
			}
			return "Psyfocus".Translate() + ": " + this.currentPsyfocus.ToStringPercent("0.#") + "\n" + "DesiredPsyfocus".Translate() + ": " + this.targetPsyfocus.ToStringPercent("0.#") + "\n\n" + "DesiredPsyfocusDesc".Translate(this.pawn.Named("PAWN")) + "\n\n" + "PsyfocusDesc".Translate() + ":\n\n" + Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached;
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x0017C044 File Offset: 0x0017A244
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.currentEntropy, "currentEntropy", 0f, false);
			Scribe_Values.Look<float>(ref this.currentPsyfocus, "currentPsyfocus", -1f, false);
			Scribe_Values.Look<float>(ref this.targetPsyfocus, "targetPsyfocus", 0.5f, false);
			Scribe_Values.Look<int>(ref this.lastMeditationTick, "lastMeditationTick", -1, false);
			Scribe_Values.Look<bool>(ref this.limitEntropyAmount, "limitEntropyAmount", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.currentPsyfocus < 0f)
			{
				this.SetInitialPsyfocusLevel();
			}
		}

		// Token: 0x0400285C RID: 10332
		private Pawn pawn;

		// Token: 0x0400285D RID: 10333
		private float currentEntropy;

		// Token: 0x0400285E RID: 10334
		private int ticksSinceLastMote;

		// Token: 0x0400285F RID: 10335
		public bool limitEntropyAmount = true;

		// Token: 0x04002860 RID: 10336
		private float currentPsyfocus = -1f;

		// Token: 0x04002861 RID: 10337
		private float targetPsyfocus = 0.5f;

		// Token: 0x04002862 RID: 10338
		private int lastMeditationTick = -1;

		// Token: 0x04002863 RID: 10339
		private Gizmo gizmo;

		// Token: 0x04002864 RID: 10340
		private Hediff_Psylink psylinkCached;

		// Token: 0x04002865 RID: 10341
		private int psylinkCachedForTick = -1;

		// Token: 0x04002866 RID: 10342
		private static readonly int[] TicksBetweenMotes = new int[]
		{
			300,
			200,
			100,
			75,
			50
		};

		// Token: 0x04002867 RID: 10343
		public const float PercentageAfterGainingPsylink = 0.75f;

		// Token: 0x04002868 RID: 10344
		public const int PsyfocusUpdateInterval = 150;

		// Token: 0x04002869 RID: 10345
		public static readonly Dictionary<PsychicEntropySeverity, float> EntropyThresholds = new Dictionary<PsychicEntropySeverity, float>
		{
			{
				PsychicEntropySeverity.Safe,
				0f
			},
			{
				PsychicEntropySeverity.Overloaded,
				1f
			},
			{
				PsychicEntropySeverity.Hyperloaded,
				1.33f
			},
			{
				PsychicEntropySeverity.BrainCharring,
				1.66f
			},
			{
				PsychicEntropySeverity.BrainRoasting,
				2f
			}
		};

		// Token: 0x0400286A RID: 10346
		public static readonly List<float> PsyfocusBandPercentages = new List<float>
		{
			0f,
			0.25f,
			0.5f,
			1f
		};

		// Token: 0x0400286B RID: 10347
		public static readonly List<float> FallRatePerPsyfocusBand = new List<float>
		{
			0.035f,
			0.055f,
			0.075f
		};

		// Token: 0x0400286C RID: 10348
		public static readonly List<int> MaxAbilityLevelPerPsyfocusBand = new List<int>
		{
			2,
			4,
			6
		};

		// Token: 0x0400286D RID: 10349
		public static Dictionary<PsychicEntropySeverity, SoundDef> EntropyThresholdSounds;

		// Token: 0x0400286E RID: 10350
		public static string psyfocusLevelInfoCached = null;
	}
}
