using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B78 RID: 2936
	public class DrugPolicyEntry : IExposable
	{
		// Token: 0x060044BE RID: 17598 RVA: 0x001735D0 File Offset: 0x001717D0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<bool>(ref this.allowedForAddiction, "allowedForAddiction", false, false);
			Scribe_Values.Look<bool>(ref this.allowedForJoy, "allowedForJoy", false, false);
			Scribe_Values.Look<bool>(ref this.allowScheduled, "allowScheduled", false, false);
			Scribe_Values.Look<float>(ref this.daysFrequency, "daysFrequency", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfMoodBelow, "onlyIfMoodBelow", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfJoyBelow, "onlyIfJoyBelow", 1f, false);
			Scribe_Values.Look<int>(ref this.takeToInventory, "takeToInventory", 0, false);
		}

		// Token: 0x04002740 RID: 10048
		public ThingDef drug;

		// Token: 0x04002741 RID: 10049
		public bool allowedForAddiction;

		// Token: 0x04002742 RID: 10050
		public bool allowedForJoy;

		// Token: 0x04002743 RID: 10051
		public bool allowScheduled;

		// Token: 0x04002744 RID: 10052
		public float daysFrequency = 1f;

		// Token: 0x04002745 RID: 10053
		public float onlyIfMoodBelow = 1f;

		// Token: 0x04002746 RID: 10054
		public float onlyIfJoyBelow = 1f;

		// Token: 0x04002747 RID: 10055
		public int takeToInventory;

		// Token: 0x04002748 RID: 10056
		public string takeToInventoryTempBuffer;
	}
}
