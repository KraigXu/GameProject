using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084C RID: 2124
	public class ThoughtWorker_ThroneRoomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x060034B2 RID: 13490 RVA: 0x00120B5B File Offset: 0x0011ED5B
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetThroneroomRequirements(false, false);
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x00120B6C File Offset: 0x0011ED6C
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithThroneRoomRequirements().def.GetLabelCapFor(p).Named("TITLE"));
		}
	}
}
