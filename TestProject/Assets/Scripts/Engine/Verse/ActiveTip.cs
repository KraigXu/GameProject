using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B9 RID: 953
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x000AB110 File Offset: 0x000A9310
		private string FinalText
		{
			get
			{
				string text;
				if (this.signal.textGetter != null)
				{
					try
					{
						text = this.signal.textGetter();
						goto IL_3F;
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString(), false);
						text = "Error getting tip text.";
						goto IL_3F;
					}
				}
				text = this.signal.text;
				IL_3F:
				return text.TrimEnd(Array.Empty<char>());
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x000AB178 File Offset: 0x000A9378
		public Rect TipRect
		{
			get
			{
				string finalText = this.FinalText;
				Vector2 vector = Text.CalcSize(finalText);
				if (vector.x > 260f)
				{
					vector.x = 260f;
					vector.y = Text.CalcHeight(finalText, vector.x);
				}
				return new Rect(0f, 0f, vector.x, vector.y).ContractedBy(-4f);
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000AB1E4 File Offset: 0x000A93E4
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000AB1F3 File Offset: 0x000A93F3
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000AB220 File Offset: 0x000A9420
		public float DrawTooltip(Vector2 pos)
		{
			Text.Font = GameFont.Small;
			string finalText = this.FinalText;
			Rect bgRect = this.TipRect;
			bgRect.position = pos;
			if (!LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting)
			{
				Find.WindowStack.ImmediateWindow(153 * this.signal.uniqueId + 62346, bgRect, WindowLayer.Super, delegate
				{
					this.DrawInner(bgRect.AtZero(), finalText);
				}, false, false, 1f);
			}
			else
			{
				Widgets.DrawShadowAround(bgRect);
				Widgets.DrawWindowBackground(bgRect);
				this.DrawInner(bgRect, finalText);
			}
			return bgRect.height;
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000AB2DB File Offset: 0x000A94DB
		private void DrawInner(Rect bgRect, string label)
		{
			Widgets.DrawAtlas(bgRect, ActiveTip.TooltipBGAtlas);
			Text.Font = GameFont.Small;
			Widgets.Label(bgRect.ContractedBy(4f), label);
		}

		// Token: 0x04001091 RID: 4241
		public TipSignal signal;

		// Token: 0x04001092 RID: 4242
		public double firstTriggerTime;

		// Token: 0x04001093 RID: 4243
		public int lastTriggerFrame;

		// Token: 0x04001094 RID: 4244
		private const int TipMargin = 4;

		// Token: 0x04001095 RID: 4245
		private const float MaxWidth = 260f;

		// Token: 0x04001096 RID: 4246
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}
