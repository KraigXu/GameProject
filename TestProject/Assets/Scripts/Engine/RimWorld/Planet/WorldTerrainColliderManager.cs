using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011FC RID: 4604
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x06006A7C RID: 27260 RVA: 0x002522F0 File Offset: 0x002504F0
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		// Token: 0x06006A7E RID: 27262 RVA: 0x00252303 File Offset: 0x00250503
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}

		// Token: 0x0400426F RID: 17007
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();
	}
}
