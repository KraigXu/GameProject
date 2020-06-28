using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B1 RID: 689
	public class GraphicMeshSet
	{
		// Token: 0x060013AB RID: 5035 RVA: 0x0007106C File Offset: 0x0006F26C
		public GraphicMeshSet(Mesh normalMesh, Mesh leftMesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			this.meshes[2] = normalMesh;
			array[num] = (array2[num2] = normalMesh);
			this.meshes[3] = leftMesh;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x000710B4 File Offset: 0x0006F2B4
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0007110C File Offset: 0x0006F30C
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0007116C File Offset: 0x0006F36C
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}

		// Token: 0x04000D36 RID: 3382
		private Mesh[] meshes = new Mesh[4];
	}
}
