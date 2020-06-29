using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class Page : Window
	{
		
		// (get) Token: 0x06005A63 RID: 23139 RVA: 0x001EAAA1 File Offset: 0x001E8CA1
		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		
		public Page()
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
		}

		
		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		
		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = 45f + extraTopSpace;
			}
			return new Rect(0f, num, rect.width, rect.height - 38f - num - 17f);
		}

		
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

		
		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		
		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

		
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

		
		public Page prev;

		
		public Page next;

		
		public Action nextAct;

		
		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		
		public const float TitleAreaHeight = 45f;

		
		public const float BottomButHeight = 38f;

		
		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);
	}
}
