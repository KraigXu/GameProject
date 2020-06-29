using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class FocusStrengthOffset_ThroneSatisfiesRequirements : FocusStrengthOffset
	{
		
		public override string GetExplanation(Thing parent)
		{
			return this.GetExplanationAbstract(null);
		}

		
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_SatisfiesTitle".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (!this.CanApply(parent, user))
			{
				return 0f;
			}
			return this.offset;
		}

		
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (user == null)
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = user.royalty;
			bool? flag = (royalty != null) ? new bool?(royalty.GetUnmetThroneroomRequirements(true, false).Any<string>()) : null;
			bool flag2 = false;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}
	}
}
