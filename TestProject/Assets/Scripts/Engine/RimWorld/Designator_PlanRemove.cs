using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2F RID: 3631
	public class Designator_PlanRemove : Designator_Plan
	{
		// Token: 0x060057C8 RID: 22472 RVA: 0x001D21C8 File Offset: 0x001D03C8
		public Designator_PlanRemove() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorPlanRemove".Translate();
			this.defaultDesc = "DesignatorPlanRemoveDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOff", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanRemove;
			this.hotKey = KeyBindingDefOf.Misc8;
		}
	}
}
