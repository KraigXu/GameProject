using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088C RID: 2188
	public class GasProperties
	{
		// Token: 0x04001CC3 RID: 7363
		public bool blockTurretTracking;

		// Token: 0x04001CC4 RID: 7364
		public float accuracyPenalty;

		// Token: 0x04001CC5 RID: 7365
		public FloatRange expireSeconds = new FloatRange(30f, 30f);

		// Token: 0x04001CC6 RID: 7366
		public float rotationSpeed;
	}
}
