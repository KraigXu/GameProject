using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E1 RID: 4577
	public class DebugTile
	{
		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x060069F0 RID: 27120 RVA: 0x0024FB90 File Offset: 0x0024DD90
		private Vector2 ScreenPos
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(Find.WorldGrid.GetTileCenter(this.tile));
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x060069F1 RID: 27121 RVA: 0x0024FBA8 File Offset: 0x0024DDA8
		private bool VisibleForCamera
		{
			get
			{
				return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(this.ScreenPos);
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x060069F2 RID: 27122 RVA: 0x0024FBE0 File Offset: 0x0024DDE0
		public float DistanceToCamera
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return Vector3.Distance(Find.WorldCamera.transform.position, tileCenter);
			}
		}

		// Token: 0x060069F3 RID: 27123 RVA: 0x0024FC14 File Offset: 0x0024DE14
		public void Draw()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			if (this.mesh == null)
			{
				Find.WorldGrid.GetTileVertices(this.tile, DebugTile.tmpVerts);
				for (int i = 0; i < DebugTile.tmpVerts.Count; i++)
				{
					Vector3 a = DebugTile.tmpVerts[i];
					DebugTile.tmpVerts[i] = a + a.normalized * 0.012f;
				}
				this.mesh = new Mesh();
				this.mesh.name = "DebugTile";
				this.mesh.SetVertices(DebugTile.tmpVerts);
				DebugTile.tmpIndices.Clear();
				for (int j = 0; j < DebugTile.tmpVerts.Count - 2; j++)
				{
					DebugTile.tmpIndices.Add(j + 2);
					DebugTile.tmpIndices.Add(j + 1);
					DebugTile.tmpIndices.Add(0);
				}
				this.mesh.SetTriangles(DebugTile.tmpIndices, 0);
			}
			Material material;
			if (this.customMat != null)
			{
				material = this.customMat;
			}
			else
			{
				material = WorldDebugMatsSpectrum.Mat(Mathf.RoundToInt(this.colorPct * 100f) % 100);
			}
			Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x060069F4 RID: 27124 RVA: 0x0024FD64 File Offset: 0x0024DF64
		public void OnGUI()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			Vector2 screenPos = this.ScreenPos;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 20f, 40f, 40f);
			if (this.displayString != null)
			{
				Widgets.Label(rect, this.displayString);
			}
		}

		// Token: 0x04004202 RID: 16898
		public int tile;

		// Token: 0x04004203 RID: 16899
		public string displayString;

		// Token: 0x04004204 RID: 16900
		public float colorPct;

		// Token: 0x04004205 RID: 16901
		public int ticksLeft;

		// Token: 0x04004206 RID: 16902
		public Material customMat;

		// Token: 0x04004207 RID: 16903
		private Mesh mesh;

		// Token: 0x04004208 RID: 16904
		private static List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x04004209 RID: 16905
		private static List<int> tmpIndices = new List<int>();
	}
}
