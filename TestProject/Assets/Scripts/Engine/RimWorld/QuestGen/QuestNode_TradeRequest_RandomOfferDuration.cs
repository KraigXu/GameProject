using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200119D RID: 4509
	public class QuestNode_TradeRequest_RandomOfferDuration : QuestNode
	{
		// Token: 0x06006867 RID: 26727 RVA: 0x00247674 File Offset: 0x00245874
		public static int RandomOfferDurationTicks(int tileIdFrom, int tileIdTo, out int travelTicks)
		{
			int randomInRange = SiteTuning.QuestSiteTimeoutDaysRange.RandomInRange;
			travelTicks = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(tileIdFrom, tileIdTo, null);
			float num = (float)travelTicks / 60000f;
			int num2 = Mathf.CeilToInt(Mathf.Max(num + 6f, num * 1.35f));
			if (num2 > SiteTuning.QuestSiteTimeoutDaysRange.max)
			{
				return -1;
			}
			int num3 = Mathf.Max(randomInRange, num2);
			return 60000 * num3;
		}

		// Token: 0x06006868 RID: 26728 RVA: 0x002476DC File Offset: 0x002458DC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x00247744 File Offset: 0x00245944
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x040040B5 RID: 16565
		private const float MinNonTravelTimeFractionOfTravelTime = 0.35f;

		// Token: 0x040040B6 RID: 16566
		private const float MinNonTravelTimeDays = 6f;

		// Token: 0x040040B7 RID: 16567
		public SlateRef<Settlement> settlement;

		// Token: 0x040040B8 RID: 16568
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040040B9 RID: 16569
		[NoTranslate]
		public SlateRef<string> storeEstimatedTravelTimeAs;
	}
}
