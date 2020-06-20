using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2E RID: 3630
	public class Designator_PlanAdd : Designator_Plan
	{
		// Token: 0x060057C7 RID: 22471 RVA: 0x001D2160 File Offset: 0x001D0360
		public Designator_PlanAdd() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorPlan".Translate();
			this.defaultDesc = "DesignatorPlanDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanAdd;
			this.hotKey = KeyBindingDefOf.Misc9;
		}
	}
}
