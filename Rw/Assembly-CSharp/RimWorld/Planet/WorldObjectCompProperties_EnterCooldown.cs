using System;

namespace RimWorld.Planet
{
	// Token: 0x020011B7 RID: 4535
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x060068CF RID: 26831 RVA: 0x00249A5B File Offset: 0x00247C5B
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		// Token: 0x04004140 RID: 16704
		public bool autoStartOnMapRemoved = true;

		// Token: 0x04004141 RID: 16705
		public float durationDays = 4f;
	}
}
