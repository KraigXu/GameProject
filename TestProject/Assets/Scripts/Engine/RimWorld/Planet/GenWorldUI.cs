using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public static class GenWorldUI
	{
		
		// (get) Token: 0x06006FB5 RID: 28597 RVA: 0x0026E45E File Offset: 0x0026C65E
		public static float CaravanDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		
		// (get) Token: 0x06006FB6 RID: 28598 RVA: 0x0026E470 File Offset: 0x0026C670
		private static float CaravanWideClickRadius
		{
			get
			{
				return 0.75f * Find.WorldGrid.averageTileSize;
			}
		}

		
		// (get) Token: 0x06006FB7 RID: 28599 RVA: 0x0026E45E File Offset: 0x0026C65E
		private static float DynamicallyDrawnObjectDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		
		public static List<WorldObject> WorldObjectsUnderMouse(Vector2 mousePos)
		{
			List<WorldObject> list = new List<WorldObject>();
			ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(mousePos, list);
			float caravanDirectClickRadius = GenWorldUI.CaravanDirectClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.DistanceToMouse(mousePos) < caravanDirectClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int j = 0; j < GenWorldUI.clickedCaravans.Count; j++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[j]))
				{
					list.Add(GenWorldUI.clickedCaravans[j]);
				}
			}
			float dynamicallyDrawnObjectDirectClickRadius = GenWorldUI.DynamicallyDrawnObjectDirectClickRadius;
			GenWorldUI.clickedDynamicallyDrawnObjects.Clear();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int k = 0; k < allWorldObjects.Count; k++)
			{
				WorldObject worldObject = allWorldObjects[k];
				if (worldObject.def.useDynamicDrawer && worldObject.DistanceToMouse(mousePos) < dynamicallyDrawnObjectDirectClickRadius)
				{
					GenWorldUI.clickedDynamicallyDrawnObjects.Add(worldObject);
				}
			}
			GenWorldUI.clickedDynamicallyDrawnObjects.SortBy((WorldObject x) => x.DistanceToMouse(mousePos));
			for (int l = 0; l < GenWorldUI.clickedDynamicallyDrawnObjects.Count; l++)
			{
				if (!list.Contains(GenWorldUI.clickedDynamicallyDrawnObjects[l]))
				{
					list.Add(GenWorldUI.clickedDynamicallyDrawnObjects[l]);
				}
			}
			int num = GenWorld.TileAt(mousePos, false);
			List<WorldObject> allWorldObjects2 = Find.WorldObjects.AllWorldObjects;
			for (int m = 0; m < allWorldObjects2.Count; m++)
			{
				if (allWorldObjects2[m].Tile == num && !list.Contains(allWorldObjects2[m]))
				{
					list.Add(allWorldObjects2[m]);
				}
			}
			float caravanWideClickRadius = GenWorldUI.CaravanWideClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans2 = Find.WorldObjects.Caravans;
			for (int n = 0; n < caravans2.Count; n++)
			{
				Caravan caravan2 = caravans2[n];
				if (caravan2.DistanceToMouse(mousePos) < caravanWideClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan2);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int num2 = 0; num2 < GenWorldUI.clickedCaravans.Count; num2++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[num2]))
				{
					list.Add(GenWorldUI.clickedCaravans[num2]);
				}
			}
			GenWorldUI.clickedCaravans.Clear();
			return list;
		}

		
		public static Vector2 WorldToUIPosition(Vector3 worldLoc)
		{
			Vector3 vector = Find.WorldCamera.WorldToScreenPoint(worldLoc) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		
		public static float CurUITileSize()
		{
			Vector3 localPosition = Find.WorldCamera.transform.localPosition;
			Quaternion rotation = Find.WorldCamera.transform.rotation;
			Find.WorldCamera.transform.localPosition = new Vector3(0f, 0f, localPosition.magnitude);
			Find.WorldCamera.transform.rotation = Quaternion.identity;
			float x = (GenWorldUI.WorldToUIPosition(new Vector3(-Find.WorldGrid.averageTileSize / 2f, 0f, 100f)) - GenWorldUI.WorldToUIPosition(new Vector3(Find.WorldGrid.averageTileSize / 2f, 0f, 100f))).x;
			Find.WorldCamera.transform.localPosition = localPosition;
			Find.WorldCamera.transform.rotation = rotation;
			return x;
		}

		
		private static List<Caravan> clickedCaravans = new List<Caravan>();

		
		private static List<WorldObject> clickedDynamicallyDrawnObjects = new List<WorldObject>();
	}
}
