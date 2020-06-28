using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000918 RID: 2328
	public class TraitDegreeData
	{
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003752 RID: 14162 RVA: 0x00129797 File Offset: 0x00127997
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x001297B8 File Offset: 0x001279B8
		public TraitMentalStateGiver MentalStateGiver
		{
			get
			{
				if (this.mentalStateGiverInt == null)
				{
					this.mentalStateGiverInt = (TraitMentalStateGiver)Activator.CreateInstance(this.mentalStateGiverClass);
					this.mentalStateGiverInt.traitDegreeData = this;
				}
				return this.mentalStateGiverInt;
			}
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x001297EA File Offset: 0x001279EA
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0400208C RID: 8332
		[MustTranslate]
		public string label;

		// Token: 0x0400208D RID: 8333
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x0400208E RID: 8334
		[MustTranslate]
		public string description;

		// Token: 0x0400208F RID: 8335
		public int degree;

		// Token: 0x04002090 RID: 8336
		public float commonality = 1f;

		// Token: 0x04002091 RID: 8337
		public List<StatModifier> statOffsets;

		// Token: 0x04002092 RID: 8338
		public List<StatModifier> statFactors;

		// Token: 0x04002093 RID: 8339
		public ThinkTreeDef thinkTree;

		// Token: 0x04002094 RID: 8340
		public MentalStateDef randomMentalState;

		// Token: 0x04002095 RID: 8341
		public SimpleCurve randomMentalStateMtbDaysMoodCurve;

		// Token: 0x04002096 RID: 8342
		public List<MentalStateDef> disallowedMentalStates;

		// Token: 0x04002097 RID: 8343
		public List<InspirationDef> disallowedInspirations;

		// Token: 0x04002098 RID: 8344
		public List<InspirationDef> mentalBreakInspirationGainSet;

		// Token: 0x04002099 RID: 8345
		public string mentalBreakInspirationGainReasonText;

		// Token: 0x0400209A RID: 8346
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		// Token: 0x0400209B RID: 8347
		public List<MeditationFocusDef> disallowedMeditationFocusTypes;

		// Token: 0x0400209C RID: 8348
		public float mentalBreakInspirationGainChance;

		// Token: 0x0400209D RID: 8349
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks;

		// Token: 0x0400209E RID: 8350
		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		// Token: 0x0400209F RID: 8351
		public float socialFightChanceFactor = 1f;

		// Token: 0x040020A0 RID: 8352
		public float marketValueFactorOffset;

		// Token: 0x040020A1 RID: 8353
		public float randomDiseaseMtbDays;

		// Token: 0x040020A2 RID: 8354
		public float hungerRateFactor = 1f;

		// Token: 0x040020A3 RID: 8355
		public Type mentalStateGiverClass = typeof(TraitMentalStateGiver);

		// Token: 0x040020A4 RID: 8356
		[Unsaved(false)]
		private TraitMentalStateGiver mentalStateGiverInt;

		// Token: 0x040020A5 RID: 8357
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
