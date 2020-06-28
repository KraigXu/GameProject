using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004E7 RID: 1255
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000D9030 File Offset: 0x000D7230
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000D9037 File Offset: 0x000D7237
		public override float ValueFor(Sample samp)
		{
			if (this.timeType == TimeType.RealtimeSeconds)
			{
				return Time.realtimeSinceStartup - samp.ParentStartRealTime;
			}
			if (this.timeType == TimeType.Ticks && Find.TickManager != null)
			{
				return (float)Find.TickManager.TicksGame - samp.ParentStartTick;
			}
			return 0f;
		}

		// Token: 0x04001605 RID: 5637
		public TimeType timeType;
	}
}
