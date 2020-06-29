using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ThoughtWorker_RoyalTitleApparelMinQualityNotMet : ThoughtWorker
	{
		
		private RoyalTitleDef Validate(Pawn p, out QualityCategory minQuality)
		{
			minQuality = QualityCategory.Awful;
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				minQuality = royalTitle.def.requiredMinimumApparelQuality;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					QualityCategory qualityCategory;
					if (wornApparel[i].TryGetQuality(out qualityCategory) && qualityCategory < royalTitle.def.requiredMinimumApparelQuality)
					{
						return royalTitle.def;
					}
				}
			}
			return null;
		}

		
		public override string PostProcessLabel(Pawn p, string label)
		{
			QualityCategory qualityCategory;
			RoyalTitleDef royalTitleDef = this.Validate(p, out qualityCategory);
			if (royalTitleDef == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitleDef.GetLabelCapFor(p).Named("TITLE"), p.Named("PAWN"));
		}

		
		public override string PostProcessDescription(Pawn p, string description)
		{
			QualityCategory cat;
			RoyalTitleDef royalTitleDef = this.Validate(p, out cat);
			if (royalTitleDef == null)
			{
				return string.Empty;
			}
			return description.Formatted(royalTitleDef.GetLabelCapFor(p).Named("TITLE"), cat.GetLabel().Named("QUALITY"), p.Named("PAWN"));
		}

		
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			QualityCategory qualityCategory;
			if (this.Validate(p, out qualityCategory) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
