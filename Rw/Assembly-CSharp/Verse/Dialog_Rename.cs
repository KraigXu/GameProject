using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D9 RID: 985
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x000B4147 File Offset: 0x000B2347
		private bool AcceptsInput
		{
			get
			{
				return this.startAcceptingInputAtFrame <= Time.frameCount;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x000B4159 File Offset: 0x000B2359
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001D4A RID: 7498 RVA: 0x000B415D File Offset: 0x000B235D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x000B416E File Offset: 0x000B236E
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x000B4199 File Offset: 0x000B2399
		public void WasOpenedByHotkey()
		{
			this.startAcceptingInputAtFrame = Time.frameCount + 1;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000B41A8 File Offset: 0x000B23A8
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			if (name.Length == 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000B41C0 File Offset: 0x000B23C0
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

		// Token: 0x06001D4F RID: 7503
		protected abstract void SetName(string name);

		// Token: 0x040011C3 RID: 4547
		protected string curName;

		// Token: 0x040011C4 RID: 4548
		private bool focusedRenameField;

		// Token: 0x040011C5 RID: 4549
		private int startAcceptingInputAtFrame;
	}
}
