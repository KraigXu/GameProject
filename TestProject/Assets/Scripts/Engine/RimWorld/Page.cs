using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E83 RID: 3715
	public abstract class Page : Window
	{
		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06005A63 RID: 23139 RVA: 0x001EAAA1 File Offset: 0x001E8CA1
		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x001EAAA8 File Offset: 0x001E8CA8
		public Page()
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x001EAAD3 File Offset: 0x001E8CD3
		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x001EAB08 File Offset: 0x001E8D08
		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = 45f + extraTopSpace;
			}
			return new Rect(0f, num, rect.width, rect.height - 38f - num - 17f);
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x001EAB50 File Offset: 0x001E8D50
		protected void DoBottomButtons(Rect rect, string nextLabel = null, string midLabel = null, Action midAct = null, bool showNext = true, bool doNextOnKeypress = true)
		{
			float y = rect.y + rect.height - 38f;
			Text.Font = GameFont.Small;
			string label = "Back".Translate();
			if ((Widgets.ButtonText(new Rect(rect.x, y, Page.BottomButSize.x, Page.BottomButSize.y), label, true, true, true) || KeyBindingDefOf.Cancel.KeyDownEvent) && this.CanDoBack())
			{
				this.DoBack();
			}
			if (showNext)
			{
				if (nextLabel.NullOrEmpty())
				{
					nextLabel = "Next".Translate();
				}
				Rect rect2 = new Rect(rect.x + rect.width - Page.BottomButSize.x, y, Page.BottomButSize.x, Page.BottomButSize.y);
				if ((Widgets.ButtonText(rect2, nextLabel, true, true, true) || (doNextOnKeypress && KeyBindingDefOf.Accept.KeyDownEvent)) && this.CanDoNext())
				{
					this.DoNext();
				}
				UIHighlighter.HighlightOpportunity(rect2, "NextPage");
			}
			if (midAct != null && Widgets.ButtonText(new Rect(rect.x + rect.width / 2f - Page.BottomButSize.x / 2f, y, Page.BottomButSize.x, Page.BottomButSize.y), midLabel, true, true, true))
			{
				midAct();
			}
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x001EACAD File Offset: 0x001E8EAD
		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		// Token: 0x06005A6A RID: 23146 RVA: 0x001EACC7 File Offset: 0x001E8EC7
		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

		// Token: 0x06005A6B RID: 23147 RVA: 0x001EACE4 File Offset: 0x001E8EE4
		protected virtual void DoNext()
		{
			if (this.next != null)
			{
				Find.WindowStack.Add(this.next);
			}
			if (this.nextAct != null)
			{
				this.nextAct();
			}
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToNextPage");
			this.Close(true);
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x001EAD41 File Offset: 0x001E8F41
		protected virtual void DoBack()
		{
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToPrevPage");
			if (this.prev != null)
			{
				Find.WindowStack.Add(this.prev);
			}
			this.Close(true);
		}

		// Token: 0x06005A6D RID: 23149 RVA: 0x001EAD80 File Offset: 0x001E8F80
		public override void OnCancelKeyPressed()
		{
			if (!this.closeOnCancel)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoBack())
			{
				this.DoBack();
			}
			else
			{
				this.Close(true);
			}
			Event.current.Use();
			base.OnCancelKeyPressed();
		}

		// Token: 0x06005A6E RID: 23150 RVA: 0x001EADD1 File Offset: 0x001E8FD1
		public override void OnAcceptKeyPressed()
		{
			if (!this.closeOnAccept)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoNext())
			{
				this.DoNext();
			}
			Event.current.Use();
		}

		// Token: 0x04003137 RID: 12599
		public Page prev;

		// Token: 0x04003138 RID: 12600
		public Page next;

		// Token: 0x04003139 RID: 12601
		public Action nextAct;

		// Token: 0x0400313A RID: 12602
		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		// Token: 0x0400313B RID: 12603
		public const float TitleAreaHeight = 45f;

		// Token: 0x0400313C RID: 12604
		public const float BottomButHeight = 38f;

		// Token: 0x0400313D RID: 12605
		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);
	}
}
