using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000386 RID: 902
	public static class EnvironmentStatsDrawer
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x000A4114 File Offset: 0x000A2314
		private static int DisplayedRoomStatsCount
		{
			get
			{
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!allDefsListForReading[i].isHidden || DebugViewSettings.showAllRoomStats)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000A4154 File Offset: 0x000A2354
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000A4170 File Offset: 0x000A2370
		private static bool ShouldShowRoomStats()
		{
			if (!Find.PlaySettings.showRoomStats)
			{
				return false;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap))
			{
				return false;
			}
			Room room = UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All);
			return room != null && room.Role != RoomRoleDefOf.None;
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000A41D8 File Offset: 0x000A23D8
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_Passable) != null;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000A422A File Offset: 0x000A242A
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				return;
			}
			EnvironmentStatsDrawer.DrawInfoWindow();
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000A4248 File Offset: 0x000A2448
		private static void DrawInfoWindow()
		{
			Text.Font = GameFont.Small;
			Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(EnvironmentStatsDrawer.ShouldShowBeauty(), EnvironmentStatsDrawer.ShouldShowRoomStats());
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
			{
				EnvironmentStatsDrawer.FillWindow(windowRect);
			}, true, false, 1f);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000A42A0 File Offset: 0x000A24A0
		public static Rect GetWindowRect(bool shouldShowBeauty, bool shouldShowRoomStats)
		{
			Rect result = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 416f, 36f);
			if (shouldShowBeauty)
			{
				result.height += 25f;
			}
			if (shouldShowRoomStats)
			{
				if (shouldShowBeauty)
				{
					result.height += 13f;
				}
				result.height += 23f;
				result.height += (float)EnvironmentStatsDrawer.DisplayedRoomStatsCount * 25f;
			}
			result.x += 26f;
			result.y += 26f;
			if (result.xMax > (float)UI.screenWidth)
			{
				result.x -= result.width + 52f;
			}
			if (result.yMax > (float)UI.screenHeight)
			{
				result.y -= result.height + 52f;
			}
			return result;
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000A43B4 File Offset: 0x000A25B4
		private static void FillWindow(Rect windowRect)
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.FrameDisplayed);
			Text.Font = GameFont.Small;
			float num = 18f;
			bool flag = EnvironmentStatsDrawer.ShouldShowBeauty();
			if (flag)
			{
				float beauty = BeautyUtility.AverageBeautyPerceptible(UI.MouseCell(), Find.CurrentMap);
				Rect rect = new Rect(18f, num, windowRect.width - 36f, 100f);
				GUI.color = BeautyDrawer.BeautyColor(beauty, 40f);
				Widgets.Label(rect, "BeautyHere".Translate() + ": " + beauty.ToString("F1"));
				num += 25f;
			}
			if (EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				if (flag)
				{
					num += 5f;
					GUI.color = new Color(1f, 1f, 1f, 0.4f);
					Widgets.DrawLineHorizontal(18f, num, windowRect.width - 36f);
					GUI.color = Color.white;
					num += 8f;
				}
				EnvironmentStatsDrawer.DoRoomInfo(UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All), ref num, windowRect);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000A44CC File Offset: 0x000A26CC
		public static void DrawRoomOverlays()
		{
			if (Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap))
			{
				GenUI.RenderMouseoverBracket();
			}
			if (EnvironmentStatsDrawer.ShouldShowWindowNow() && EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000A4530 File Offset: 0x000A2730
		public static void DoRoomInfo(Room room, ref float curY, Rect windowRect)
		{
			Rect rect = new Rect(18f, curY, windowRect.width - 36f, 100f);
			GUI.color = Color.white;
			Widgets.Label(rect, EnvironmentStatsDrawer.GetRoomRoleLabel(room));
			curY += 25f;
			Text.WordWrap = false;
			for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
			{
				RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
				if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
				{
					float stat = room.GetStat(roomStatDef);
					RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
					if (room.Role.IsStatRelated(roomStatDef))
					{
						GUI.color = EnvironmentStatsDrawer.RelatedStatColor;
					}
					else
					{
						GUI.color = Color.gray;
					}
					Rect rect2 = new Rect(rect.x, curY, 100f, 23f);
					Widgets.Label(rect2, roomStatDef.LabelCap);
					Rect rect3 = new Rect(rect2.xMax + 35f, curY, 50f, 23f);
					string label = roomStatDef.ScoreToString(stat);
					Widgets.Label(rect3, label);
					Widgets.Label(new Rect(rect3.xMax + 35f, curY, 160f, 23f), (scoreStage == null) ? "" : scoreStage.label);
					curY += 25f;
				}
			}
			Text.WordWrap = true;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000A4690 File Offset: 0x000A2890
		private static string GetRoomRoleLabel(Room room)
		{
			Pawn pawn = null;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in room.Owners)
			{
				if (pawn == null)
				{
					pawn = pawn3;
				}
				else
				{
					pawn2 = pawn3;
				}
			}
			TaggedString taggedString;
			if (pawn == null)
			{
				taggedString = room.Role.LabelCap;
			}
			else if (pawn2 == null)
			{
				taggedString = "SomeonesRoom".Translate(pawn.LabelShort, room.Role.label, pawn.Named("PAWN"));
			}
			else
			{
				taggedString = "CouplesRoom".Translate(pawn.LabelShort, pawn2.LabelShort, room.Role.label, pawn.Named("PAWN1"), pawn2.Named("PAWN2"));
			}
			return taggedString;
		}

		// Token: 0x04000FC1 RID: 4033
		private const float StatLabelColumnWidth = 100f;

		// Token: 0x04000FC2 RID: 4034
		private const float ScoreColumnWidth = 50f;

		// Token: 0x04000FC3 RID: 4035
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x04000FC4 RID: 4036
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x04000FC5 RID: 4037
		private const float DistFromMouse = 26f;

		// Token: 0x04000FC6 RID: 4038
		public const float WindowPadding = 18f;

		// Token: 0x04000FC7 RID: 4039
		private const float LineHeight = 23f;

		// Token: 0x04000FC8 RID: 4040
		private const float SpaceBetweenLines = 2f;

		// Token: 0x04000FC9 RID: 4041
		private const float SpaceBetweenColumns = 35f;
	}
}
