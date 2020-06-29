using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public abstract class Dialog_Rename : Window
	{
		
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x000B4147 File Offset: 0x000B2347
		private bool AcceptsInput
		{
			get
			{
				return this.startAcceptingInputAtFrame <= Time.frameCount;
			}
		}

		
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x000B4159 File Offset: 0x000B2359
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		
		// (get) Token: 0x06001D4A RID: 7498 RVA: 0x000B415D File Offset: 0x000B235D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		
		public void WasOpenedByHotkey()
		{
			this.startAcceptingInputAtFrame = Time.frameCount + 1;
		}

		
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			if (name.Length == 0)
			{
				return false;
			}
			return true;
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			GUI.SetNextControlName("RenameField");
			string text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), this.curName);
			if (this.AcceptsInput && text.Length < this.MaxNameLength)
			{
				this.curName = text;
			}
			else if (!this.AcceptsInput)
			{
				((TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl)).SelectAll();
			}
			if (!this.focusedRenameField)
			{
				UI.FocusControl("RenameField", this);
				this.focusedRenameField = true;
			}
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 15f, inRect.width - 15f - 15f, 35f), "OK", true, true, true) || flag)
			{
				AcceptanceReport acceptanceReport = this.NameIsValid(this.curName);
				if (!acceptanceReport.Accepted)
				{
					if (acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message("NameIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					return;
				}
				else
				{
					this.SetName(this.curName);
					Find.WindowStack.TryRemove(this, true);
				}
			}
		}

		
		protected abstract void SetName(string name);

		
		protected string curName;

		
		private bool focusedRenameField;

		
		private int startAcceptingInputAtFrame;
	}
}
