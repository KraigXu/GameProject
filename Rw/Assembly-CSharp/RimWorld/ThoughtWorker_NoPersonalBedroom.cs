using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084F RID: 2127
	public class ThoughtWorker_NoPersonalBedroom : ThoughtWorker
	{
		// Token: 0x060034BA RID: 13498 RVA: 0x00120C78 File Offset: 0x0011EE78
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithBedroomRequirements() == null)
			{
				return false;
			}
			return !p.royalty.HasPersonalBedroom();
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x00120CC9 File Offset: 0x0011EEC9
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.royalty.HighestTitleWithBedroomRequirements().def.GetLabelCapFor(p).Named("TITLE"));
		}
	}
}
