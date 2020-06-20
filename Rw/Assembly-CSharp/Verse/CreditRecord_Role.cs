using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000405 RID: 1029
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x06001E67 RID: 7783 RVA: 0x000BDCF7 File Offset: 0x000BBEF7
		public CreditRecord_Role()
		{
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000BDD42 File Offset: 0x000BBF42
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000BDD5F File Offset: 0x000BBF5F
		public override float DrawHeight(float width)
		{
			if (this.roleKey.NullOrEmpty())
			{
				width *= 0.5f;
			}
			if (!this.compressed)
			{
				return 50f;
			}
			return Text.CalcHeight(this.creditee, width * 0.5f);
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000BDD98 File Offset: 0x000BBF98
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			if (!this.roleKey.NullOrEmpty())
			{
				rect2.width = rect.width / 2f;
				if (this.displayKey)
				{
					Widgets.Label(rect2, this.roleKey.Translate());
				}
			}
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			if (this.roleKey.NullOrEmpty())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			Widgets.Label(rect3, this.creditee);
			Text.Anchor = TextAnchor.MiddleLeft;
			if (!this.extra.NullOrEmpty())
			{
				Rect rect4 = rect3;
				rect4.yMin += 28f;
				Text.Font = GameFont.Tiny;
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				Widgets.Label(rect4, this.extra);
				GUI.color = Color.white;
			}
		}

		// Token: 0x040012CD RID: 4813
		public string roleKey;

		// Token: 0x040012CE RID: 4814
		public string creditee;

		// Token: 0x040012CF RID: 4815
		public string extra;

		// Token: 0x040012D0 RID: 4816
		public bool displayKey;

		// Token: 0x040012D1 RID: 4817
		public bool compressed;
	}
}
