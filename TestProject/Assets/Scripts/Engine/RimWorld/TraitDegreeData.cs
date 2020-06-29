using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class TraitDegreeData
	{
		
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

		
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		
		[MustTranslate]
		public string label;

		
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		
		[MustTranslate]
		public string description;

		
		public int degree;

		
		public float commonality = 1f;

		
		public List<StatModifier> statOffsets;

		
		public List<StatModifier> statFactors;

		
		public ThinkTreeDef thinkTree;

		
		public MentalStateDef randomMentalState;

		
		public SimpleCurve randomMentalStateMtbDaysMoodCurve;

		
		public List<MentalStateDef> disallowedMentalStates;

		
		public List<InspirationDef> disallowedInspirations;

		
		public List<InspirationDef> mentalBreakInspirationGainSet;

		
		public string mentalBreakInspirationGainReasonText;

		
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		
		public List<MeditationFocusDef> disallowedMeditationFocusTypes;

		
		public float mentalBreakInspirationGainChance;

		
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks;

		
		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		
		public float socialFightChanceFactor = 1f;

		
		public float marketValueFactorOffset;

		
		public float randomDiseaseMtbDays;

		
		public float hungerRateFactor = 1f;

		
		public Type mentalStateGiverClass = typeof(TraitMentalStateGiver);

		
		[Unsaved(false)]
		private TraitMentalStateGiver mentalStateGiverInt;

		
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
