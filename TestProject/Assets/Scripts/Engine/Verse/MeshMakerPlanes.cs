using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AE RID: 686
	public static class MeshMakerPlanes
	{
		// Token: 0x0600139E RID: 5022 RVA: 0x0007098F File Offset: 0x0006EB8F
		public static Mesh NewPlaneMesh(float size)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, false);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00070998 File Offset: 0x0006EB98
		public static Mesh NewPlaneMesh(float size, bool flipped)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, flipped, false);
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x000709A2 File Offset: 0x0006EBA2
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, false);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x000709B3 File Offset: 0x0006EBB3
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift, bool twist)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, twist);
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x000709C4 File Offset: 0x0006EBC4
		public static Mesh NewPlaneMesh(Vector2 size, bool flipped, bool backLift, bool twist)
		{
			Vector3[] array = new Vector3[4];
			Vector2[] array2 = new Vector2[4];
			int[] array3 = new int[6];
			array[0] = new Vector3(-0.5f * size.x, 0f, -0.5f * size.y);
			array[1] = new Vector3(-0.5f * size.x, 0f, 0.5f * size.y);
			array[2] = new Vector3(0.5f * size.x, 0f, 0.5f * size.y);
			array[3] = new Vector3(0.5f * size.x, 0f, -0.5f * size.y);
			if (backLift)
			{
				array[1].y = 0.00227272743f;
				array[2].y = 0.00227272743f;
				array[3].y = 0.000909091f;
			}
			if (twist)
			{
				array[0].y = 0.00113636372f;
				array[1].y = 0.000568181858f;
				array[2].y = 0f;
				array[3].y = 0.000568181858f;
			}
			if (!flipped)
			{
				array2[0] = new Vector2(0f, 0f);
				array2[1] = new Vector2(0f, 1f);
				array2[2] = new Vector2(1f, 1f);
				array2[3] = new Vector2(1f, 0f);
			}
			else
			{
				array2[0] = new Vector2(1f, 0f);
				array2[1] = new Vector2(1f, 1f);
				array2[2] = new Vector2(0f, 1f);
				array2[3] = new Vector2(0f, 0f);
			}
			array3[0] = 0;
			array3[1] = 1;
			array3[2] = 2;
			array3[3] = 0;
			array3[4] = 2;
			array3[5] = 3;
			Mesh mesh = new Mesh();
			mesh.name = "NewPlaneMesh()";
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.SetTriangles(array3, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00070C08 File Offset: 0x0006EE08
		public static Mesh NewWholeMapPlane()
		{
			Mesh mesh = MeshMakerPlanes.NewPlaneMesh(2000f, false, false);
			Vector2[] array = new Vector2[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = mesh.uv[i] * 200f;
			}
			mesh.uv = array;
			return mesh;
		}

		// Token: 0x04000D2F RID: 3375
		private const float BackLiftAmount = 0.00227272743f;

		// Token: 0x04000D30 RID: 3376
		private const float TwistAmount = 0.00113636372f;
	}
}
