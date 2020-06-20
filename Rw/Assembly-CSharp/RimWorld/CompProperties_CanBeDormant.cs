using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFA RID: 3322
	public class CompProperties_CanBeDormant : CompProperties
	{
		// Token: 0x060050B9 RID: 20665 RVA: 0x001B1CB4 File Offset: 0x001AFEB4
		public CompProperties_CanBeDormant()
		{
			this.compClass = typeof(CompCanBeDormant);
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x001B1D0A File Offset: 0x001AFF0A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!parentDef.receivesSignals)
			{
				yield return "ThingDefs with CanBeDormant component must have receivesSignals set to true, otherwise wakeup won't work properly!";
			}
			yield break;
		}

		// Token: 0x04002CD4 RID: 11476
		public bool startsDormant;

		// Token: 0x04002CD5 RID: 11477
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x04002CD6 RID: 11478
		public float maxDistAwakenByOther = 40f;

		// Token: 0x04002CD7 RID: 11479
		public bool canWakeUpFogged = true;

		// Token: 0x04002CD8 RID: 11480
		public string awakeStateLabelKey = "AwokeDaysAgo";

		// Token: 0x04002CD9 RID: 11481
		public string dormantStateLabelKey = "DormantCompInactive";
	}
}
