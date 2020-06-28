using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DF RID: 735
	public class Gizmo_RoomStats : Gizmo
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x00079D3E File Offset: 0x00077F3E
		private Room Room
		{
			get
			{
				return Gizmo_RoomStats.GetRoomToShowStatsFor(this.building);
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00079D4B File Offset: 0x00077F4B
		public Gizmo_RoomStats(Building building)
		{
			this.building = building;
			this.order = -100f;
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00079D65 File Offset: 0x00077F65
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(300f, maxWidth);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00079D74 File Offset: 0x00077F74
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Room room = this.Room;
			if (room == null)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			Text.WordWrap = false;
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero().ContractedBy(10f);
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect(rect2.x, rect2.y - 2f, rect2.width, 100f);
			float stat = room.GetStat(RoomStatDefOf.Impressiveness);
			RoomStatScoreStage scoreStage = RoomStatDefOf.Impressiveness.GetScoreStage(stat);
			TaggedString str = room.Role.LabelCap + ", " + scoreStage.label + " (" + RoomStatDefOf.Impressiveness.ScoreToString(stat) + ")";
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			float num = rect2.y + Text.LineHeight + Text.SpaceBetweenLines + 7f;
			GUI.color = Gizmo_RoomStats.RoomStatsColor;
			Text.Font = GameFont.Tiny;
			List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
			int num2 = 0;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (!allDefsListForReading[i].isHidden && allDefsListForReading[i] != RoomStatDefOf.Impressiveness)
				{
					float stat2 = room.GetStat(allDefsListForReading[i]);
					RoomStatScoreStage scoreStage2 = allDefsListForReading[i].GetScoreStage(stat2);
					Rect rect4;
					if (num2 % 2 == 0)
					{
						rect4 = new Rect(rect2.x, num, rect2.width / 2f, 100f);
					}
					else
					{
						rect4 = new Rect(rect2.x + rect2.width / 2f, num, rect2.width / 2f, 100f);
					}
					string str2 = scoreStage2.label.CapitalizeFirst() + " (" + allDefsListForReading[i].ScoreToString(stat2) + ")";
					Widgets.Label(rect4, str2.Truncate(rect4.width, null));
					if (num2 % 2 == 1)
					{
						num += Text.LineHeight + Text.SpaceBetweenLines;
					}
					num2++;
				}
			}
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			GUI.EndGroup();
			Text.WordWrap = true;
			GenUI.AbsorbClicksInRect(rect);
			if (Mouse.IsOver(rect))
			{
				Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(false, true);
				Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
				{
					float num3 = 18f;
					EnvironmentStatsDrawer.DoRoomInfo(room, ref num3, windowRect);
				}, true, false, 1f);
				return new GizmoResult(GizmoState.Mouseover);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0007A068 File Offset: 0x00078268
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.FrameInteraction);
			Room room = this.Room;
			if (room != null)
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0007A098 File Offset: 0x00078298
		public static Room GetRoomToShowStatsFor(Building building)
		{
			if (!building.Spawned || building.Fogged())
			{
				return null;
			}
			Room room = null;
			if (building.def.passability != Traversability.Impassable)
			{
				room = building.GetRoom(RegionType.Set_Passable);
			}
			else if (building.def.hasInteractionCell)
			{
				room = building.InteractionCell.GetRoom(building.Map, RegionType.Set_Passable);
			}
			else
			{
				CellRect cellRect = building.OccupiedRect().ExpandedBy(1);
				foreach (IntVec3 intVec in cellRect)
				{
					if (cellRect.IsOnEdge(intVec))
					{
						room = intVec.GetRoom(building.Map, RegionType.Set_Passable);
						if (Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
						{
							break;
						}
					}
				}
			}
			if (!Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
			{
				return null;
			}
			return room;
		}

		// Token: 0x04000DBD RID: 3517
		private Building building;

		// Token: 0x04000DBE RID: 3518
		private static readonly Color RoomStatsColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
