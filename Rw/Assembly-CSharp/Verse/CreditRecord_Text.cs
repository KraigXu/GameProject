using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000404 RID: 1028
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x06001E63 RID: 7779 RVA: 0x000BDCF7 File Offset: 0x000BBEF7
		public CreditRecord_Text()
		{
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000BDCFF File Offset: 0x000BBEFF
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000BDD15 File Offset: 0x000BBF15
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000BDD23 File Offset: 0x000BBF23
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040012CB RID: 4811
		public string text;

		// Token: 0x040012CC RID: 4812
		public TextAnchor anchor;
	}
}
