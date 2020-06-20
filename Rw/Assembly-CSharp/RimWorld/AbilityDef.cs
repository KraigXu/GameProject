using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008AC RID: 2220
	public class AbilityDef : Def
	{
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060035A4 RID: 13732 RVA: 0x00123E3E File Offset: 0x0012203E
		public float EntropyGain
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EntropyGain, 0f);
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060035A5 RID: 13733 RVA: 0x00123E55 File Offset: 0x00122055
		public float PsyfocusCost
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_PsyfocusCost, 0f);
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060035A6 RID: 13734 RVA: 0x00123E6C File Offset: 0x0012206C
		public float EffectRadius
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EffectRadius, 0f);
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060035A7 RID: 13735 RVA: 0x00123E83 File Offset: 0x00122083
		public float EffectDuration
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 0f);
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060035A8 RID: 13736 RVA: 0x00123E9A File Offset: 0x0012209A
		public bool HasAreaOfEffect
		{
			get
			{
				return this.EffectRadius > float.Epsilon;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x00123EA9 File Offset: 0x001220A9
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

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060035AA RID: 13738 RVA: 0x00123ECC File Offset: 0x001220CC
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

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060035AB RID: 13739 RVA: 0x00123F2D File Offset: 0x0012212D
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

		// Token: 0x060035AC RID: 13740 RVA: 0x00123F3D File Offset: 0x0012213D
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

		// Token: 0x060035AD RID: 13741 RVA: 0x00123F60 File Offset: 0x00122160
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
						return this.cachedTooltip + "\n\n" + "PsycastIsIllegal".Translate(pawn.Named("PAWN"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE")).Colorize(ColoredText.WarningColor);
					}
				}
			}
			return this.cachedTooltip;
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x001240F1 File Offset: 0x001222F1
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

		// Token: 0x060035AF RID: 13743 RVA: 0x00124108 File Offset: 0x00122308
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
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
				using (List<StatModifier>.Enumerator enumerator2 = this.statBases.GetEnumerator())
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
				List<StatModifier>.Enumerator enumerator2 = default(List<StatModifier>.Enumerator);
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

		// Token: 0x04001D52 RID: 7506
		public Type abilityClass = typeof(Ability);

		// Token: 0x04001D53 RID: 7507
		public Type gizmoClass = typeof(Command_Ability);

		// Token: 0x04001D54 RID: 7508
		public List<AbilityCompProperties> comps = new List<AbilityCompProperties>();

		// Token: 0x04001D55 RID: 7509
		public List<StatModifier> statBases;

		// Token: 0x04001D56 RID: 7510
		public VerbProperties verbProperties;

		// Token: 0x04001D57 RID: 7511
		public KeyBindingDef hotKey;

		// Token: 0x04001D58 RID: 7512
		public JobDef jobDef;

		// Token: 0x04001D59 RID: 7513
		public ThingDef warmupMote;

		// Token: 0x04001D5A RID: 7514
		public Vector3 moteDrawOffset;

		// Token: 0x04001D5B RID: 7515
		public bool canUseAoeToGetTargets = true;

		// Token: 0x04001D5C RID: 7516
		public bool targetRequired = true;

		// Token: 0x04001D5D RID: 7517
		public int level;

		// Token: 0x04001D5E RID: 7518
		public IntRange cooldownTicksRange;

		// Token: 0x04001D5F RID: 7519
		public bool sendLetterOnCooldownComplete;

		// Token: 0x04001D60 RID: 7520
		public bool displayGizmoWhileUndrafted;

		// Token: 0x04001D61 RID: 7521
		public bool disableGizmoWhileUndrafted = true;

		// Token: 0x04001D62 RID: 7522
		public float detectionChanceOverride = -1f;

		// Token: 0x04001D63 RID: 7523
		public string iconPath;

		// Token: 0x04001D64 RID: 7524
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x04001D65 RID: 7525
		private string cachedTooltip;

		// Token: 0x04001D66 RID: 7526
		private List<string> cachedTargets;

		// Token: 0x04001D67 RID: 7527
		private int requiredPsyfocusBandCached = -1;
	}
}
