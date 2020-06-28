using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1E RID: 3614
	public class Designator_AreaIgnoreRoof : Designator_Area
	{
		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x0600574B RID: 22347 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x0600574C RID: 22348 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x001D04B0 File Offset: 0x001CE6B0
		public Designator_AreaIgnoreRoof()
		{
			this.defaultLabel = "DesignatorAreaIgnoreRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaIgnoreRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/IgnoreRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc11;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x001D0530 File Offset: 0x001CE730
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			return base.Map.areaManager.BuildRoof[c] || base.Map.areaManager.NoRoof[c];
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x001D059D File Offset: 0x001CE79D
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x001D0290 File Offset: 0x001CE490
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
