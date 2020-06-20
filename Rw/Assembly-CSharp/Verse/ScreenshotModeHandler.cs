using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B4 RID: 948
	public class ScreenshotModeHandler
	{
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x000AA8F7 File Offset: 0x000A8AF7
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x000AA900 File Offset: 0x000A8B00
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000AA95C File Offset: 0x000A8B5C
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		// Token: 0x0400107D RID: 4221
		private bool active;
	}
}
