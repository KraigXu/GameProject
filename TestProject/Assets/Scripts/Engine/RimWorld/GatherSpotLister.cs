using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000D11 RID: 3345
	public class GatherSpotLister
	{
		// Token: 0x0600515E RID: 20830 RVA: 0x001B4803 File Offset: 0x001B2A03
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x001B481F File Offset: 0x001B2A1F
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}

		// Token: 0x04002D0B RID: 11531
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();
	}
}
