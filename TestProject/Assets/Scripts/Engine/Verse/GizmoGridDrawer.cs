using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000395 RID: 917
	public static class GizmoGridDrawer
	{
		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x000A58FE File Offset: 0x000A3AFE
		public static float HeightDrawnRecently
		{
			get
			{
				if (Time.frameCount > GizmoGridDrawer.heightDrawnFrame + 2)
				{
					return 0f;
				}
				return GizmoGridDrawer.heightDrawn;
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000A591C File Offset: 0x000A3B1C
		public static void DrawGizmoGrid(IEnumerable<Gizmo> gizmos, float startX, out Gizmo mouseoverGizmo)
		{
			GizmoGridDrawer.tmpAllGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.AddRange(gizmos);
			GizmoGridDrawer.tmpAllGizmos.SortStable(GizmoGridDrawer.SortByOrder);
			GizmoGridDrawer.gizmoGroups.Clear();
			for (int i = 0; i < GizmoGridDrawer.tmpAllGizmos.Count; i++)
			{
				Gizmo gizmo = GizmoGridDrawer.tmpAllGizmos[i];
				bool flag = false;
				for (int j = 0; j < GizmoGridDrawer.gizmoGroups.Count; j++)
				{
					if (GizmoGridDrawer.gizmoGroups[j][0].GroupsWith(gizmo))
					{
						flag = true;
						GizmoGridDrawer.gizmoGroups[j].Add(gizmo);
						GizmoGridDrawer.gizmoGroups[j][0].MergeWith(gizmo);
						break;
					}
				}
				if (!flag)
				{
					List<Gizmo> list = SimplePool<List<Gizmo>>.Get();
					list.Add(gizmo);
					GizmoGridDrawer.gizmoGroups.Add(list);
				}
			}
			GizmoGridDrawer.firstGizmos.Clear();
			for (int k = 0; k < GizmoGridDrawer.gizmoGroups.Count; k++)
			{
				List<Gizmo> list2 = GizmoGridDrawer.gizmoGroups[k];
				Gizmo gizmo2 = null;
				for (int l = 0; l < list2.Count; l++)
				{
					if (!list2[l].disabled)
					{
						gizmo2 = list2[l];
						break;
					}
				}
				if (gizmo2 == null)
				{
					gizmo2 = list2.FirstOrDefault<Gizmo>();
				}
				else
				{
					Command_Toggle command_Toggle = gizmo2 as Command_Toggle;
					if (command_Toggle != null)
					{
						if (!command_Toggle.activateIfAmbiguous && !command_Toggle.isActive())
						{
							for (int m = 0; m < list2.Count; m++)
							{
								Command_Toggle command_Toggle2 = list2[m] as Command_Toggle;
								if (command_Toggle2 != null && !command_Toggle2.disabled && command_Toggle2.isActive())
								{
									gizmo2 = list2[m];
									break;
								}
							}
						}
						if (command_Toggle.activateIfAmbiguous && command_Toggle.isActive())
						{
							for (int n = 0; n < list2.Count; n++)
							{
								Command_Toggle command_Toggle3 = list2[n] as Command_Toggle;
								if (command_Toggle3 != null && !command_Toggle3.disabled && !command_Toggle3.isActive())
								{
									gizmo2 = list2[n];
									break;
								}
							}
						}
					}
				}
				if (gizmo2 != null)
				{
					GizmoGridDrawer.firstGizmos.Add(gizmo2);
				}
			}
			GizmoGridDrawer.drawnHotKeys.Clear();
			float num = (float)(UI.screenWidth - 147);
			float maxWidth = num - startX;
			Text.Font = GameFont.Tiny;
			Vector2 vector = new Vector2(startX, (float)(UI.screenHeight - 35) - GizmoGridDrawer.GizmoSpacing.y - 75f);
			mouseoverGizmo = null;
			Gizmo gizmo3 = null;
			Event ev = null;
			Gizmo gizmo4 = null;
			for (int num2 = 0; num2 < GizmoGridDrawer.firstGizmos.Count; num2++)
			{
				Gizmo gizmo5 = GizmoGridDrawer.firstGizmos[num2];
				if (gizmo5.Visible)
				{
					if (vector.x + gizmo5.GetWidth(maxWidth) > num)
					{
						vector.x = startX;
						vector.y -= 75f + GizmoGridDrawer.GizmoSpacing.x;
					}
					GizmoGridDrawer.heightDrawnFrame = Time.frameCount;
					GizmoGridDrawer.heightDrawn = (float)UI.screenHeight - vector.y;
					GizmoResult gizmoResult = gizmo5.GizmoOnGUI(vector, maxWidth);
					if (gizmoResult.State == GizmoState.Interacted || (gizmoResult.State == GizmoState.OpenedFloatMenu && gizmo5.RightClickFloatMenuOptions.FirstOrDefault<FloatMenuOption>() == null))
					{
						ev = gizmoResult.InteractEvent;
						gizmo3 = gizmo5;
					}
					else if (gizmoResult.State == GizmoState.OpenedFloatMenu)
					{
						gizmo4 = gizmo5;
					}
					if (gizmoResult.State >= GizmoState.Mouseover)
					{
						mouseoverGizmo = gizmo5;
					}
					GenUI.AbsorbClicksInRect(new Rect(vector.x, vector.y, gizmo5.GetWidth(maxWidth), 75f + GizmoGridDrawer.GizmoSpacing.y).ContractedBy(-12f));
					vector.x += gizmo5.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
				}
			}
			if (gizmo3 != null)
			{
				List<Gizmo> list3 = GizmoGridDrawer.<DrawGizmoGrid>g__FindMatchingGroup|10_0(gizmo3);
				for (int num3 = 0; num3 < list3.Count; num3++)
				{
					Gizmo gizmo6 = list3[num3];
					if (gizmo6 != gizmo3 && !gizmo6.disabled && gizmo3.InheritInteractionsFrom(gizmo6))
					{
						gizmo6.ProcessInput(ev);
					}
				}
				gizmo3.ProcessInput(ev);
				Event.current.Use();
			}
			else if (gizmo4 != null)
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				foreach (FloatMenuOption item in gizmo4.RightClickFloatMenuOptions)
				{
					list4.Add(item);
				}
				List<Gizmo> list5 = GizmoGridDrawer.<DrawGizmoGrid>g__FindMatchingGroup|10_0(gizmo4);
				for (int num4 = 0; num4 < list5.Count; num4++)
				{
					Gizmo gizmo7 = list5[num4];
					if (gizmo7 != gizmo4 && !gizmo7.disabled && gizmo4.InheritFloatMenuInteractionsFrom(gizmo7))
					{
						foreach (FloatMenuOption floatMenuOption in gizmo7.RightClickFloatMenuOptions)
						{
							FloatMenuOption floatMenuOption2 = null;
							for (int num5 = 0; num5 < list4.Count; num5++)
							{
								if (list4[num5].Label == floatMenuOption.Label)
								{
									floatMenuOption2 = list4[num5];
									break;
								}
							}
							if (floatMenuOption2 == null)
							{
								list4.Add(floatMenuOption);
							}
							else if (!floatMenuOption.Disabled)
							{
								if (!floatMenuOption2.Disabled)
								{
									Action prevAction = floatMenuOption2.action;
									Action localOptionAction = floatMenuOption.action;
									floatMenuOption2.action = delegate
									{
										prevAction();
										localOptionAction();
									};
								}
								else if (floatMenuOption2.Disabled)
								{
									list4[list4.IndexOf(floatMenuOption2)] = floatMenuOption;
								}
							}
						}
					}
				}
				Event.current.Use();
				if (list4.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list4));
				}
			}
			for (int num6 = 0; num6 < GizmoGridDrawer.gizmoGroups.Count; num6++)
			{
				GizmoGridDrawer.gizmoGroups[num6].Clear();
				SimplePool<List<Gizmo>>.Return(GizmoGridDrawer.gizmoGroups[num6]);
			}
			GizmoGridDrawer.gizmoGroups.Clear();
			GizmoGridDrawer.firstGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.Clear();
		}

		// Token: 0x04000FFD RID: 4093
		public static HashSet<KeyCode> drawnHotKeys = new HashSet<KeyCode>();

		// Token: 0x04000FFE RID: 4094
		private static float heightDrawn;

		// Token: 0x04000FFF RID: 4095
		private static int heightDrawnFrame;

		// Token: 0x04001000 RID: 4096
		public static readonly Vector2 GizmoSpacing = new Vector2(5f, 14f);

		// Token: 0x04001001 RID: 4097
		private static List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>>();

		// Token: 0x04001002 RID: 4098
		private static List<Gizmo> firstGizmos = new List<Gizmo>();

		// Token: 0x04001003 RID: 4099
		private static List<Gizmo> tmpAllGizmos = new List<Gizmo>();

		// Token: 0x04001004 RID: 4100
		private static readonly Func<Gizmo, Gizmo, int> SortByOrder = (Gizmo lhs, Gizmo rhs) => lhs.order.CompareTo(rhs.order);
	}
}
