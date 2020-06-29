using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class ScreenshotModeHandler
	{
		
		// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x000AA8F7 File Offset: 0x000A8AF7
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x000AA900 File Offset: 0x000A8B00
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		
		private bool active;
	}
}
