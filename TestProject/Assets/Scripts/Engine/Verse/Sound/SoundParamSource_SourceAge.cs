using System;
using UnityEngine;

namespace Verse.Sound
{
	
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000D9030 File Offset: 0x000D7230
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		
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

		
		public TimeType timeType;
	}
}
