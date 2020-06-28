using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082A RID: 2090
	public class ThoughtWorker_DeadMansApparel : ThoughtWorker
	{
		// Token: 0x06003461 RID: 13409 RVA: 0x0011FA80 File Offset: 0x0011DC80
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].WornByCorpse)
				{
					if (text == null)
					{
						text = wornApparel[i].def.label;
					}
					num++;
				}
			}
			if (num == 0)
			{
				return ThoughtState.Inactive;
			}
			if (num >= 5)
			{
				return ThoughtState.ActiveAtStage(4, text);
			}
			return ThoughtState.ActiveAtStage(num - 1, text);
		}
	}
}
