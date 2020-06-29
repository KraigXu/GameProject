using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class LayerSubMesh
	{
		
		public LayerSubMesh(Mesh mesh, Material material)
		{
			this.mesh = mesh;
			this.material = material;
		}

		
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

		
		public override string ToString()
		{
			return "LayerSubMesh(" + this.material.ToString() + ")";
		}

		
		public bool finalized;

		
		public bool disabled;

		
		public Material material;

		
		public Mesh mesh;

		
		public List<Vector3> verts = new List<Vector3>();

		
		public List<int> tris = new List<int>();

		
		public List<Color32> colors = new List<Color32>();

		
		public List<Vector3> uvs = new List<Vector3>();
	}
}
