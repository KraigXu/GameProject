using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D2 RID: 978
	public static class UnityGUIBugsFixer
	{
		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x000AF1DC File Offset: 0x000AD3DC
		public static List<Resolution> ScreenResolutionsWithoutDuplicates
		{
			get
			{
				UnityGUIBugsFixer.resolutions.Clear();
				Resolution[] array = Screen.resolutions;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < UnityGUIBugsFixer.resolutions.Count; j++)
					{
						if (UnityGUIBugsFixer.resolutions[j].width == array[i].width && UnityGUIBugsFixer.resolutions[j].height == array[i].height)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						UnityGUIBugsFixer.resolutions.Add(array[i]);
					}
				}
				return UnityGUIBugsFixer.resolutions;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x000AF27F File Offset: 0x000AD47F
		public static Vector2 CurrentEventDelta
		{
			get
			{
				return UnityGUIBugsFixer.currentEventDelta;
			}
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x000AF286 File Offset: 0x000AD486
		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
			UnityGUIBugsFixer.FixDelta();
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x000AF298 File Offset: 0x000AD498
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * 6f);
			}
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000AF2F0 File Offset: 0x000AD4F0
		private static void FixShift()
		{
			if ((Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) && !Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000AF33E File Offset: 0x000AD53E
		public static bool ResolutionsEqual(IntVec2 a, IntVec2 b)
		{
			return a == b;
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000AF348 File Offset: 0x000AD548
		private static void FixDelta()
		{
			Vector2 vector = UI.GUIToScreenPoint(Event.current.mousePosition);
			if (Event.current.rawType == EventType.MouseDrag)
			{
				if (vector != UnityGUIBugsFixer.lastMousePosition || Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = vector - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(vector);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else
			{
				UnityGUIBugsFixer.currentEventDelta = Event.current.delta;
				if (Event.current.rawType == EventType.MouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(vector);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.rawType == EventType.MouseUp)
				{
					UnityGUIBugsFixer.lastMousePosition = null;
				}
			}
		}

		// Token: 0x04001159 RID: 4441
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x0400115A RID: 4442
		private static Vector2 currentEventDelta;

		// Token: 0x0400115B RID: 4443
		private static int lastMousePositionFrame;

		// Token: 0x0400115C RID: 4444
		private const float ScrollFactor = 6f;

		// Token: 0x0400115D RID: 4445
		private static Vector2? lastMousePosition;
	}
}
