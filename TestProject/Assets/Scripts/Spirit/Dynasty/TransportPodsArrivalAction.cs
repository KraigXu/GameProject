using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001268 RID: 4712
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x06006E3C RID: 28220 RVA: 0x002584FF File Offset: 0x002566FF
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x06006E3D RID: 28221 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x06006E3E RID: 28222
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x06006E3F RID: 28223 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}
	}
}
