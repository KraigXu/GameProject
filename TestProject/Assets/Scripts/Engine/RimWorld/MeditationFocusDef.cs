using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class MeditationFocusDef : Def
	{
		
		public bool CanPawnUse(Pawn p)
		{
			return MeditationFocusTypeAvailabilityCache.PawnCanUse(p, this);
		}

		
		public string EnablingThingsExplanation(Pawn pawn)
		{
			//MeditationFocusDef.c__DisplayClass4_0 c__DisplayClass4_;
			//c__DisplayClass4_.reasons = new List<string>();
			//if (this.requiresRoyalTitle && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			//{
			//	RoyalTitle royalTitle = pawn.royalty.AllTitlesInEffectForReading.MaxBy((RoyalTitle t) => t.def.seniority);
			//	c__DisplayClass4_.reasons.Add("MeditationFocusEnabledByTitle".Translate(royalTitle.def.GetLabelCapFor(pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).Resolve());
			//}
			//if (pawn.story != null)
			//{
			//	Backstory adulthood = pawn.story.adulthood;
			//	Backstory childhood = pawn.story.childhood;
			//	if (!this.requiresRoyalTitle && this.requiredBackstoriesAny.Count == 0)
			//	{
			//		for (int i = 0; i < this.incompatibleBackstoriesAny.Count; i++)
			//		{
			//			BackstoryCategoryAndSlot backstoryCategoryAndSlot = this.incompatibleBackstoriesAny[i];
			//			Backstory backstory = (backstoryCategoryAndSlot.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
			//			if (!backstory.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName))
			//			{
			//				MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|4_0(backstoryCategoryAndSlot.slot, backstory, ref c__DisplayClass4_);
			//			}
			//		}
			//		for (int j = 0; j < DefDatabase<TraitDef>.AllDefsListForReading.Count; j++)
			//		{
			//			TraitDef traitDef = DefDatabase<TraitDef>.AllDefsListForReading[j];
			//			List<MeditationFocusDef> disallowedMeditationFocusTypes = traitDef.degreeDatas[0].disallowedMeditationFocusTypes;
			//			if (disallowedMeditationFocusTypes != null && disallowedMeditationFocusTypes.Contains(this))
			//			{
			//				c__DisplayClass4_.reasons.Add("MeditationFocusDisabledByTrait".Translate() + ": " + traitDef.degreeDatas[0].LabelCap + ".");
			//			}
			//		}
			//	}
			//	for (int k = 0; k < this.requiredBackstoriesAny.Count; k++)
			//	{
			//		BackstoryCategoryAndSlot backstoryCategoryAndSlot2 = this.requiredBackstoriesAny[k];
			//		Backstory backstory2 = (backstoryCategoryAndSlot2.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
			//		if (backstory2.spawnCategories.Contains(backstoryCategoryAndSlot2.categoryName))
			//		{
			//			MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|4_0(backstoryCategoryAndSlot2.slot, backstory2, ref c__DisplayClass4_);
			//		}
			//	}
			//	for (int l = 0; l < pawn.story.traits.allTraits.Count; l++)
			//	{
			//		Trait trait = pawn.story.traits.allTraits[l];
			//		List<MeditationFocusDef> allowedMeditationFocusTypes = trait.CurrentData.allowedMeditationFocusTypes;
			//		if (allowedMeditationFocusTypes != null && allowedMeditationFocusTypes.Contains(this))
			//		{
			//			c__DisplayClass4_.reasons.Add("MeditationFocusEnabledByTrait".Translate() + ": " + trait.LabelCap + ".");
			//		}
			//	}
			//}
			//return c__DisplayClass4_.reasons.ToLineList("  - ", true);
			return default;
		}

		
		public bool requiresRoyalTitle;

		
		public List<BackstoryCategoryAndSlot> requiredBackstoriesAny = new List<BackstoryCategoryAndSlot>();

		
		public List<BackstoryCategoryAndSlot> incompatibleBackstoriesAny = new List<BackstoryCategoryAndSlot>();
	}
}
