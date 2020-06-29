using System;
using Verse;

namespace RimWorld
{
	
	public class FocusStrengthOffset_GraveFull : FocusStrengthOffset
	{
		
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				Building_Grave building_Grave = parent as Building_Grave;
				return "StatsReport_GraveFull".Translate(building_Grave.Corpse.InnerPawn.LabelShortCap) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveFullAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
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
			Building_Grave building_Grave;
			return parent.Spawned && (building_Grave = (parent as Building_Grave)) != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike;
		}
	}
}
