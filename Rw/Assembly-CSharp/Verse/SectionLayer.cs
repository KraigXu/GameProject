using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018C RID: 396
	public abstract class SectionLayer
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0003DACA File Offset: 0x0003BCCA
		protected Map Map
		{
			get
			{
				return this.section.map;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0003DAD7 File Offset: 0x0003BCD7
		public SectionLayer(Section section)
		{
			this.section = section;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0003DAF4 File Offset: 0x0003BCF4
		public LayerSubMesh GetSubMesh(Material material)
		{
			if (material == null)
			{
				return null;
			}
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].material == material)
				{
					return this.subMeshes[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = string.Concat(new object[]
				{
					"SectionLayerSubMesh_",
					base.GetType().Name,
					"_",
					this.Map.Tile
				});
			}
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material);
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0003DBAC File Offset: 0x0003BDAC
		protected void FinalizeMesh(MeshParts tags)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags);
				}
			}
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0003DBFC File Offset: 0x0003BDFC
		public virtual void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0);
				}
			}
		}

		// Token: 0x06000B6C RID: 2924
		public abstract void Regenerate();

		// Token: 0x06000B6D RID: 2925 RVA: 0x0003DC64 File Offset: 0x0003BE64
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}

		// Token: 0x04000930 RID: 2352
		protected Section section;

		// Token: 0x04000931 RID: 2353
		public MapMeshFlag relevantChangeTypes;

		// Token: 0x04000932 RID: 2354
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();
	}
}
