using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E20 RID: 3616
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06005759 RID: 22361 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x0600575A RID: 22362 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x001D0758 File Offset: 0x001CE958
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x001D07A8 File Offset: 0x001CE9A8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.SnowClear[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x001D07F9 File Offset: 0x001CE9F9
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.SnowClear[c] = true;
				return;
			}
			base.Map.areaManager.SnowClear[c] = false;
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x001D0832 File Offset: 0x001CEA32
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}

		// Token: 0x04002F9A RID: 12186
		private DesignateMode mode;
	}
}
