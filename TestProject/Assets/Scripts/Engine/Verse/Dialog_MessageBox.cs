using System;
using UnityEngine;

namespace Verse
{
	
	public class Dialog_MessageBox : Window
	{
		
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x000B516E File Offset: 0x000B336E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 460f);
			}
		}

		
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x000B517F File Offset: 0x000B337F
		private float TimeUntilInteractive
		{
			get
			{
				return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
			}
		}

		
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x000B5194 File Offset: 0x000B3394
		private bool InteractionDelayExpired
		{
			get
			{
				return this.TimeUntilInteractive <= 0f;
			}
		}

		
		public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, bool destructive = false, string title = null)
		{
			return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive, confirmedAct, delegate
			{
			});
		}

		
		public Dialog_MessageBox(TaggedString text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null, Action cancelAction = null)
		{
			this.text = text;
			this.buttonAText = buttonAText;
			this.buttonAAction = buttonAAction;
			this.buttonADestructive = buttonADestructive;
			this.buttonBText = buttonBText;
			this.buttonBAction = buttonBAction;
			this.title = title;
			this.acceptAction = acceptAction;
			this.cancelAction = cancelAction;
			if (buttonAText.NullOrEmpty())
			{
				this.buttonAText = "OK".Translate();
			}
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.creationRealTime = RealTime.LastRealTime;
			this.onlyOneOfTypeAllowed = false;
			bool flag = buttonAAction == null && buttonBAction == null && this.buttonCAction == null;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = (acceptAction != null || cancelAction != null || flag);
			this.closeOnAccept = flag;
			this.closeOnCancel = flag;
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, num, inRect.width, 42f), this.title);
				num += 42f;
			}
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect.x, num, inRect.width, inRect.height - 35f - 5f - num);
			float width = outRect.width - 16f;
			Rect viewRect = new Rect(0f, 0f, width, Text.CalcHeight(this.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			Widgets.Label(new Rect(0f, 0f, viewRect.width, viewRect.height), this.text);
			Widgets.EndScrollView();
			int num2 = this.buttonCText.NullOrEmpty() ? 2 : 3;
			float num3 = inRect.width / (float)num2;
			float width2 = num3 - 10f;
			if (this.buttonADestructive)
			{
				GUI.color = new Color(1f, 0.3f, 0.35f);
			}
			string label = this.InteractionDelayExpired ? this.buttonAText : (this.buttonAText + "(" + Mathf.Ceil(this.TimeUntilInteractive).ToString("F0") + ")");
			if (Widgets.ButtonText(new Rect(num3 * (float)(num2 - 1) + 10f, inRect.height - 35f, width2, 35f), label, true, true, true) && this.InteractionDelayExpired)
			{
				if (this.buttonAAction != null)
				{
					this.buttonAAction();
				}
				this.Close(true);
			}
			GUI.color = Color.white;
			if (this.buttonBText != null && Widgets.ButtonText(new Rect(0f, inRect.height - 35f, width2, 35f), this.buttonBText, true, true, true))
			{
				if (this.buttonBAction != null)
				{
					this.buttonBAction();
				}
				this.Close(true);
			}
			if (this.buttonCText != null && Widgets.ButtonText(new Rect(num3, inRect.height - 35f, width2, 35f), this.buttonCText, true, true, true))
			{
				if (this.buttonCAction != null)
				{
					this.buttonCAction();
				}
				if (this.buttonCClose)
				{
					this.Close(true);
				}
			}
		}

		
		public override void OnCancelKeyPressed()
		{
			if (this.cancelAction != null)
			{
				this.cancelAction();
				this.Close(true);
				return;
			}
			base.OnCancelKeyPressed();
		}

		
		public override void OnAcceptKeyPressed()
		{
			if (this.acceptAction != null)
			{
				this.acceptAction();
				this.Close(true);
				return;
			}
			base.OnAcceptKeyPressed();
		}

		
		public TaggedString text;

		
		public string title;

		
		public string buttonAText;

		
		public Action buttonAAction;

		
		public bool buttonADestructive;

		
		public string buttonBText;

		
		public Action buttonBAction;

		
		public string buttonCText;

		
		public Action buttonCAction;

		
		public bool buttonCClose = true;

		
		public float interactionDelay;

		
		public Action acceptAction;

		
		public Action cancelAction;

		
		private Vector2 scrollPosition = Vector2.zero;

		
		private float creationRealTime = -1f;

		
		private const float TitleHeight = 42f;

		
		private const float ButtonHeight = 35f;
	}
}
