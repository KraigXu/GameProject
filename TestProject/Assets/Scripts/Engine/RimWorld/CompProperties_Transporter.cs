using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087D RID: 2173
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x0600353F RID: 13631 RVA: 0x0012312A File Offset: 0x0012132A
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		// Token: 0x04001CA0 RID: 7328
		public float massCapacity = 150f;

		// Token: 0x04001CA1 RID: 7329
		public float restEffectiveness;

		// Token: 0x04001CA2 RID: 7330
		public bool max1PerGroup;

		// Token: 0x04001CA3 RID: 7331
		public bool canChangeAssignedThingsAfterStarting;

		// Token: 0x04001CA4 RID: 7332
		public bool showOverallStats = true;

		// Token: 0x04001CA5 RID: 7333
		public SoundDef pawnLoadedSound;

		// Token: 0x04001CA6 RID: 7334
		public SoundDef pawnExitSound;
	}
}
