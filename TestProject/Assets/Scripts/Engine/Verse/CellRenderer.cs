using System;
using UnityEngine;

namespace Verse
{
	
	public static class CellRenderer
	{
		
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			return DebugMatsSpectrum.Mat(GenMath.PositiveMod(Mathf.RoundToInt(colorPct * 100f), 100), transparent);
		}

		
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		
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

		
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		
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

		
		private static int lastCameraUpdateFrame = -1;

		
		private static CellRect viewRect;
	}
}
