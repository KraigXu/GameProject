using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB5 RID: 3765
	[StaticConstructorOnStartup]
	public static class SelectionDrawer
	{
		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06005BE3 RID: 23523 RVA: 0x001FBA8C File Offset: 0x001F9C8C
		public static Dictionary<object, float> SelectTimes
		{
			get
			{
				return SelectionDrawer.selectTimes;
			}
		}

		// Token: 0x06005BE4 RID: 23524 RVA: 0x001FBA93 File Offset: 0x001F9C93
		public static void Notify_Selected(object t)
		{
			SelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x06005BE5 RID: 23525 RVA: 0x001FBAA5 File Offset: 0x001F9CA5
		public static void Clear()
		{
			SelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x06005BE6 RID: 23526 RVA: 0x001FBAB4 File Offset: 0x001F9CB4
		public static void DrawSelectionOverlays()
		{
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				SelectionDrawer.DrawSelectionBracketFor(obj);
				Thing thing = obj as Thing;
				if (thing != null)
				{
					thing.DrawExtraSelectionOverlays();
				}
			}
		}

		// Token: 0x06005BE7 RID: 23527 RVA: 0x001FBB18 File Offset: 0x001F9D18
		private static void DrawSelectionBracketFor(object obj)
		{
			Zone zone = obj as Zone;
			if (zone != null)
			{
				GenDraw.DrawFieldEdges(zone.Cells);
			}
			Thing thing = obj as Thing;
			if (thing != null)
			{
				CellRect? customRectForSelector = thing.CustomRectForSelector;
				if (customRectForSelector != null)
				{
					SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, customRectForSelector.Value.CenterVector3, new Vector2((float)customRectForSelector.Value.Width, (float)customRectForSelector.Value.Height), SelectionDrawer.selectTimes, Vector2.one, 1f);
				}
				else
				{
					SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, thing.DrawPos, thing.RotatedSize.ToVector2(), SelectionDrawer.selectTimes, Vector2.one, 1f);
				}
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					Quaternion rotation = Quaternion.AngleAxis((float)num, Vector3.up);
					Graphics.DrawMesh(MeshPool.plane10, SelectionDrawer.bracketLocs[i], rotation, SelectionDrawer.SelectionBracketMat, 0);
					num -= 90;
				}
			}
		}

		// Token: 0x04003231 RID: 12849
		private static Dictionary<object, float> selectTimes = new Dictionary<object, float>();

		// Token: 0x04003232 RID: 12850
		private static readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay);

		// Token: 0x04003233 RID: 12851
		private static Vector3[] bracketLocs = new Vector3[4];
	}
}
