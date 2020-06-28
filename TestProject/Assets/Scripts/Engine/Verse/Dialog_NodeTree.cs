using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E2 RID: 994
	public class Dialog_NodeTree : Window
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x000B5730 File Offset: 0x000B3930
		public override Vector2 InitialSize
		{
			get
			{
				int num = 480;
				if (this.curNode.options.Count > 5)
				{
					Text.Font = GameFont.Small;
					num += (this.curNode.options.Count - 5) * (int)(Text.LineHeight + 7f);
				}
				return new Vector2(620f, (float)Mathf.Min(num, UI.screenHeight));
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x000B5794 File Offset: 0x000B3994
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000B57A8 File Offset: 0x000B39A8
		public Dialog_NodeTree(DiaNode nodeRoot, bool delayInteractivity = false, bool radioMode = false, string title = null)
		{
			this.title = title;
			this.GotoNode(nodeRoot);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			if (delayInteractivity)
			{
				this.makeInteractiveAtTime = RealTime.LastRealTime + 0.5f;
			}
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (radioMode)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000B5829 File Offset: 0x000B3A29
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000B583C File Offset: 0x000B3A3C
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000B5858 File Offset: 0x000B3A58
		public override void WindowOnGUI()
		{
			if (this.screenFillColor != Color.clear)
			{
				GUI.color = this.screenFillColor;
				GUI.DrawTexture(new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			base.WindowOnGUI();
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000B58B8 File Offset: 0x000B3AB8
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			if (this.title != null)
			{
				Text.Font = GameFont.Small;
				Rect rect2 = rect;
				rect2.height = 36f;
				rect.yMin += 53f;
				Widgets.DrawTitleBG(rect2);
				rect2.xMin += 9f;
				rect2.yMin += 5f;
				Widgets.Label(rect2, this.title);
			}
			this.DrawNode(rect);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000B593C File Offset: 0x000B3B3C
		protected void DrawNode(Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Mathf.Min(this.optTotalHeight, rect.height - 100f - this.Margin * 2f);
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - Mathf.Max(num, this.minOptionsAreaHeight));
			float width = rect.width - 16f;
			Rect rect2 = new Rect(0f, 0f, width, Text.CalcHeight(this.curNode.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Widgets.Label(rect2, this.curNode.text.Resolve());
			Widgets.EndScrollView();
			Widgets.BeginScrollView(new Rect(0f, rect.height - num, rect.width, num), ref this.optsScrollPosition, new Rect(0f, 0f, rect.width - 16f, this.optTotalHeight), true);
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.curNode.options.Count; i++)
			{
				Rect rect3 = new Rect(15f, num2, rect.width - 30f, 999f);
				float num4 = this.curNode.options[i].OptOnGUI(rect3, this.InteractiveNow);
				num2 += num4 + 7f;
				num3 += num4 + 7f;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.optTotalHeight = num3;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000B5AF0 File Offset: 0x000B3CF0
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}

		// Token: 0x040011F7 RID: 4599
		private Vector2 scrollPosition;

		// Token: 0x040011F8 RID: 4600
		private Vector2 optsScrollPosition;

		// Token: 0x040011F9 RID: 4601
		protected string title;

		// Token: 0x040011FA RID: 4602
		protected DiaNode curNode;

		// Token: 0x040011FB RID: 4603
		public Action closeAction;

		// Token: 0x040011FC RID: 4604
		private float makeInteractiveAtTime;

		// Token: 0x040011FD RID: 4605
		public Color screenFillColor = Color.clear;

		// Token: 0x040011FE RID: 4606
		protected float minOptionsAreaHeight;

		// Token: 0x040011FF RID: 4607
		private const float InteractivityDelay = 0.5f;

		// Token: 0x04001200 RID: 4608
		private const float TitleHeight = 36f;

		// Token: 0x04001201 RID: 4609
		protected const float OptHorMargin = 15f;

		// Token: 0x04001202 RID: 4610
		protected const float OptVerticalSpace = 7f;

		// Token: 0x04001203 RID: 4611
		private const int ResizeIfMoreOptionsThan = 5;

		// Token: 0x04001204 RID: 4612
		private const float MinSpaceLeftForTextAfterOptionsResizing = 100f;

		// Token: 0x04001205 RID: 4613
		private float optTotalHeight;
	}
}
