using System;

namespace RimWorld
{
	// Token: 0x02000864 RID: 2148
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x06003505 RID: 13573 RVA: 0x00122717 File Offset: 0x00120917
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		// Token: 0x04001C38 RID: 7224
		public float storedEnergyMax = 1000f;

		// Token: 0x04001C39 RID: 7225
		public float efficiency = 0.5f;
	}
}
