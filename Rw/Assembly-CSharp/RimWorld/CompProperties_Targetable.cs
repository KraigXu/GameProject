using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087B RID: 2171
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x0600353D RID: 13629 RVA: 0x001230B7 File Offset: 0x001212B7
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		// Token: 0x04001C93 RID: 7315
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x04001C94 RID: 7316
		public bool fleshCorpsesOnly;

		// Token: 0x04001C95 RID: 7317
		public bool nonDessicatedCorpsesOnly;

		// Token: 0x04001C96 RID: 7318
		public bool nonDownedPawnOnly;

		// Token: 0x04001C97 RID: 7319
		public bool ignoreQuestLodgerPawns;

		// Token: 0x04001C98 RID: 7320
		public bool ignorePlayerFactionPawns;

		// Token: 0x04001C99 RID: 7321
		public ThingDef moteOnTarget;

		// Token: 0x04001C9A RID: 7322
		public ThingDef moteConnecting;
	}
}
