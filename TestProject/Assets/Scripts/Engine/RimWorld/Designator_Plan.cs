using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public abstract class Designator_Plan : Designator
	{
		
		// (get) Token: 0x060057BF RID: 22463 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		
		// (get) Token: 0x060057C0 RID: 22464 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x060057C1 RID: 22465 RVA: 0x001D2020 File Offset: 0x001D0220
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Plan;
			}
		}

		
		public Designator_Plan(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc9;
		}

		
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

		
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			GenDraw.DrawNoBuildEdgeLines();
		}

		
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		
		private DesignateMode mode;
	}
}
