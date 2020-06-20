using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082C RID: 2092
	public class ThoughtWorker_ApparelDamaged : ThoughtWorker
	{
		// Token: 0x06003465 RID: 13413 RVA: 0x0011FB78 File Offset: 0x0011DD78
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float num = 999f;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].def.useHitPoints && !p.apparel.IsLocked(wornApparel[i]))
				{
					float num2 = (float)wornApparel[i].HitPoints / (float)wornApparel[i].MaxHitPoints;
					if (num2 < num)
					{
						num = num2;
					}
					if (num < 0.2f)
					{
						return ThoughtState.ActiveAtStage(1);
					}
				}
			}
			if (num < 0.5f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x04001BB5 RID: 7093
		public const float MinForFrayed = 0.5f;

		// Token: 0x04001BB6 RID: 7094
		public const float MinForTattered = 0.2f;
	}
}
