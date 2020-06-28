using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000398 RID: 920
	public static class GUIEventFilterForOSX
	{
		// Token: 0x06001B09 RID: 6921 RVA: 0x000A6210 File Offset: 0x000A4410
		public static void CheckRejectGUIEvent()
		{
			if (UnityData.platform != RuntimePlatform.OSXPlayer)
			{
				return;
			}
			if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseUp)
			{
				return;
			}
			if (Time.frameCount != GUIEventFilterForOSX.lastRecordedFrame)
			{
				GUIEventFilterForOSX.eventsThisFrame.Clear();
				GUIEventFilterForOSX.lastRecordedFrame = Time.frameCount;
			}
			for (int i = 0; i < GUIEventFilterForOSX.eventsThisFrame.Count; i++)
			{
				if (GUIEventFilterForOSX.EventsAreEquivalent(GUIEventFilterForOSX.eventsThisFrame[i], Event.current))
				{
					GUIEventFilterForOSX.RejectEvent();
				}
			}
			GUIEventFilterForOSX.eventsThisFrame.Add(Event.current);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000A62A0 File Offset: 0x000A44A0
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000A62D0 File Offset: 0x000A44D0
		private static void RejectEvent()
		{
			if (DebugViewSettings.logInput)
			{
				Log.Message(string.Concat(new object[]
				{
					"Frame ",
					Time.frameCount,
					": REJECTED ",
					Event.current.ToStringFull()
				}), false);
			}
			Event.current.Use();
		}

		// Token: 0x04001016 RID: 4118
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x04001017 RID: 4119
		private static int lastRecordedFrame = -1;
	}
}
