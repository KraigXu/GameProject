using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3F RID: 3647
	public abstract class Designator_Zone : Designator
	{
		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06005842 RID: 22594 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06005843 RID: 22595 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x001D4BD3 File Offset: 0x001D2DD3
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			OverlayDrawHandler.DrawZonesThisFrame();
			if (Find.Selector.SelectedZone != null)
			{
				GenDraw.DrawFieldEdges(Find.Selector.SelectedZone.Cells);
			}
			GenDraw.DrawNoZoneEdgeLines();
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x001CFE8D File Offset: 0x001CE08D
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
