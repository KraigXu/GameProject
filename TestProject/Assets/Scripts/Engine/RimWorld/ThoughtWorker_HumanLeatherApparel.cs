using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082B RID: 2091
	public class ThoughtWorker_HumanLeatherApparel : ThoughtWorker
	{
		// Token: 0x06003463 RID: 13411 RVA: 0x0011FAF4 File Offset: 0x0011DCF4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].Stuff == ThingDefOf.Human.race.leatherDef)
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
