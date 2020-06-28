using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200015A RID: 346
	public static class CellRenderer
	{
		// Token: 0x060009AB RID: 2475 RVA: 0x00034A3C File Offset: 0x00032C3C
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00034A63 File Offset: 0x00032C63
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			return DebugMatsSpectrum.Mat(GenMath.PositiveMod(Mathf.RoundToInt(colorPct * 100f), 100), transparent);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00034A7E File Offset: 0x00032C7E
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00034A90 File Offset: 0x00032C90
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(c))
			{
				return;
			}
			Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00034ACC File Offset: 0x00032CCC
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00034AE0 File Offset: 0x00032CE0
		public static void RenderSpot(Vector3 loc, Material mat, float scale = 0.15f)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(loc.ToIntVec3()))
			{
				return;
			}
			loc.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 s = new Vector3(scale, 1f, scale);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(loc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, mat, 0);
		}

		// Token: 0x040007F5 RID: 2037
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x040007F6 RID: 2038
		private static CellRect viewRect;
	}
}
