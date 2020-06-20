using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9F RID: 3743
	public static class InspectGizmoGrid
	{
		// Token: 0x06005B54 RID: 23380 RVA: 0x001F6A94 File Offset: 0x001F4C94
		public static void DrawInspectGizmoGridFor(IEnumerable<object> selectedObjects, out Gizmo mouseoverGizmo)
		{
			mouseoverGizmo = null;
			try
			{
				InspectGizmoGrid.objList.Clear();
				InspectGizmoGrid.objList.AddRange(selectedObjects);
				InspectGizmoGrid.gizmoList.Clear();
				for (int i = 0; i < InspectGizmoGrid.objList.Count; i++)
				{
					ISelectable selectable = InspectGizmoGrid.objList[i] as ISelectable;
					if (selectable != null)
					{
						InspectGizmoGrid.gizmoList.AddRange(selectable.GetGizmos());
					}
				}
				for (int j = 0; j < InspectGizmoGrid.objList.Count; j++)
				{
					Thing t = InspectGizmoGrid.objList[j] as Thing;
					if (t != null)
					{
						List<Designator> allDesignators = Find.ReverseDesignatorDatabase.AllDesignators;
						for (int k = 0; k < allDesignators.Count; k++)
						{
							Designator des = allDesignators[k];
							if (des.CanDesignateThing(t).Accepted)
							{
								Command_Action command_Action = new Command_Action();
								command_Action.defaultLabel = des.LabelCapReverseDesignating(t);
								float iconAngle;
								Vector2 iconOffset;
								command_Action.icon = des.IconReverseDesignating(t, out iconAngle, out iconOffset);
								command_Action.iconAngle = iconAngle;
								command_Action.iconOffset = iconOffset;
								command_Action.defaultDesc = des.DescReverseDesignating(t);
								command_Action.order = ((des is Designator_Uninstall) ? -11f : -20f);
								command_Action.action = delegate
								{
									if (!TutorSystem.AllowAction(des.TutorTagDesignate))
									{
										return;
									}
									des.DesignateThing(t);
									des.Finalize(true);
								};
								command_Action.hotKey = des.hotKey;
								command_Action.groupKey = des.groupKey;
								InspectGizmoGrid.gizmoList.Add(command_Action);
							}
						}
					}
				}
				InspectGizmoGrid.objList.Clear();
				GizmoGridDrawer.DrawGizmoGrid(InspectGizmoGrid.gizmoList, InspectPaneUtility.PaneWidthFor(Find.WindowStack.WindowOfType<IInspectPane>()) + GizmoGridDrawer.GizmoSpacing.y, out mouseoverGizmo);
				InspectGizmoGrid.gizmoList.Clear();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 3427734, false);
			}
		}

		// Token: 0x040031D6 RID: 12758
		private static List<object> objList = new List<object>();

		// Token: 0x040031D7 RID: 12759
		private static List<Gizmo> gizmoList = new List<Gizmo>();
	}
}
