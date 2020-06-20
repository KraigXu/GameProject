using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084D RID: 2125
	public class ThoughtWorker_BedroomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x060034B5 RID: 13493 RVA: 0x00120BC3 File Offset: 0x0011EDC3
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetBedroomRequirements(false, false);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x00120BD4 File Offset: 0x0011EDD4
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithBedroomRequirements().def.GetLabelCapFor(p).Named("TITLE"));
		}
	}
}
