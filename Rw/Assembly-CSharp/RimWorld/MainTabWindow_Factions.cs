using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000EC3 RID: 3779
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x06005C51 RID: 23633 RVA: 0x001FDEB1 File Offset: 0x001FC0B1
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight);
		}

		// Token: 0x0400324F RID: 12879
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04003250 RID: 12880
		private float scrollViewHeight;
	}
}
