using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2D RID: 3629
	public abstract class Designator_Plan : Designator
	{
		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x060057BF RID: 22463 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x060057C0 RID: 22464 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060057C1 RID: 22465 RVA: 0x001D2020 File Offset: 0x001D0220
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x001D2027 File Offset: 0x001D0227
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x001D2060 File Offset: 0x001D0260
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.InNoBuildEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			if (this.mode == DesignateMode.Add)
			{
				if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
				{
					return false;
				}
			}
			else if (this.mode == DesignateMode.Remove && base.Map.designationManager.DesignationAt(c, this.Designation) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x001D20F8 File Offset: 0x001D02F8
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.designationManager.AddDesignation(new Designation(c, this.Designation));
				return;
			}
			if (this.mode == DesignateMode.Remove)
			{
				base.Map.designationManager.DesignationAt(c, this.Designation).Delete();
			}
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x001D2154 File Offset: 0x001D0354
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x001CFE8D File Offset: 0x001CE08D
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x04002FA3 RID: 12195
		private DesignateMode mode;
	}
}
