using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public static class EnvironmentStatsDrawer
	{
		
		
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

		
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		
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

		
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_Passable) != null;
		}

		
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				return;
			}
			EnvironmentStatsDrawer.DrawInfoWindow();
		}

		
		private static void DrawInfoWindow()
		{
			Text.Font = GameFont.Small;
			Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(EnvironmentStatsDrawer.ShouldShowBeauty(), EnvironmentStatsDrawer.ShouldShowRoomStats());
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
			{
				EnvironmentStatsDrawer.FillWindow(windowRect);
			}, true, false, 1f);
		}

		
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

		
		private const float StatLabelColumnWidth = 100f;

		
		private const float ScoreColumnWidth = 50f;

		
		private const float ScoreStageLabelColumnWidth = 160f;

		
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		
		private const float DistFromMouse = 26f;

		
		public const float WindowPadding = 18f;

		
		private const float LineHeight = 23f;

		
		private const float SpaceBetweenLines = 2f;

		
		private const float SpaceBetweenColumns = 35f;
	}
}
