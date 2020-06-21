using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011F6 RID: 4598
	public static class ExpandableWorldObjectsUtility
	{
		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x06006A55 RID: 27221 RVA: 0x002510BF File Offset: 0x0024F2BF
		public static float TransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.transitionPct;
			}
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x06006A56 RID: 27222 RVA: 0x002510D8 File Offset: 0x0024F2D8
		public static float ExpandMoreTransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.expandMoreTransitionPct;
			}
		}

		// Token: 0x06006A57 RID: 27223 RVA: 0x002510F4 File Offset: 0x0024F2F4
		public static void ExpandableWorldObjectsUpdate()
		{
			float num = Time.deltaTime * 3f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose)
			{
				ExpandableWorldObjectsUtility.transitionPct -= num;
			}
			else
			{
				ExpandableWorldObjectsUtility.transitionPct += num;
			}
			ExpandableWorldObjectsUtility.transitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.transitionPct);
			float num2 = Time.deltaTime * 4f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.Far)
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct -= num2;
			}
			else
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct += num2;
			}
			ExpandableWorldObjectsUtility.expandMoreTransitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.expandMoreTransitionPct);
		}

		// Token: 0x06006A58 RID: 27224 RVA: 0x00251188 File Offset: 0x0024F388
		public static void ExpandableWorldObjectsOnGUI()
		{
			if (ExpandableWorldObjectsUtility.TransitionPct == 0f)
			{
				return;
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			ExpandableWorldObjectsUtility.tmpWorldObjects.AddRange(Find.WorldObjects.AllWorldObjects);
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(ExpandableWorldObjectsUtility.tmpWorldObjects);
			WorldTargeter worldTargeter = Find.WorldTargeter;
			List<WorldObject> worldObjectsUnderMouse = null;
			if (worldTargeter.IsTargeting)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			for (int i = 0; i < ExpandableWorldObjectsUtility.tmpWorldObjects.Count; i++)
			{
				try
				{
					WorldObject worldObject = ExpandableWorldObjectsUtility.tmpWorldObjects[i];
					if (worldObject.def.expandingIcon)
					{
						if (!worldObject.HiddenBehindTerrainNow())
						{
							Color expandingIconColor = worldObject.ExpandingIconColor;
							expandingIconColor.a = ExpandableWorldObjectsUtility.TransitionPct;
							if (worldTargeter.IsTargetedNow(worldObject, worldObjectsUnderMouse))
							{
								float num = GenMath.LerpDouble(-1f, 1f, 0.7f, 1f, Mathf.Sin(Time.time * 8f));
								expandingIconColor.r *= num;
								expandingIconColor.g *= num;
								expandingIconColor.b *= num;
							}
							GUI.color = expandingIconColor;
							GUI.DrawTexture(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject), worldObject.ExpandingIcon);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while drawing ",
						ExpandableWorldObjectsUtility.tmpWorldObjects[i].ToStringSafe<WorldObject>(),
						": ",
						ex
					}), false);
				}
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			GUI.color = Color.white;
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x00251318 File Offset: 0x0024F518
		public static Rect ExpandedIconScreenRect(WorldObject o)
		{
			Vector2 vector = o.ScreenPos();
			float num;
			if (o.ExpandMore)
			{
				num = Mathf.Lerp(30f, 40.5f, ExpandableWorldObjectsUtility.ExpandMoreTransitionPct);
			}
			else
			{
				num = 30f;
			}
			return new Rect(vector.x - num / 2f, vector.y - num / 2f, num, num);
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x00251374 File Offset: 0x0024F574
		public static bool IsExpanded(WorldObject o)
		{
			return ExpandableWorldObjectsUtility.TransitionPct > 0.5f && o.def.expandingIcon;
		}

		// Token: 0x06006A5B RID: 27227 RVA: 0x00251390 File Offset: 0x0024F590
		public static void GetExpandedWorldObjectUnderMouse(Vector2 mousePos, List<WorldObject> outList)
		{
			outList.Clear();
			Vector2 vector = mousePos;
			vector.y = (float)UI.screenHeight - vector.y;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (ExpandableWorldObjectsUtility.IsExpanded(worldObject) && ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject).Contains(vector) && !worldObject.HiddenBehindTerrainNow())
				{
					outList.Add(worldObject);
				}
			}
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(outList);
			outList.Reverse();
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x00251414 File Offset: 0x0024F614
		private static void SortByExpandingIconPriority(List<WorldObject> worldObjects)
		{
			worldObjects.SortBy(delegate(WorldObject x)
			{
				float num = x.ExpandingIconPriority;
				if (x.Faction != null && x.Faction.IsPlayer)
				{
					num += 0.001f;
				}
				return num;
			}, (WorldObject x) => x.ID);
		}

		// Token: 0x0400423D RID: 16957
		private static float transitionPct;

		// Token: 0x0400423E RID: 16958
		private static float expandMoreTransitionPct;

		// Token: 0x0400423F RID: 16959
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		// Token: 0x04004240 RID: 16960
		private const float WorldObjectIconSize = 30f;

		// Token: 0x04004241 RID: 16961
		private const float ExpandMoreWorldObjectIconSizeFactor = 1.35f;

		// Token: 0x04004242 RID: 16962
		private const float TransitionSpeed = 3f;

		// Token: 0x04004243 RID: 16963
		private const float ExpandMoreTransitionSpeed = 4f;
	}
}
