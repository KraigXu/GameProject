using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020011DF RID: 4575
	public static class WorldCameraManager
	{
		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x060069E2 RID: 27106 RVA: 0x0024F705 File Offset: 0x0024D905
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.worldCameraInt;
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x060069E3 RID: 27107 RVA: 0x0024F70C File Offset: 0x0024D90C
		public static Camera WorldSkyboxCamera
		{
			get
			{
				return WorldCameraManager.worldSkyboxCameraInt;
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x060069E4 RID: 27108 RVA: 0x0024F713 File Offset: 0x0024D913
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.worldCameraDriverInt;
			}
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x0024F71C File Offset: 0x0024D91C
		static WorldCameraManager()
		{
			WorldCameraManager.worldCameraInt = WorldCameraManager.CreateWorldCamera();
			WorldCameraManager.worldSkyboxCameraInt = WorldCameraManager.CreateWorldSkyboxCamera(WorldCameraManager.worldCameraInt);
			WorldCameraManager.worldCameraDriverInt = WorldCameraManager.worldCameraInt.GetComponent<WorldCameraDriver>();
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x0024F7CC File Offset: 0x0024D9CC
		private static Camera CreateWorldCamera()
		{
			GameObject gameObject = new GameObject("WorldCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<WorldCameraDriver>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldLayerMask;
			component.clearFlags = CameraClearFlags.Depth;
			component.useOcclusionCulling = true;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 1200f;
			component.fieldOfView = 20f;
			component.depth = 1f;
			return component;
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x0024F864 File Offset: 0x0024DA64
		private static Camera CreateWorldSkyboxCamera(Camera parent)
		{
			GameObject gameObject = new GameObject("WorldSkyboxCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(true);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.SetParent(parent.transform);
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldSkyboxLayerMask;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = WorldCameraManager.SkyColor;
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 1200f;
			component.fieldOfView = 60f;
			component.depth = 0f;
			return component;
		}

		// Token: 0x040041F4 RID: 16884
		private static Camera worldCameraInt;

		// Token: 0x040041F5 RID: 16885
		private static Camera worldSkyboxCameraInt;

		// Token: 0x040041F6 RID: 16886
		private static WorldCameraDriver worldCameraDriverInt;

		// Token: 0x040041F7 RID: 16887
		public static readonly string WorldLayerName = "World";

		// Token: 0x040041F8 RID: 16888
		public static int WorldLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldLayerName
		});

		// Token: 0x040041F9 RID: 16889
		public static int WorldLayer = LayerMask.NameToLayer(WorldCameraManager.WorldLayerName);

		// Token: 0x040041FA RID: 16890
		public static readonly string WorldSkyboxLayerName = "WorldSkybox";

		// Token: 0x040041FB RID: 16891
		public static int WorldSkyboxLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldSkyboxLayerName
		});

		// Token: 0x040041FC RID: 16892
		public static int WorldSkyboxLayer = LayerMask.NameToLayer(WorldCameraManager.WorldSkyboxLayerName);

		// Token: 0x040041FD RID: 16893
		private static readonly Color SkyColor = new Color(0.0627451f, 0.09019608f, 0.117647059f);
	}
}
