using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class AbilityDef : Def
	{
		
		
		public float EntropyGain
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EntropyGain, 0f);
			}
		}

		
		
		public float PsyfocusCost
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_PsyfocusCost, 0f);
			}
		}

		
		
		public float EffectRadius
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EffectRadius, 0f);
			}
		}

		
		
		public float EffectDuration
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 0f);
			}
		}

		
		
		public bool HasAreaOfEffect
		{
			get
			{
				return this.EffectRadius > float.Epsilon;
			}
		}

		
		
		public float DetectionChance
		{
			get
			{
				if (this.detectionChanceOverride < 0f)
				{
					return this.GetStatValueAbstract(StatDefOf.Ability_DetectChancePerEntropy);
				}
				return this.detectionChanceOverride;
			}
		}

		
		
		public int RequiredPsyfocusBand
		{
			get
			{
				if (this.requiredPsyfocusBandCached == -1)
				{
					this.requiredPsyfocusBandCached = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count - 1;
					for (int i = 0; i < Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count; i++)
					{
						int num = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i];
						if (this.level <= num)
						{
							this.requiredPsyfocusBandCached = i;
							break;
						}
					}
				}
				return this.requiredPsyfocusBandCached;
			}
		}

		
		
		public IEnumerable<string> StatSummary
		{
			get
			{
				if (this.PsyfocusCost > 1.401298E-45f)
				{
					yield return "AbilityPsyfocusCost".Translate() + ": " + this.PsyfocusCost.ToStringPercent();
				}
				if (this.EntropyGain > 1.401298E-45f)
				{
					yield return "AbilityEntropyGain".Translate() + ": " + this.EntropyGain;
				}
				if (this.verbProperties.warmupTime > 1.401298E-45f)
				{
					yield return "AbilityCastingTime".Translate() + ": " + this.verbProperties.warmupTime + "LetterSecond".Translate();
				}
				if (this.EffectDuration > 1.401298E-45f)
				{
					yield return "AbilityDuration".Translate() + ": " + this.EffectDuration.SecondsToTicks().ToStringTicksToPeriod(true, false, true, true);
				}
				if (this.HasAreaOfEffect)
				{
					yield return "AbilityEffectRadius".Translate() + ": " + Mathf.Ceil(this.EffectRadius);
				}
				yield break;
			}
		}

		
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		
		public string GetTooltip(Pawn pawn = null)
		{
			if (this.cachedTooltip == null)
			{
				this.cachedTooltip = base.LabelCap + ((this.level > 0) ? ("\n" + "Level".Translate() + " " + this.level) : "") + "\n\n" + this.description;
				string text = this.StatSummary.ToLineList(null, false);
				if (!text.NullOrEmpty())
				{
					this.cachedTooltip = this.cachedTooltip + "\n\n" + text;
				}
			}
			if (pawn != null && ModsConfig.RoyaltyActive && this.abilityClass == typeof(Psycast) && this.level > 0)
			{
				Faction first = Faction.GetMinTitleForImplantAllFactions(HediffDefOf.PsychicAmplifier).First;
				if (first != null)
				{
					RoyalTitleDef minTitleForImplant = first.GetMinTitleForImplant(HediffDefOf.PsychicAmplifier, this.level);
					RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(first);
					if (minTitleForImplant != null && (currentTitle == null || currentTitle.seniority < minTitleForImplant.seniority) && this.DetectionChance > 0f)
					{
						return cachedTooltip + "\n\n" + ColoredText.Colorize("PsycastIsIllegal".Translate(pawn.Named("PAWN"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE")), ColoredText.WarningColor);
					}
				}
			}
			return this.cachedTooltip;
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.cachedTargets == null)
			{
				this.cachedTargets = new List<string>();
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetSelf)
				{
					this.cachedTargets.Add("TargetSelf".Translate());
				}
				if (this.verbProperties.targetParams.canTargetLocations)
				{
					this.cachedTargets.Add("TargetGround".Translate());
				}
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetHumans)
				{
					this.cachedTargets.Add("TargetHuman".Translate());
				}
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetAnimals)
				{
					this.cachedTargets.Add("TargetAnimal".Translate());
				}
			}
			int num = this.comps.OfType<CompProperties_AbilityEffect>().Sum((CompProperties_AbilityEffect e) => e.goodwillImpact);
			if (num != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_GoodwillImpact, (float)num, req, ToStringNumberSense.Undefined, null, false);
			}
			if (this.level != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_RequiredPsylink, (float)this.level, req, ToStringNumberSense.Undefined, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_CastingTime, this.verbProperties.warmupTime, req, ToStringNumberSense.Undefined, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_Range, this.verbProperties.range, req, ToStringNumberSense.Undefined, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "Target".Translate(), this.cachedTargets.ToCommaList(false).CapitalizeFirst(), "AbilityTargetDesc".Translate(), 1001, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "AbilityRequiresLOS".Translate(), this.verbProperties.requireLineOfSight ? "Yes".Translate() : "No".Translate(), "", 1000, null, null, false);
			yield break;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			IEnumerator<string> enumerator = null;
			if (this.abilityClass == null)
			{
				yield return "abilityClass is null";
			}
			if (this.verbProperties == null)
			{
				yield return "verbProperties are null";
			}
			if (this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.statBases != null)
			{
				List<StatModifier>.Enumerator enumerator2 = this.statBases.GetEnumerator();
				{
					while (enumerator2.MoveNext())
					{
						StatModifier statBase = enumerator2.Current;
						if (this.statBases.Count((StatModifier st) => st.stat == statBase.stat) > 1)
						{
							yield return "defines the stat base " + statBase.stat + " more than once.";
						}
					}
				}
			}
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		
		public Type abilityClass = typeof(Ability);

		
		public Type gizmoClass = typeof(Command_Ability);

		
		public List<AbilityCompProperties> comps = new List<AbilityCompProperties>();

		
		public List<StatModifier> statBases;

		
		public VerbProperties verbProperties;

		
		public KeyBindingDef hotKey;

		
		public JobDef jobDef;

		
		public ThingDef warmupMote;

		
		public Vector3 moteDrawOffset;

		
		public bool canUseAoeToGetTargets = true;

		
		public bool targetRequired = true;

		
		public int level;

		
		public IntRange cooldownTicksRange;

		
		public bool sendLetterOnCooldownComplete;

		
		public bool displayGizmoWhileUndrafted;

		
		public bool disableGizmoWhileUndrafted = true;

		
		public float detectionChanceOverride = -1f;

		
		public string iconPath;

		
		public Texture2D uiIcon = BaseContent.BadTex;

		
		private string cachedTooltip;

		
		private List<string> cachedTargets;

		
		private int requiredPsyfocusBandCached = -1;
	}
}
