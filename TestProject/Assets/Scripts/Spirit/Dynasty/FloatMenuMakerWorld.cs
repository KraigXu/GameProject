using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200128C RID: 4748
	public static class FloatMenuMakerWorld
	{
		// Token: 0x06006FB2 RID: 28594 RVA: 0x0026E380 File Offset: 0x0026C580
		public static bool TryMakeFloatMenu(Caravan caravan)
		{
			if (!caravan.IsPlayerControlled)
			{
				return false;
			}
			Vector2 mousePositionOnUI = UI.MousePositionOnUI;
			List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(mousePositionOnUI, caravan);
			if (list.Count == 0)
			{
				return false;
			}
			FloatMenuWorld window = new FloatMenuWorld(list, caravan.LabelCap, mousePositionOnUI);
			Find.WindowStack.Add(window);
			return true;
		}

		// Token: 0x06006FB3 RID: 28595 RVA: 0x0026E3CC File Offset: 0x0026C5CC
		public static List<FloatMenuOption> ChoicesAtFor(Vector2 mousePos, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> list2 = GenWorldUI.WorldObjectsUnderMouse(mousePos);
			for (int i = 0; i < list2.Count; i++)
			{
				list.AddRange(list2[i].GetFloatMenuOptions(caravan));
			}
			return list;
		}

		// Token: 0x06006FB4 RID: 28596 RVA: 0x0026E40C File Offset: 0x0026C60C
		public static List<FloatMenuOption> ChoicesAtFor(int tile, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile)
				{
					list.AddRange(allWorldObjects[i].GetFloatMenuOptions(caravan));
				}
			}
			return list;
		}
	}
}
