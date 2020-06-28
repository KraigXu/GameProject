using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000406 RID: 1030
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x06001E6B RID: 7787 RVA: 0x000BDCF7 File Offset: 0x000BBEF7
		public CreditRecord_Title()
		{
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000BDE85 File Offset: 0x000BC085
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000BDE94 File Offset: 0x000BC094
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000BDE9C File Offset: 0x000BC09C
		public override void Draw(Rect rect)
		{
			rect.yMin += 31f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(rect.x + 10f, Mathf.Round(rect.yMax) - 14f, rect.width - 20f);
			GUI.color = Color.white;
		}

		// Token: 0x040012D2 RID: 4818
		public string title;
	}
}
