using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200129F RID: 4767
	public static class WorldSelectionDrawer
	{
		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x0600706F RID: 28783 RVA: 0x00273955 File Offset: 0x00271B55
		public static Dictionary<WorldObject, float> SelectTimes
		{
			get
			{
				return WorldSelectionDrawer.selectTimes;
			}
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x0027395C File Offset: 0x00271B5C
		public static void Notify_Selected(WorldObject t)
		{
			WorldSelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x0027396E File Offset: 0x00271B6E
		public static void Clear()
		{
			WorldSelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x06007072 RID: 28786 RVA: 0x0027397C File Offset: 0x00271B7C
		public static void SelectionOverlaysOnGUI()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				WorldObject worldObject = selectedObjects[i];
				WorldSelectionDrawer.DrawSelectionBracketOnGUIFor(worldObject);
				worldObject.ExtraSelectionOverlaysOnGUI();
			}
		}

		// Token: 0x06007073 RID: 28787 RVA: 0x002739B8 File Offset: 0x00271BB8
		public static void DrawSelectionOverlays()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				selectedObjects[i].DrawExtraSelectionOverlays();
			}
		}

		// Token: 0x06007074 RID: 28788 RVA: 0x002739F0 File Offset: 0x00271BF0
		private static void DrawSelectionBracketOnGUIFor(WorldObject obj)
		{
			Vector2 vector = obj.ScreenPos();
			Rect rect = new Rect(vector.x - 17.5f, vector.y - 17.5f, 35f, 35f);
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * 0.4f, (float)SelectionDrawerUtility.SelectedTexGUI.height * 0.4f);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(WorldSelectionDrawer.bracketLocs, obj, rect, WorldSelectionDrawer.selectTimes, textureSize, 25f);
			if (obj.HiddenBehindTerrainNow())
			{
				GUI.color = WorldSelectionDrawer.HiddenSelectionBracketColor;
			}
			else
			{
				GUI.color = Color.white;
			}
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(WorldSelectionDrawer.bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, 0.4f);
				num += 90;
			}
			GUI.color = Color.white;
		}

		// Token: 0x0400451D RID: 17693
		private static Dictionary<WorldObject, float> selectTimes = new Dictionary<WorldObject, float>();

		// Token: 0x0400451E RID: 17694
		private const float BaseSelectedTexJump = 25f;

		// Token: 0x0400451F RID: 17695
		private const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04004520 RID: 17696
		private const float BaseSelectionRectSize = 35f;

		// Token: 0x04004521 RID: 17697
		private static readonly Color HiddenSelectionBracketColor = new Color(1f, 1f, 1f, 0.35f);

		// Token: 0x04004522 RID: 17698
		private static Vector2[] bracketLocs = new Vector2[4];
	}
}
