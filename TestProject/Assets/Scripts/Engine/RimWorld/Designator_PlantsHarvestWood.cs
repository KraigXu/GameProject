using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4C RID: 3660
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x06005875 RID: 22645 RVA: 0x001D5898 File Offset: 0x001D3A98
		public Designator_PlantsHarvestWood()
		{
			this.defaultLabel = "DesignatorHarvestWood".Translate();
			this.defaultDesc = "DesignatorHarvestWoodDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HarvestWood", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Harvest;
			this.hotKey = KeyBindingDefOf.Misc1;
			this.designationDef = DesignationDefOf.HarvestPlant;
			this.tutorTag = "PlantsHarvestWood";
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x001D5930 File Offset: 0x001D3B30
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (!plant.HarvestableNow || !plant.def.plant.IsTree)
			{
				return "MessageMustDesignateHarvestableWood".Translate();
			}
			return true;
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x001D5987 File Offset: 0x001D3B87
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.IsTree;
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x001D5752 File Offset: 0x001D3952
		public override void DesignateThing(Thing t)
		{
			if (t.def == ThingDefOf.Plant_TreeAnima)
			{
				Messages.Message("MessageWarningCutAnimaTree".Translate(), t, MessageTypeDefOf.CautionInput, false);
			}
			base.DesignateThing(t);
		}
	}
}
