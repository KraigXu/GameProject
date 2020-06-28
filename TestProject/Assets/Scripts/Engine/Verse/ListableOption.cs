using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C7 RID: 967
	public class ListableOption
	{
		// Token: 0x06001C8D RID: 7309 RVA: 0x000ADA46 File Offset: 0x000ABC46
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000ADA70 File Offset: 0x000ABC70
		public virtual float DrawOption(Vector2 pos, float width)
		{
			float b = Text.CalcHeight(this.label, width);
			float num = Mathf.Max(this.minHeight, b);
			Rect rect = new Rect(pos.x, pos.y, width, num);
			if (Widgets.ButtonText(rect, this.label, true, true, true))
			{
				this.action();
			}
			if (this.uiHighlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.uiHighlightTag);
			}
			return num;
		}

		// Token: 0x040010D3 RID: 4307
		public string label;

		// Token: 0x040010D4 RID: 4308
		public Action action;

		// Token: 0x040010D5 RID: 4309
		private string uiHighlightTag;

		// Token: 0x040010D6 RID: 4310
		public float minHeight = 45f;
	}
}
