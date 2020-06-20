using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC6 RID: 4038
	public static class MiscDebugDrawer
	{
		// Token: 0x0600610F RID: 24847 RVA: 0x0021AFE8 File Offset: 0x002191E8
		public static void DebugDrawInteractionCells()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (DebugViewSettings.drawInteractionCells)
			{
				foreach (object obj in Find.Selector.SelectedObjects)
				{
					Thing thing = obj as Thing;
					if (thing != null)
					{
						CellRenderer.RenderCell(thing.InteractionCell, 0.5f);
					}
				}
			}
		}
	}
}
