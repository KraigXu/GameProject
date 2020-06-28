using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000387 RID: 903
	public abstract class FeedbackItem
	{
		// Token: 0x06001ABB RID: 6843 RVA: 0x000A4798 File Offset: 0x000A2998
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x000A47F9 File Offset: 0x000A29F9
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x06001ABD RID: 6845
		public abstract void FeedbackOnGUI();

		// Token: 0x06001ABE RID: 6846 RVA: 0x000A4830 File Offset: 0x000A2A30
		protected void DrawFloatingText(string str, Color TextColor)
		{
			float x = Text.CalcSize(str).x;
			Rect wordRect = new Rect(this.CurScreenPos.x - x / 2f, this.CurScreenPos.y, x, 20f);
			Find.WindowStack.ImmediateWindow(5983 * this.uniqueID + 495, wordRect, WindowLayer.Super, delegate
			{
				Rect rect = wordRect.AtZero();
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				GUI.DrawTexture(rect, TexUI.GrayTextBG);
				GUI.color = TextColor;
				Widgets.Label(rect, str);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, false, false, 1f);
		}

		// Token: 0x04000FCA RID: 4042
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x04000FCB RID: 4043
		private int uniqueID;

		// Token: 0x04000FCC RID: 4044
		public float TimeLeft = 2f;

		// Token: 0x04000FCD RID: 4045
		protected Vector2 CurScreenPos;

		// Token: 0x04000FCE RID: 4046
		private static int freeUniqueID;
	}
}
