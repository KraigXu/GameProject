using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B30 RID: 2864
	public static class PortraitCameraManager
	{
		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004392 RID: 17298 RVA: 0x0016C35D File Offset: 0x0016A55D
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.portraitCameraInt;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004393 RID: 17299 RVA: 0x0016C364 File Offset: 0x0016A564
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.portraitRendererInt;
			}
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0016C388 File Offset: 0x0016A588
		private static Camera CreatePortraitCamera()
		{
			GameObject gameObject = new GameObject("PortraitCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<PortraitRenderer>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.position = new Vector3(0f, 15f, 0f);
			component.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			component.orthographic = true;
			component.cullingMask = 0;
			component.orthographicSize = 1f;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = new Color(0f, 0f, 0f, 0f);
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			Camera camera = Current.Camera;
			component.nearClipPlane = camera.nearClipPlane;
			component.farClipPlane = camera.farClipPlane;
			return component;
		}

		// Token: 0x040026BE RID: 9918
		private static Camera portraitCameraInt = PortraitCameraManager.CreatePortraitCamera();

		// Token: 0x040026BF RID: 9919
		private static PortraitRenderer portraitRendererInt = PortraitCameraManager.portraitCameraInt.GetComponent<PortraitRenderer>();
	}
}
