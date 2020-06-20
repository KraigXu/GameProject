using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F7 RID: 759
	public abstract class Graphic_WithPropertyBlock : Graphic_Single
	{
		// Token: 0x06001576 RID: 5494 RVA: 0x0007D698 File Offset: 0x0007B898
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x04000E0B RID: 3595
		protected MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
