using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B3 RID: 691
	[StaticConstructorOnStartup]
	public static class MeshPool
	{
		// Token: 0x060013B3 RID: 5043 RVA: 0x000711A0 File Offset: 0x0006F3A0
		static MeshPool()
		{
			for (int i = 0; i < 361; i++)
			{
				MeshPool.pies[i] = MeshMakerCircles.MakePieMesh(i);
			}
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x000712DC File Offset: 0x0006F4DC
		public static Mesh GridPlane(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planes.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, false, false, false);
				MeshPool.planes.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00071310 File Offset: 0x0006F510
		public static Mesh GridPlaneFlip(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planesFlip.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, true, false, false);
				MeshPool.planesFlip.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00071343 File Offset: 0x0006F543
		private static Vector2 RoundedToHundredths(this Vector2 v)
		{
			return new Vector2((float)((int)(v.x * 100f)) / 100f, (float)((int)(v.y * 100f)) / 100f);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00071374 File Offset: 0x0006F574
		[DebugOutput("System", false)]
		public static void MeshPoolStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MeshPool stats:");
			stringBuilder.AppendLine("Planes: " + MeshPool.planes.Count);
			stringBuilder.AppendLine("PlanesFlip: " + MeshPool.planesFlip.Count);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04000D38 RID: 3384
		private const int MaxGridMeshSize = 15;

		// Token: 0x04000D39 RID: 3385
		private const float HumanlikeBodyWidth = 1.5f;

		// Token: 0x04000D3A RID: 3386
		private const float HumanlikeHeadAverageWidth = 1.5f;

		// Token: 0x04000D3B RID: 3387
		private const float HumanlikeHeadNarrowWidth = 1.3f;

		// Token: 0x04000D3C RID: 3388
		public static readonly GraphicMeshSet humanlikeBodySet = new GraphicMeshSet(1.5f);

		// Token: 0x04000D3D RID: 3389
		public static readonly GraphicMeshSet humanlikeHeadSet = new GraphicMeshSet(1.5f);

		// Token: 0x04000D3E RID: 3390
		public static readonly GraphicMeshSet humanlikeHairSetAverage = new GraphicMeshSet(1.5f);

		// Token: 0x04000D3F RID: 3391
		public static readonly GraphicMeshSet humanlikeHairSetNarrow = new GraphicMeshSet(1.3f, 1.5f);

		// Token: 0x04000D40 RID: 3392
		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		// Token: 0x04000D41 RID: 3393
		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		// Token: 0x04000D42 RID: 3394
		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		// Token: 0x04000D43 RID: 3395
		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		// Token: 0x04000D44 RID: 3396
		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		// Token: 0x04000D45 RID: 3397
		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		// Token: 0x04000D46 RID: 3398
		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		// Token: 0x04000D47 RID: 3399
		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		// Token: 0x04000D48 RID: 3400
		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		// Token: 0x04000D49 RID: 3401
		public static readonly Mesh wholeMapPlane;

		// Token: 0x04000D4A RID: 3402
		private static Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x04000D4B RID: 3403
		private static Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x04000D4C RID: 3404
		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		// Token: 0x04000D4D RID: 3405
		public static readonly Mesh[] pies = new Mesh[361];
	}
}
