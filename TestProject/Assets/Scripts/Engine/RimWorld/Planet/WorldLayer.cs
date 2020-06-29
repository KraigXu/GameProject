﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public class WorldLayer
	{
		
		// (get) Token: 0x06006A02 RID: 27138 RVA: 0x002500DF File Offset: 0x0024E2DF
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		
		// (get) Token: 0x06006A03 RID: 27139 RVA: 0x002500E7 File Offset: 0x0024E2E7
		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x002500EE File Offset: 0x0024E2EE
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		
		// (get) Token: 0x06006A05 RID: 27141 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		
		// (get) Token: 0x06006A06 RID: 27142 RVA: 0x002500DF File Offset: 0x0024E2DF
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		
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

		
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		
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

		
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		
		public void SetDirty()
		{
			this.dirty = true;
		}

		
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}

		
		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		
		private bool dirty = true;

		
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		
		private const int MaxVerticesPerMesh = 40000;
	}
}
