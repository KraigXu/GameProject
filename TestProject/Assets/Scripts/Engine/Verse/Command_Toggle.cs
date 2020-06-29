﻿using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class Command_Toggle : Command
	{
		
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

		
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = this.isActive() ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		
		public Func<bool> isActive;

		
		public Action toggleAction;

		
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		
		public bool activateIfAmbiguous = true;
	}
}
