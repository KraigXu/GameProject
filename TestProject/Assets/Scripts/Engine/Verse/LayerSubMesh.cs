using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000187 RID: 391
	public class LayerSubMesh
	{
		// Token: 0x06000B4E RID: 2894 RVA: 0x0003CF84 File Offset: 0x0003B184
		public LayerSubMesh(Mesh mesh, Material material)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0003CFD4 File Offset: 0x0003B1D4
		public void Clear(MeshParts parts)
		{
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				this.verts.Clear();
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				this.tris.Clear();
			}
			if ((parts & MeshParts.Colors) != MeshParts.None)
			{
				this.colors.Clear();
			}
			if ((parts & MeshParts.UVs) != MeshParts.None)
			{
				this.uvs.Clear();
			}
			this.finalized = false;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0003D028 File Offset: 0x0003B228
		public void FinalizeMesh(MeshParts parts)
		{
			if (this.finalized)
			{
				Log.Warning("Finalizing mesh which is already finalized. Did you forget to call Clear()?", false);
			}
			if ((parts & MeshParts.Verts) != MeshParts.None || (parts & MeshParts.Tris) != MeshParts.None)
			{
				this.mesh.Clear();
			}
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				if (this.verts.Count > 0)
				{
					this.mesh.SetVertices(this.verts);
				}
				else
				{
					Log.Error("Cannot cook Verts for " + this.material.ToString() + ": no ingredients data. If you want to not render this submesh, disable it.", false);
				}
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				if (this.tris.Count > 0)
				{
					this.mesh.SetTriangles(this.tris, 0);
				}
				else
				{
					Log.Error("Cannot cook Tris for " + this.material.ToString() + ": no ingredients data.", false);
				}
			}
			if ((parts & MeshParts.Colors) != MeshParts.None && this.colors.Count > 0)
			{
				this.mesh.SetColors(this.colors);
			}
			if ((parts & MeshParts.UVs) != MeshParts.None && this.uvs.Count > 0)
			{
				this.mesh.SetUVs(0, this.uvs);
			}
			this.finalized = true;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0003D13A File Offset: 0x0003B33A
		public override string ToString()
		{
			return "LayerSubMesh(" + this.material.ToString() + ")";
		}

		// Token: 0x04000912 RID: 2322
		public bool finalized;

		// Token: 0x04000913 RID: 2323
		public bool disabled;

		// Token: 0x04000914 RID: 2324
		public Material material;

		// Token: 0x04000915 RID: 2325
		public Mesh mesh;

		// Token: 0x04000916 RID: 2326
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000917 RID: 2327
		public List<int> tris = new List<int>();

		// Token: 0x04000918 RID: 2328
		public List<Color32> colors = new List<Color32>();

		// Token: 0x04000919 RID: 2329
		public List<Vector3> uvs = new List<Vector3>();
	}
}
