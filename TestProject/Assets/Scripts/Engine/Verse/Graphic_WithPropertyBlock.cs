using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class Graphic_WithPropertyBlock : Graphic_Single
	{
		
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		
		protected MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
