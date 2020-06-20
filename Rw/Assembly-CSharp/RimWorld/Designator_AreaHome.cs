using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1B RID: 3611
	public abstract class Designator_AreaHome : Designator_Area
	{
		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x001D02C1 File Offset: 0x001CE4C1
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x001D02F8 File Offset: 0x001CE4F8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.Home[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x001D0349 File Offset: 0x001CE549
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.Home[c] = true;
				return;
			}
			base.Map.areaManager.Home[c] = false;
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x001D0382 File Offset: 0x001CE582
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x001D0395 File Offset: 0x001CE595
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x04002F98 RID: 12184
		private DesignateMode mode;
	}
}
