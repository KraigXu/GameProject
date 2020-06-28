using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200011C RID: 284
	public class TimeSlower
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0002554D File Offset: 0x0002374D
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00025561 File Offset: 0x00023761
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Mathf.Max(new int[]
			{
				Find.TickManager.TicksGame + 800
			});
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00025587 File Offset: 0x00023787
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Mathf.Max(this.forceNormalSpeedUntil, Find.TickManager.TicksGame + 240);
		}

		// Token: 0x04000727 RID: 1831
		private int forceNormalSpeedUntil;

		// Token: 0x04000728 RID: 1832
		private const int ForceTicksStandard = 800;

		// Token: 0x04000729 RID: 1833
		private const int ForceTicksShort = 240;
	}
}
