using System;

namespace Verse
{
	// Token: 0x020003D7 RID: 983
	internal static class DraggableResultUtility
	{
		// Token: 0x06001D43 RID: 7491 RVA: 0x000B3D80 File Offset: 0x000B1F80
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
