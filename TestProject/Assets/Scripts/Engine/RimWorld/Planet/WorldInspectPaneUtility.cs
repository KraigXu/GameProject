using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200129C RID: 4764
	public static class WorldInspectPaneUtility
	{
		// Token: 0x06007050 RID: 28752 RVA: 0x00272A60 File Offset: 0x00270C60
		public static string AdjustedLabelFor(List<WorldObject> worldObjects, Rect rect)
		{
			if (worldObjects.Count == 1)
			{
				return worldObjects[0].LabelCap;
			}
			if (WorldInspectPaneUtility.AllLabelsAreSame(worldObjects))
			{
				return worldObjects[0].LabelCap + " x" + worldObjects.Count.ToStringCached();
			}
			return "VariousLabel".Translate();
		}

		// Token: 0x06007051 RID: 28753 RVA: 0x00272ABC File Offset: 0x00270CBC
		private static bool AllLabelsAreSame(List<WorldObject> worldObjects)
		{
			if (worldObjects.Count <= 1)
			{
				return true;
			}
			string labelCap = worldObjects[0].LabelCap;
			for (int i = 1; i < worldObjects.Count; i++)
			{
				if (worldObjects[i].LabelCap != labelCap)
				{
					return false;
				}
			}
			return true;
		}
	}
}
