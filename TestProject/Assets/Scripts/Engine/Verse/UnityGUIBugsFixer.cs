using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public static class UnityGUIBugsFixer
	{
		
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

		
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x000AF27F File Offset: 0x000AD47F
		public static Vector2 CurrentEventDelta
		{
			get
			{
				return UnityGUIBugsFixer.currentEventDelta;
			}
		}

		
		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
			UnityGUIBugsFixer.FixDelta();
		}

		
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * 6f);
			}
		}

		
		private static void FixShift()
		{
			if ((Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) && !Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}

		
		public static bool ResolutionsEqual(IntVec2 a, IntVec2 b)
		{
			return a == b;
		}

		
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

		
		private static List<Resolution> resolutions = new List<Resolution>();

		
		private static Vector2 currentEventDelta;

		
		private static int lastMousePositionFrame;

		
		private const float ScrollFactor = 6f;

		
		private static Vector2? lastMousePosition;
	}
}
