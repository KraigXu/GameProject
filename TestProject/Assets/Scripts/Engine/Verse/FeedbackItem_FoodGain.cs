using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000388 RID: 904
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x06001ABF RID: 6847 RVA: 0x000A48C7 File Offset: 0x000A2AC7
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x000A48D8 File Offset: 0x000A2AD8
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x04000FCF RID: 4047
		protected int Amount;
	}
}
