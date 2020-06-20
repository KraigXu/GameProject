using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC4 RID: 4036
	public static class GenWorld
	{
		// Token: 0x0600610B RID: 24843 RVA: 0x0021AE38 File Offset: 0x00219038
		public static int MouseTile(bool snapToExpandableWorldObjects = false)
		{
			if (snapToExpandableWorldObjects)
			{
				if (GenWorld.cachedFrame_snap == Time.frameCount)
				{
					return GenWorld.cachedTile_snap;
				}
				GenWorld.cachedTile_snap = GenWorld.TileAt(UI.MousePositionOnUI, true);
				GenWorld.cachedFrame_snap = Time.frameCount;
				return GenWorld.cachedTile_snap;
			}
			else
			{
				if (GenWorld.cachedFrame_noSnap == Time.frameCount)
				{
					return GenWorld.cachedTile_noSnap;
				}
				GenWorld.cachedTile_noSnap = GenWorld.TileAt(UI.MousePositionOnUI, false);
				GenWorld.cachedFrame_noSnap = Time.frameCount;
				return GenWorld.cachedTile_noSnap;
			}
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x0021AEAC File Offset: 0x002190AC
		public static int TileAt(Vector2 clickPos, bool snapToExpandableWorldObjects = false)
		{
			Camera worldCamera = Find.WorldCamera;
			if (!worldCamera.gameObject.activeInHierarchy)
			{
				return -1;
			}
			if (snapToExpandableWorldObjects)
			{
				ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(UI.MousePositionOnUI, GenWorld.tmpWorldObjectsUnderMouse);
				if (GenWorld.tmpWorldObjectsUnderMouse.Any<WorldObject>())
				{
					int tile = GenWorld.tmpWorldObjectsUnderMouse[0].Tile;
					GenWorld.tmpWorldObjectsUnderMouse.Clear();
					return tile;
				}
			}
			Ray ray = worldCamera.ScreenPointToRay(clickPos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1500f, worldLayerMask))
			{
				return Find.World.renderer.GetTileIDFromRayHit(hit);
			}
			return -1;
		}

		// Token: 0x04003B1A RID: 15130
		private static int cachedTile_noSnap = -1;

		// Token: 0x04003B1B RID: 15131
		private static int cachedFrame_noSnap = -1;

		// Token: 0x04003B1C RID: 15132
		private static int cachedTile_snap = -1;

		// Token: 0x04003B1D RID: 15133
		private static int cachedFrame_snap = -1;

		// Token: 0x04003B1E RID: 15134
		public const float MaxRayLength = 1500f;

		// Token: 0x04003B1F RID: 15135
		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();
	}
}
