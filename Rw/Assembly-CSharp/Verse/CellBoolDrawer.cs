using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000150 RID: 336
	public class CellBoolDrawer
	{
		// Token: 0x06000981 RID: 2433 RVA: 0x0003399C File Offset: 0x00031B9C
		private CellBoolDrawer(int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000339EC File Offset: 0x00031BEC
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = (() => giver.Color);
			this.extraColorGetter = new Func<int, Color>(giver.GetCellExtraColor);
			this.cellBoolGetter = new Func<int, bool>(giver.GetCellBool);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00033A52 File Offset: 0x00031C52
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(giver, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00033A67 File Offset: 0x00031C67
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = colorGetter;
			this.extraColorGetter = extraColorGetter;
			this.cellBoolGetter = cellBoolGetter;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00033A8A File Offset: 0x00031C8A
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(cellBoolGetter, colorGetter, extraColorGetter, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00033AA3 File Offset: 0x00031CA3
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00033AAC File Offset: 0x00031CAC
		public void CellBoolDrawerUpdate()
		{
			if (this.wantDraw)
			{
				this.ActuallyDraw();
				this.wantDraw = false;
			}
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00033AC4 File Offset: 0x00031CC4
		private void ActuallyDraw()
		{
			if (this.dirty)
			{
				this.RegenerateMesh();
			}
			for (int i = 0; i < this.meshes.Count; i++)
			{
				Graphics.DrawMesh(this.meshes[i], Vector3.zero, Quaternion.identity, this.material, 0);
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00033B17 File Offset: 0x00031D17
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00033B20 File Offset: 0x00031D20
		public void RegenerateMesh()
		{
			for (int i = 0; i < this.meshes.Count; i++)
			{
				this.meshes[i].Clear();
			}
			int num = 0;
			int num2 = 0;
			if (this.meshes.Count < num + 1)
			{
				Mesh mesh = new Mesh();
				mesh.name = "CellBoolDrawer";
				this.meshes.Add(mesh);
			}
			Mesh mesh2 = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			bool careAboutVertexColors = false;
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int arg = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					if (this.cellBoolGetter(arg))
					{
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color color = this.extraColorGetter(arg);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						if (color != Color.white)
						{
							careAboutVertexColors = true;
						}
						int count = CellBoolDrawer.verts.Count;
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 3);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh2);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh mesh3 = new Mesh();
								mesh3.name = "CellBoolDrawer";
								this.meshes.Add(mesh3);
							}
							mesh2 = this.meshes[num];
							num2 = 0;
						}
					}
				}
			}
			this.FinalizeWorkingDataIntoMesh(mesh2);
			this.CreateMaterialIfNeeded(careAboutVertexColors);
			this.dirty = false;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00033DA0 File Offset: 0x00031FA0
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellBoolDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellBoolDrawer.verts);
				CellBoolDrawer.verts.Clear();
				mesh.SetTriangles(CellBoolDrawer.tris, 0);
				CellBoolDrawer.tris.Clear();
				mesh.SetColors(CellBoolDrawer.colors);
				CellBoolDrawer.colors.Clear();
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00033DFC File Offset: 0x00031FFC
		private void CreateMaterialIfNeeded(bool careAboutVertexColors)
		{
			if (this.material == null || this.materialCaresAboutVertexColors != careAboutVertexColors)
			{
				Color color = this.colorGetter();
				this.material = SolidColorMaterials.SimpleSolidColorMaterial(new Color(color.r, color.g, color.b, this.opacity * color.a), careAboutVertexColors);
				this.materialCaresAboutVertexColors = careAboutVertexColors;
				this.material.renderQueue = this.renderQueue;
			}
		}

		// Token: 0x040007CB RID: 1995
		private bool wantDraw;

		// Token: 0x040007CC RID: 1996
		private Material material;

		// Token: 0x040007CD RID: 1997
		private bool materialCaresAboutVertexColors;

		// Token: 0x040007CE RID: 1998
		private bool dirty = true;

		// Token: 0x040007CF RID: 1999
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x040007D0 RID: 2000
		private int mapSizeX;

		// Token: 0x040007D1 RID: 2001
		private int mapSizeZ;

		// Token: 0x040007D2 RID: 2002
		private float opacity = 0.33f;

		// Token: 0x040007D3 RID: 2003
		private int renderQueue = 3600;

		// Token: 0x040007D4 RID: 2004
		private Func<Color> colorGetter;

		// Token: 0x040007D5 RID: 2005
		private Func<int, Color> extraColorGetter;

		// Token: 0x040007D6 RID: 2006
		private Func<int, bool> cellBoolGetter;

		// Token: 0x040007D7 RID: 2007
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x040007D8 RID: 2008
		private static List<int> tris = new List<int>();

		// Token: 0x040007D9 RID: 2009
		private static List<Color> colors = new List<Color>();

		// Token: 0x040007DA RID: 2010
		private const float DefaultOpacity = 0.33f;

		// Token: 0x040007DB RID: 2011
		private const int MaxCellsPerMesh = 16383;
	}
}
