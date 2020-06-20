using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084A RID: 2122
	public class ThoughtWorker_RoyalTitleApparelMinQualityNotMet : ThoughtWorker
	{
		// Token: 0x060034A9 RID: 13481 RVA: 0x00120994 File Offset: 0x0011EB94
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

		// Token: 0x060034AA RID: 13482 RVA: 0x00120A44 File Offset: 0x0011EC44
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

		// Token: 0x060034AB RID: 13483 RVA: 0x00120A8C File Offset: 0x0011EC8C
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

		// Token: 0x060034AC RID: 13484 RVA: 0x00120AE4 File Offset: 0x0011ECE4
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
