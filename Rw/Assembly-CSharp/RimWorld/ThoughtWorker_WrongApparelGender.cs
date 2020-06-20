using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082D RID: 2093
	public class ThoughtWorker_WrongApparelGender : ThoughtWorker
	{
		// Token: 0x06003467 RID: 13415 RVA: 0x0011FC14 File Offset: 0x0011DE14
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.Named("PAWN"));
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x0011FC48 File Offset: 0x0011DE48
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.gender.Opposite().GetLabel(false).ToLower(), p.gender.GetLabel(false), p.Named("PAWN"));
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x0011FC98 File Offset: 0x0011DE98
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!wornApparel[i].def.apparel.CorrectGenderForWearing(p.gender))
				{
					return ThoughtState.ActiveDefault;
				}
			}
			return false;
		}
	}
}
