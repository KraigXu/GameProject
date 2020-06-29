using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public static class TooltipHandler
	{
		
		public static void ClearTooltipsFrom(Rect rect)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.dyingTips.Clear();
				foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
				{
					if (keyValuePair.Value.lastTriggerFrame == TooltipHandler.frame)
					{
						TooltipHandler.dyingTips.Add(keyValuePair.Key);
					}
				}
				for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
				{
					TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
				}
			}
		}

		
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		
		public static void TipRegionByKey(Rect rect, string key)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate());
		}

		
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1));
		}

		
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2));
		}

		
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2, arg3));
		}

		
		public static void TipRegion(Rect rect, TipSignal tip)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (tip.textGetter == null && tip.text.NullOrEmpty())
			{
				return;
			}
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			if (DebugViewSettings.drawTooltipEdges)
			{
				Widgets.DrawBox(rect, 1);
			}
			if (!TooltipHandler.activeTips.ContainsKey(tip.uniqueId))
			{
				ActiveTip value = new ActiveTip(tip);
				TooltipHandler.activeTips.Add(tip.uniqueId, value);
				TooltipHandler.activeTips[tip.uniqueId].firstTriggerTime = (double)Time.realtimeSinceStartup;
			}
			TooltipHandler.activeTips[tip.uniqueId].lastTriggerFrame = TooltipHandler.frame;
			TooltipHandler.activeTips[tip.uniqueId].signal.text = tip.text;
			TooltipHandler.activeTips[tip.uniqueId].signal.textGetter = tip.textGetter;
		}

		
		public static void DoTooltipGUI()
		{
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		
		private static void DrawActiveTips()
		{
			if (TooltipHandler.activeTips.Count == 0)
			{
				return;
			}
			TooltipHandler.drawingTips.Clear();
			foreach (ActiveTip activeTip in TooltipHandler.activeTips.Values)
			{
				if ((double)Time.realtimeSinceStartup > activeTip.firstTriggerTime + (double)activeTip.signal.delay)
				{
					TooltipHandler.drawingTips.Add(activeTip);
				}
			}
			if (TooltipHandler.drawingTips.Any<ActiveTip>())
			{
				TooltipHandler.drawingTips.Sort(TooltipHandler.compareTooltipsByPriorityCached);
				Vector2 pos = TooltipHandler.CalculateInitialTipPosition(TooltipHandler.drawingTips);
				for (int i = 0; i < TooltipHandler.drawingTips.Count; i++)
				{
					pos.y += TooltipHandler.drawingTips[i].DrawTooltip(pos);
					pos.y += 2f;
				}
				TooltipHandler.drawingTips.Clear();
			}
		}

		
		private static void CleanActiveTooltips()
		{
			TooltipHandler.dyingTips.Clear();
			foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
			{
				if (keyValuePair.Value.lastTriggerFrame != TooltipHandler.frame)
				{
					TooltipHandler.dyingTips.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
			{
				TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
			}
		}

		
		private static Vector2 CalculateInitialTipPosition(List<ActiveTip> drawingTips)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < drawingTips.Count; i++)
			{
				Rect tipRect = drawingTips[i].TipRect;
				num += tipRect.height;
				num2 = Mathf.Max(num2, tipRect.width);
				if (i != drawingTips.Count - 1)
				{
					num += 2f;
				}
			}
			return GenUI.GetMouseAttachedWindowPos(num2, num);
		}

		
		private static int CompareTooltipsByPriority(ActiveTip A, ActiveTip B)
		{
			if (A.signal.priority == B.signal.priority)
			{
				return 0;
			}
			if (A.signal.priority == TooltipPriority.Pawn)
			{
				return -1;
			}
			if (B.signal.priority == TooltipPriority.Pawn)
			{
				return 1;
			}
			return 0;
		}

		
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		
		private static int frame = 0;

		
		private static List<int> dyingTips = new List<int>(32);

		
		private const float SpaceBetweenTooltips = 2f;

		
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		
		private static Comparison<ActiveTip> compareTooltipsByPriorityCached = new Comparison<ActiveTip>(TooltipHandler.CompareTooltipsByPriority);
	}
}
