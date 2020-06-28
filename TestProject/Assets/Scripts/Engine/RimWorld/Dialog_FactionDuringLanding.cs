using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E51 RID: 3665
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x0600588A RID: 22666 RVA: 0x001270D1 File Offset: 0x001252D1
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x001D6684 File Offset: 0x001D4884
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x001D66AC File Offset: 0x001D48AC
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - this.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight);
		}

		// Token: 0x04002FC8 RID: 12232
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04002FC9 RID: 12233
		private float scrollViewHeight;
	}
}
