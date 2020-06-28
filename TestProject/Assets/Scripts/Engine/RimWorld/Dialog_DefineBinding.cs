using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E55 RID: 3669
	public class Dialog_DefineBinding : Window
	{
		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x060058CF RID: 22735 RVA: 0x001D915B File Offset: 0x001D735B
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowSize;
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x060058D0 RID: 22736 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060058D1 RID: 22737 RVA: 0x001D9164 File Offset: 0x001D7364
		public Dialog_DefineBinding(KeyPrefsData keyPrefsData, KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			this.keyDef = keyDef;
			this.slot = slot;
			this.keyPrefsData = keyPrefsData;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forcePause = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060058D2 RID: 22738 RVA: 0x001D91C4 File Offset: 0x001D73C4
		public override void DoWindowContents(Rect inRect)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(inRect, "PressAnyKeyOrEsc".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None)
			{
				if (Event.current.keyCode != KeyCode.Escape)
				{
					this.keyPrefsData.EraseConflictingBindingsForKeyCode(this.keyDef, Event.current.keyCode, delegate(KeyBindingDef oldDef)
					{
						Messages.Message("KeyBindingOverwritten".Translate(oldDef.LabelCap), MessageTypeDefOf.TaskCompletion, false);
					});
					this.keyPrefsData.SetBinding(this.keyDef, this.slot, Event.current.keyCode);
				}
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x04003002 RID: 12290
		protected Vector2 windowSize = new Vector2(400f, 200f);

		// Token: 0x04003003 RID: 12291
		protected KeyPrefsData keyPrefsData;

		// Token: 0x04003004 RID: 12292
		protected KeyBindingDef keyDef;

		// Token: 0x04003005 RID: 12293
		protected KeyPrefs.BindingSlot slot;
	}
}
