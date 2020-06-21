using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E5 RID: 4581
	[StaticConstructorOnStartup]
	public class WorldLayer
	{
		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x06006A02 RID: 27138 RVA: 0x002500DF File Offset: 0x0024E2DF
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06006A03 RID: 27139 RVA: 0x002500E7 File Offset: 0x0024E2E7
		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x002500EE File Offset: 0x0024E2EE
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x06006A05 RID: 27141 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x06006A06 RID: 27142 RVA: 0x002500DF File Offset: 0x0024E2DF
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x06006A07 RID: 27143 RVA: 0x002500F8 File Offset: 0x0024E2F8
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x06006A08 RID: 27144 RVA: 0x00250110 File Offset: 0x0024E310
		protected LayerSubMesh GetSubMesh(Material material, out int subMeshIndex)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.material == material && layerSubMesh.verts.Count < 40000)
				{
					subMeshIndex = i;
					return layerSubMesh;
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "WorldLayerSubMesh_" + base.GetType().Name + "_" + Find.World.info.seedString;
			}
			LayerSubMesh layerSubMesh2 = new LayerSubMesh(mesh, material);
			subMeshIndex = this.subMeshes.Count;
			this.subMeshes.Add(layerSubMesh2);
			return layerSubMesh2;
		}

		// Token: 0x06006A09 RID: 27145 RVA: 0x002501C4 File Offset: 0x0024E3C4
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

		// Token: 0x06006A0A RID: 27146 RVA: 0x00250212 File Offset: 0x0024E412
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x06006A0B RID: 27147 RVA: 0x00250228 File Offset: 0x0024E428
		public void Render()
		{
			if (this.ShouldRegenerate)
			{
				this.RegenerateNow();
			}
			int layer = this.Layer;
			Quaternion rotation = this.Rotation;
			float alpha = this.Alpha;
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].finalized)
				{
					if (alpha != 1f)
					{
						Color color = this.subMeshes[i].material.color;
						WorldLayer.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * alpha));
						Graphics.DrawMesh(this.subMeshes[i].mesh, Vector3.zero, rotation, this.subMeshes[i].material, layer, null, 0, WorldLayer.propertyBlock);
					}
					else
					{
						Graphics.DrawMesh(this.subMeshes[i].mesh, Vector3.zero, rotation, this.subMeshes[i].material, layer);
					}
				}
			}
		}

		// Token: 0x06006A0C RID: 27148 RVA: 0x00250344 File Offset: 0x0024E544
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		// Token: 0x06006A0D RID: 27149 RVA: 0x00250354 File Offset: 0x0024E554
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06006A0E RID: 27150 RVA: 0x00250360 File Offset: 0x0024E560
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}

		// Token: 0x04004214 RID: 16916
		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x04004215 RID: 16917
		private bool dirty = true;

		// Token: 0x04004216 RID: 16918
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04004217 RID: 16919
		private const int MaxVerticesPerMesh = 40000;
	}
}
