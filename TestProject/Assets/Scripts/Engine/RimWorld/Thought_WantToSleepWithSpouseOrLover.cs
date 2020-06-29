using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Thought_WantToSleepWithSpouseOrLover : Thought_Situational
	{
		
		// (get) Token: 0x06003428 RID: 13352 RVA: 0x0011EE44 File Offset: 0x0011D044
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false);
				string text = base.CurStage.label.Formatted(directPawnRelation.otherPawn.LabelShort, directPawnRelation.otherPawn).CapitalizeFirst();
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessLabel(this.pawn, text);
				}
				return text;
			}
		}

		
		// (get) Token: 0x06003429 RID: 13353 RVA: 0x0011EEBD File Offset: 0x0011D0BD
		protected override float BaseMoodOffset
		{
			get
			{
				return Mathf.Min(-0.05f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn), -1f);
			}
		}
	}
}
