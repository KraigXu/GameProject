using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000807 RID: 2055
	public class Thought_OpinionOfMyLover : Thought_Situational
	{
		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x0011EA90 File Offset: 0x0011CC90
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false);
				string text = base.CurStage.label.Formatted(directPawnRelation.def.GetGenderSpecificLabel(directPawnRelation.otherPawn), directPawnRelation.otherPawn.LabelShort, directPawnRelation.otherPawn).CapitalizeFirst();
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessLabel(this.pawn, text);
				}
				return text;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x0011EB20 File Offset: 0x0011CD20
		protected override float BaseMoodOffset
		{
			get
			{
				float num = 0.1f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn);
				if (num < 0f)
				{
					return Mathf.Min(num, -1f);
				}
				return Mathf.Max(num, 1f);
			}
		}
	}
}
