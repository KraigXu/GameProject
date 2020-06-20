using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200129D RID: 4765
	public static class WorldObjectSelectionUtility
	{
		// Token: 0x06007052 RID: 28754 RVA: 0x00272B09 File Offset: 0x00270D09
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < allObjects.Count; i = num + 1)
			{
				if (!allObjects[i].NeverMultiSelect && !allObjects[i].HiddenBehindTerrainNow())
				{
					if (ExpandableWorldObjectsUtility.IsExpanded(allObjects[i]))
					{
						if (rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(allObjects[i])))
						{
							yield return allObjects[i];
						}
					}
					else if (rect.Contains(allObjects[i].ScreenPos()))
					{
						yield return allObjects[i];
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06007053 RID: 28755 RVA: 0x00272B19 File Offset: 0x00270D19
		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos);
		}

		// Token: 0x06007054 RID: 28756 RVA: 0x00272B26 File Offset: 0x00270D26
		public static Vector2 ScreenPos(this WorldObject o)
		{
			return GenWorldUI.WorldToUIPosition(o.DrawPos);
		}

		// Token: 0x06007055 RID: 28757 RVA: 0x00272B34 File Offset: 0x00270D34
		public static bool VisibleToCameraNow(this WorldObject o)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return false;
			}
			if (o.HiddenBehindTerrainNow())
			{
				return false;
			}
			Vector2 point = o.ScreenPos();
			return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(point);
		}

		// Token: 0x06007056 RID: 28758 RVA: 0x00272B80 File Offset: 0x00270D80
		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask))
			{
				return Vector3.Distance(raycastHit.point, o.DrawPos);
			}
			return Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
		}
	}
}
