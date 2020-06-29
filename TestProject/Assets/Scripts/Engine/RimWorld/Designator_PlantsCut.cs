using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Designator_PlantsCut : Designator_Plants
	{
		
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

		
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			return this.AffectsThing(t);
		}

		
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return this.AffectsThing(target.Thing);
		}

		
		private bool AffectsThing(Thing t)
		{
			Plant plant;
			return (plant = (t as Plant)) != null && (this.isOrder || !plant.def.plant.IsTree || !plant.HarvestableNow);
		}

		
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
