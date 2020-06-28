using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000389 RID: 905
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x06001AC1 RID: 6849 RVA: 0x000A4907 File Offset: 0x000A2B07
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000A4920 File Offset: 0x000A2B20
		public override void FeedbackOnGUI()
		{
			string text;
			if (this.Amount >= 0)
			{
				text = "+";
			}
			else
			{
				text = "-";
			}
			text += this.Amount;
			base.DrawFloatingText(text, Color.red);
		}

		// Token: 0x04000FD0 RID: 4048
		protected Pawn Healer;

		// Token: 0x04000FD1 RID: 4049
		protected int Amount;
	}
}
