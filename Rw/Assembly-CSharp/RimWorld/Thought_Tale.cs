using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000825 RID: 2085
	public class Thought_Tale : Thought_SituationalSocial
	{
		// Token: 0x06003457 RID: 13399 RVA: 0x0011F8C0 File Offset: 0x0011DAC0
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			Tale latestTale = Find.TaleManager.GetLatestTale(this.def.taleDef, this.otherPawn);
			if (latestTale != null)
			{
				float num = 1f;
				if (latestTale.def.type == TaleType.Expirable)
				{
					float value = (float)latestTale.AgeTicks / (latestTale.def.expireDays * 60000f);
					num = Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, value);
				}
				return base.CurStage.baseOpinionOffset * num;
			}
			return 0f;
		}
	}
}
