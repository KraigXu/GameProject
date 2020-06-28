using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E19 RID: 3609
	public class Designator_AreaBuildRoof : Designator_Area
	{
		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x0600573B RID: 22331 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x0600573C RID: 22332 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x001D00E8 File Offset: 0x001CE2E8
		public Designator_AreaBuildRoof()
		{
			this.defaultLabel = "DesignatorAreaBuildRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaBuildRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc9;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
			this.tutorTag = "AreaBuildRoofExpand";
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x001D0170 File Offset: 0x001CE370
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
			return !base.Map.areaManager.BuildRoof[c];
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x001D01C5 File Offset: 0x001CE3C5
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = true;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x001D01F8 File Offset: 0x001CE3F8
		public override bool ShowWarningForCell(IntVec3 c)
		{
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(c))
			{
				if (thing.def.plant != null && thing.def.plant.interferesWithRoof)
				{
					Messages.Message("MessageRoofIncompatibleWithPlant".Translate(thing), MessageTypeDefOf.CautionInput, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x001D0290 File Offset: 0x001CE490
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
