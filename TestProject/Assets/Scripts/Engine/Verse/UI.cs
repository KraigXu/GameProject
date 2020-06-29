using System;
using UnityEngine;

namespace Verse
{
	
	public static class UI
	{
		
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000ECE0 File Offset: 0x0000CEE0
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000ECF8 File Offset: 0x0000CEF8
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000ED20 File Offset: 0x0000CF20
		public static Vector2 MousePosUIInvertedUseEventIfCan
		{
			get
			{
				if (Event.current != null)
				{
					return UI.GUIToScreenPoint(Event.current.mousePosition);
				}
				return UI.MousePositionOnUIInverted;
			}
		}

		
		public static void ApplyUIScale()
		{
			if (Prefs.UIScale == 1f)
			{
				UI.screenWidth = Screen.width;
				UI.screenHeight = Screen.height;
				return;
			}
			UI.screenWidth = Mathf.RoundToInt((float)Screen.width / Prefs.UIScale);
			UI.screenHeight = Mathf.RoundToInt((float)Screen.height / Prefs.UIScale);
			float uiscale = Prefs.UIScale;
			float uiscale2 = Prefs.UIScale;
			GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(uiscale, uiscale2, 1f));
		}

		
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		
		public static float CurUICellSize()
		{
			return (new Vector3(1f, 0f, 0f).MapToUIPosition() - new Vector3(0f, 0f, 0f).MapToUIPosition()).x;
		}

		
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		
		public static int screenWidth;

		
		public static int screenHeight;
	}
}
