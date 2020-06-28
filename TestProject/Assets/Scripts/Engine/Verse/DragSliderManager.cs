using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C2 RID: 962
	public static class DragSliderManager
	{
		// Token: 0x06001C4C RID: 7244 RVA: 0x000AC10A File Offset: 0x000AA30A
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000AC112 File Offset: 0x000AA312
		public static bool DragSlider(Rect rect, float rateFactor, DragSliderCallback newStartMethod, DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragSliderManager.lastRateFactor = rateFactor;
				newStartMethod(0f, rateFactor);
				DragSliderManager.StartDragSliding(newDraggingUpdateMethod, newCompletedMethod);
				return true;
			}
			return false;
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000AC151 File Offset: 0x000AA351
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000AC174 File Offset: 0x000AA374
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000AC188 File Offset: 0x000AA388
		public static void DragSlidersOnGUI()
		{
			if (DragSliderManager.dragging && Event.current.type == EventType.MouseUp && Event.current.button == 0)
			{
				DragSliderManager.dragging = false;
				if (DragSliderManager.completedMethod != null)
				{
					DragSliderManager.completedMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
				}
			}
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x000AC1D6 File Offset: 0x000AA3D6
		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging && DragSliderManager.draggingUpdateMethod != null)
			{
				DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
			}
		}

		// Token: 0x040010AF RID: 4271
		private static bool dragging = false;

		// Token: 0x040010B0 RID: 4272
		private static float rootX;

		// Token: 0x040010B1 RID: 4273
		private static float lastRateFactor = 1f;

		// Token: 0x040010B2 RID: 4274
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x040010B3 RID: 4275
		private static DragSliderCallback completedMethod;
	}
}
