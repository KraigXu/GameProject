using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002A RID: 42
	public static class UI
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000ECE0 File Offset: 0x0000CEE0
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x17000076 RID: 118
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

		// Token: 0x17000077 RID: 119
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

		// Token: 0x060002D5 RID: 725 RVA: 0x0000ED40 File Offset: 0x0000CF40
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

		// Token: 0x060002D6 RID: 726 RVA: 0x0000EDD5 File Offset: 0x0000CFD5
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000EE02 File Offset: 0x0000D002
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000EE18 File Offset: 0x0000D018
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000EE53 File Offset: 0x0000D053
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000EE64 File Offset: 0x0000D064
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000EEAE File Offset: 0x0000D0AE
		public static float CurUICellSize()
		{
			return (new Vector3(1f, 0f, 0f).MapToUIPosition() - new Vector3(0f, 0f, 0f).MapToUIPosition()).x;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000EEEC File Offset: 0x0000D0EC
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000EEF8 File Offset: 0x0000D0F8
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		// Token: 0x0400007A RID: 122
		public static int screenWidth;

		// Token: 0x0400007B RID: 123
		public static int screenHeight;
	}
}
