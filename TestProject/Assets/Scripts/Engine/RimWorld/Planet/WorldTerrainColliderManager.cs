using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		
		// (get) Token: 0x06006A7C RID: 27260 RVA: 0x002522F0 File Offset: 0x002504F0
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}

		
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();
	}
}
