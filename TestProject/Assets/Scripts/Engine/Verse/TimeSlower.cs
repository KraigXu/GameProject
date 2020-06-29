using System;
using UnityEngine;

namespace Verse
{
	
	public class TimeSlower
	{
		
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0002554D File Offset: 0x0002374D
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Mathf.Max(new int[]
			{
				Find.TickManager.TicksGame + 800
			});
		}

		
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Mathf.Max(this.forceNormalSpeedUntil, Find.TickManager.TicksGame + 240);
		}

		
		private int forceNormalSpeedUntil;

		
		private const int ForceTicksStandard = 800;

		
		private const int ForceTicksShort = 240;
	}
}
