using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DD RID: 989
	public class Dialog_Slider : Window
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x000B4771 File Offset: 0x000B2971
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f);
			}
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000B4784 File Offset: 0x000B2984
		public Dialog_Slider(Func<int, string> textGetter, int from, int to, Action<int> confirmAction, int startingValue = -2147483648)
		{
			this.textGetter = textGetter;
			this.from = from;
			this.to = to;
			this.confirmAction = confirmAction;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			if (startingValue == -2147483648)
			{
				this.curValue = from;
				return;
			}
			this.curValue = startingValue;
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000B47DC File Offset: 0x000B29DC
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue)
		{
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000B4810 File Offset: 0x000B2A10
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect.x, inRect.y + 15f, inRect.width, 30f);
			this.curValue = (int)Widgets.HorizontalSlider(rect, (float)this.curValue, (float)this.from, (float)this.to, true, this.textGetter(this.curValue), null, null, 1f);
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 30f, inRect.width / 2f, 30f), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 2f, inRect.yMax - 30f, inRect.width / 2f, 30f), "OK".Translate(), true, true, true))
			{
				this.Close(true);
				this.confirmAction(this.curValue);
			}
		}

		// Token: 0x040011CD RID: 4557
		public Func<int, string> textGetter;

		// Token: 0x040011CE RID: 4558
		public int from;

		// Token: 0x040011CF RID: 4559
		public int to;

		// Token: 0x040011D0 RID: 4560
		private Action<int> confirmAction;

		// Token: 0x040011D1 RID: 4561
		private int curValue;

		// Token: 0x040011D2 RID: 4562
		private const float BotAreaHeight = 30f;

		// Token: 0x040011D3 RID: 4563
		private const float TopPadding = 15f;
	}
}
