using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Thought_WantToSleepWithSpouseOrLover : Thought_Situational
	{
		
		
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

		
		
		protected override float BaseMoodOffset
		{
			get
			{
				return Mathf.Min(-0.05f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn), -1f);
			}
		}
	}
}
