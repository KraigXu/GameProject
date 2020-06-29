using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ThoughtWorker_ThroneRoomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetThroneroomRequirements(false, false);
		}

		
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithThroneRoomRequirements().def.GetLabelCapFor(p).Named("TITLE"));
		}
	}
}
