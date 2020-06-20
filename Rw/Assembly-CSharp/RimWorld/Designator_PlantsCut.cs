using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4A RID: 3658
	public class Designator_PlantsCut : Designator_Plants
	{
		// Token: 0x0600586D RID: 22637 RVA: 0x001D564C File Offset: 0x001D384C
		public Designator_PlantsCut()
		{
			this.defaultLabel = "DesignatorCutPlants".Translate();
			this.defaultDesc = "DesignatorCutPlantsDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/CutPlants", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_CutPlants;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.designationDef = DesignationDefOf.CutPlant;
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x001D56D8 File Offset: 0x001D38D8
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			return this.AffectsThing(t);
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x001D5704 File Offset: 0x001D3904
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return this.AffectsThing(target.Thing);
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x001D5714 File Offset: 0x001D3914
		private bool AffectsThing(Thing t)
		{
			Plant plant;
			return (plant = (t as Plant)) != null && (this.isOrder || !plant.def.plant.IsTree || !plant.HarvestableNow);
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x001D5752 File Offset: 0x001D3952
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
