using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public class HediffStage
	{
		
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001B78A File Offset: 0x0001998A
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001B7A8 File Offset: 0x000199A8
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		
		public float minSeverity;

		
		[MustTranslate]
		public string label;

		
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		
		public bool becomeVisible = true;

		
		public bool lifeThreatening;

		
		public TaleDef tale;

		
		public float vomitMtbDays = -1f;

		
		public float deathMtbDays = -1f;

		
		public float painFactor = 1f;

		
		public float painOffset;

		
		public float totalBleedFactor = 1f;

		
		public float naturalHealingFactor = -1f;

		
		public float forgetMemoryThoughtMtbDays = -1f;

		
		public float pctConditionalThoughtsNullified;

		
		public float opinionOfOthersFactor = 1f;

		
		public float hungerRateFactor = 1f;

		
		public float hungerRateFactorOffset;

		
		public float restFallFactor = 1f;

		
		public float restFallFactorOffset;

		
		public float socialFightChanceFactor = 1f;

		
		public float foodPoisoningChanceFactor = 1f;

		
		public float mentalBreakMtbDays = -1f;

		
		public List<MentalBreakIntensity> allowedMentalBreakIntensities;

		
		public List<HediffDef> makeImmuneTo;

		
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		
		public List<HediffGiver> hediffGivers;

		
		public List<MentalStateGiver> mentalStateGivers;

		
		public List<StatModifier> statOffsets;

		
		public List<StatModifier> statFactors;

		
		public StatDef statOffsetEffectMultiplier;

		
		public StatDef statFactorEffectMultiplier;

		
		public StatDef capacityFactorEffectMultiplier;

		
		public WorkTags disabledWorkTags;

		
		public float partEfficiencyOffset;

		
		public bool partIgnoreMissingHP;

		
		public bool destroyPart;
	}
}
