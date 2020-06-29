using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class Pawn_PsychicEntropyTracker : IExposable
	{
		
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x0017B6DC File Offset: 0x001798DC
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x0017B6E4 File Offset: 0x001798E4
		public float MaxEntropy
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true);
			}
		}

		
		// (get) Token: 0x06004654 RID: 18004 RVA: 0x0017B6F7 File Offset: 0x001798F7
		public float MaxPotentialEntropy
		{
			get
			{
				return Mathf.Max(this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true), this.MaxEntropy);
			}
		}

		
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x0017B715 File Offset: 0x00179915
		public float PainMultiplier
		{
			get
			{
				return 1f + this.pawn.health.hediffSet.PainTotal * 3f;
			}
		}

		
		// (get) Token: 0x06004656 RID: 18006 RVA: 0x0017B738 File Offset: 0x00179938
		public float RecoveryRate
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyRecoveryRate, true) * this.PainMultiplier;
			}
		}

		
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x0017B752 File Offset: 0x00179952
		public float EntropyValue
		{
			get
			{
				return this.currentEntropy;
			}
		}

		
		// (get) Token: 0x06004658 RID: 18008 RVA: 0x0017B75A File Offset: 0x0017995A
		public float CurrentPsyfocus
		{
			get
			{
				return this.currentPsyfocus;
			}
		}

		
		// (get) Token: 0x06004659 RID: 18009 RVA: 0x0017B762 File Offset: 0x00179962
		public float TargetPsyfocus
		{
			get
			{
				return this.targetPsyfocus;
			}
		}

		
		// (get) Token: 0x0600465A RID: 18010 RVA: 0x0017B76A File Offset: 0x0017996A
		public int MaxAbilityLevel
		{
			get
			{
				return Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[this.PsyfocusBand];
			}
		}

		
		// (get) Token: 0x0600465B RID: 18011 RVA: 0x0017B77C File Offset: 0x0017997C
		public bool IsCurrentlyMeditating
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastMeditationTick + 10;
			}
		}

		
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

		
		// (get) Token: 0x06004660 RID: 18016 RVA: 0x0017B8E0 File Offset: 0x00179AE0
		public bool NeedsPsyfocus
		{
			get
			{
				return this.Psylink != null && !this.pawn.Suspended && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		
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

		
		public Pawn_PsychicEntropyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
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

		
		public bool WouldOverflowEntropy(float value)
		{
			return this.limitEntropyAmount && this.currentEntropy + value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true) > this.MaxEntropy;
		}

		
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

		
		public void RemoveAllEntropy()
		{
			this.currentEntropy = 0f;
		}

		
		[Obsolete("Only used for mod compatibility")]
		private void GiveHangoverIfNeeded()
		{
		}

		
		[Obsolete("Only used for mod compatibility")]
		private void GiveHangoverIfNeeded_NewTemp(float entropyChange)
		{
		}

		
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

		
		public void Notify_Meditated()
		{
			this.lastMeditationTick = Find.TickManager.TicksGame;
		}

		
		public void OffsetPsyfocusDirectly(float offset)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + offset, 0f, 1f);
		}

		
		public void SetInitialPsyfocusLevel()
		{
			if (this.pawn.IsColonist && !this.pawn.IsQuestLodger())
			{
				this.currentPsyfocus = 0.75f;
				return;
			}
			this.currentPsyfocus = Rand.Range(0.5f, 0.7f);
		}

		
		public void SetPsyfocusTarget(float val)
		{
			this.targetPsyfocus = Mathf.Clamp(val, 0f, 1f);
		}

		
		public void Notify_GainedPsylink()
		{
			this.currentPsyfocus = Mathf.Max(this.currentPsyfocus, 0.75f);
		}

		
		public void Notify_PawnDied()
		{
			this.currentEntropy = 0f;
			this.currentPsyfocus = 0f;
		}

		
		public bool NeedToShowGizmo()
		{
			return ModsConfig.RoyaltyActive && this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) > float.Epsilon && (this.EntropyValue > float.Epsilon || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false));
		}

		
		public Gizmo GetGizmo()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new PsychicEntropyGizmo(this);
			}
			return this.gizmo;
		}

		
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

		
		private Pawn pawn;

		
		private float currentEntropy;

		
		private int ticksSinceLastMote;

		
		public bool limitEntropyAmount = true;

		
		private float currentPsyfocus = -1f;

		
		private float targetPsyfocus = 0.5f;

		
		private int lastMeditationTick = -1;

		
		private Gizmo gizmo;

		
		private Hediff_Psylink psylinkCached;

		
		private int psylinkCachedForTick = -1;

		
		private static readonly int[] TicksBetweenMotes = new int[]
		{
			300,
			200,
			100,
			75,
			50
		};

		
		public const float PercentageAfterGainingPsylink = 0.75f;

		
		public const int PsyfocusUpdateInterval = 150;

		
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

		
		public static readonly List<float> PsyfocusBandPercentages = new List<float>
		{
			0f,
			0.25f,
			0.5f,
			1f
		};

		
		public static readonly List<float> FallRatePerPsyfocusBand = new List<float>
		{
			0.035f,
			0.055f,
			0.075f
		};

		
		public static readonly List<int> MaxAbilityLevelPerPsyfocusBand = new List<int>
		{
			2,
			4,
			6
		};

		
		public static Dictionary<PsychicEntropySeverity, SoundDef> EntropyThresholdSounds;

		
		public static string psyfocusLevelInfoCached = null;
	}
}
