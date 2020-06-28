using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D7A RID: 3450
	public class CompProperties_WakeUpDormant : CompProperties
	{
		// Token: 0x06005411 RID: 21521 RVA: 0x001C1248 File Offset: 0x001BF448
		public CompProperties_WakeUpDormant()
		{
			this.compClass = typeof(CompWakeUpDormant);
		}

		// Token: 0x04002E56 RID: 11862
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x04002E57 RID: 11863
		public float anyColonistCloseCheckRadius;

		// Token: 0x04002E58 RID: 11864
		public float wakeUpOnThingConstructedRadius = 3f;

		// Token: 0x04002E59 RID: 11865
		public bool wakeUpOnDamage = true;

		// Token: 0x04002E5A RID: 11866
		public bool onlyWakeUpSameFaction = true;

		// Token: 0x04002E5B RID: 11867
		public SoundDef wakeUpSound;

		// Token: 0x04002E5C RID: 11868
		public bool wakeUpIfAnyColonistClose;
	}
}
