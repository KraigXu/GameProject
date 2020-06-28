using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000390 RID: 912
	public class Command_Toggle : Command
	{
		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x000A55CA File Offset: 0x000A37CA
		public override SoundDef CurActivateSound
		{
			get
			{
				if (this.isActive())
				{
					return this.turnOffSound;
				}
				return this.turnOnSound;
			}
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000A55E6 File Offset: 0x000A37E6
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000A55FC File Offset: 0x000A37FC
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = this.isActive() ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000A567C File Offset: 0x000A387C
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x04000FE9 RID: 4073
		public Func<bool> isActive;

		// Token: 0x04000FEA RID: 4074
		public Action toggleAction;

		// Token: 0x04000FEB RID: 4075
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x04000FEC RID: 4076
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x04000FED RID: 4077
		public bool activateIfAmbiguous = true;
	}
}
