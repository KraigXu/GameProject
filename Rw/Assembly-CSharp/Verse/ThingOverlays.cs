using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B8 RID: 952
	public class ThingOverlays
	{
		// Token: 0x06001C1B RID: 7195 RVA: 0x000AB050 File Offset: 0x000A9250
		public void ThingOverlaysOnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.HasGUIOverlay);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (currentViewRect.Contains(thing.Position) && !Find.CurrentMap.fogGrid.IsFogged(thing.Position))
				{
					try
					{
						thing.DrawGUIOverlay();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception drawing ThingOverlay for ",
							thing,
							": ",
							ex
						}), false);
					}
				}
			}
		}
	}
}
