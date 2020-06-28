using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E15 RID: 3605
	public abstract class Designator_Area : Designator
	{
		// Token: 0x0600572A RID: 22314 RVA: 0x001CFE8D File Offset: 0x001CE08D
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
