using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000BB RID: 187
	public class HediffStage
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001B78A File Offset: 0x0001998A
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001B7A8 File Offset: 0x000199A8
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001B7BA File Offset: 0x000199BA
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001B7C8 File Offset: 0x000199C8
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		// Token: 0x040003E4 RID: 996
		public float minSeverity;

		// Token: 0x040003E5 RID: 997
		[MustTranslate]
		public string label;

		// Token: 0x040003E6 RID: 998
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x040003E7 RID: 999
		public bool becomeVisible = true;

		// Token: 0x040003E8 RID: 1000
		public bool lifeThreatening;

		// Token: 0x040003E9 RID: 1001
		public TaleDef tale;

		// Token: 0x040003EA RID: 1002
		public float vomitMtbDays = -1f;

		// Token: 0x040003EB RID: 1003
		public float deathMtbDays = -1f;

		// Token: 0x040003EC RID: 1004
		public float painFactor = 1f;

		// Token: 0x040003ED RID: 1005
		public float painOffset;

		// Token: 0x040003EE RID: 1006
		public float totalBleedFactor = 1f;

		// Token: 0x040003EF RID: 1007
		public float naturalHealingFactor = -1f;

		// Token: 0x040003F0 RID: 1008
		public float forgetMemoryThoughtMtbDays = -1f;

		// Token: 0x040003F1 RID: 1009
		public float pctConditionalThoughtsNullified;

		// Token: 0x040003F2 RID: 1010
		public float opinionOfOthersFactor = 1f;

		// Token: 0x040003F3 RID: 1011
		public float hungerRateFactor = 1f;

		// Token: 0x040003F4 RID: 1012
		public float hungerRateFactorOffset;

		// Token: 0x040003F5 RID: 1013
		public float restFallFactor = 1f;

		// Token: 0x040003F6 RID: 1014
		public float restFallFactorOffset;

		// Token: 0x040003F7 RID: 1015
		public float socialFightChanceFactor = 1f;

		// Token: 0x040003F8 RID: 1016
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x040003F9 RID: 1017
		public float mentalBreakMtbDays = -1f;

		// Token: 0x040003FA RID: 1018
		public List<MentalBreakIntensity> allowedMentalBreakIntensities;

		// Token: 0x040003FB RID: 1019
		public List<HediffDef> makeImmuneTo;

		// Token: 0x040003FC RID: 1020
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		// Token: 0x040003FD RID: 1021
		public List<HediffGiver> hediffGivers;

		// Token: 0x040003FE RID: 1022
		public List<MentalStateGiver> mentalStateGivers;

		// Token: 0x040003FF RID: 1023
		public List<StatModifier> statOffsets;

		// Token: 0x04000400 RID: 1024
		public List<StatModifier> statFactors;

		// Token: 0x04000401 RID: 1025
		public StatDef statOffsetEffectMultiplier;

		// Token: 0x04000402 RID: 1026
		public StatDef statFactorEffectMultiplier;

		// Token: 0x04000403 RID: 1027
		public StatDef capacityFactorEffectMultiplier;

		// Token: 0x04000404 RID: 1028
		public WorkTags disabledWorkTags;

		// Token: 0x04000405 RID: 1029
		public float partEfficiencyOffset;

		// Token: 0x04000406 RID: 1030
		public bool partIgnoreMissingHP;

		// Token: 0x04000407 RID: 1031
		public bool destroyPart;
	}
}
