using System;
using Verse;

namespace RimWorld
{
	
	public class FocusStrengthOffset_Lit : FocusStrengthOffset
	{
		
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Lit".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Lit".Translate() + ": " + this.offset.ToStringWithSign("0%");
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
			CompGlower compGlower = parent.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}
	}
}
