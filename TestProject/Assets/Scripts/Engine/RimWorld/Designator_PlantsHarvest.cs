using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4B RID: 3659
	public class Designator_PlantsHarvest : Designator_Plants
	{
		// Token: 0x06005872 RID: 22642 RVA: 0x001D5788 File Offset: 0x001D3988
		public Designator_PlantsHarvest()
		{
			this.defaultLabel = "DesignatorHarvest".Translate();
			this.defaultDesc = "DesignatorHarvestDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Harvest", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Harvest;
			this.hotKey = KeyBindingDefOf.Misc2;
			this.designationDef = DesignationDefOf.HarvestPlant;
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x001D5814 File Offset: 0x001D3A14
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (!plant.HarvestableNow || plant.def.plant.harvestTag != "Standard")
			{
				return "MessageMustDesignateHarvestable".Translate();
			}
			return true;
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x001D5875 File Offset: 0x001D3A75
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Standard";
		}
	}
}
