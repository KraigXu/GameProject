using System;
using Verse;

namespace RimWorld
{
	
	public class QueuedIncident : IExposable
	{
		
		// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x0013BAE6 File Offset: 0x00139CE6
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		
		// (get) Token: 0x06003BD4 RID: 15316 RVA: 0x0013BAEE File Offset: 0x00139CEE
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x0013BAF6 File Offset: 0x00139CF6
		public int RetryDurationTicks
		{
			get
			{
				return this.retryDurationTicks;
			}
		}

		
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x0013BAFE File Offset: 0x00139CFE
		public bool TriedToFire
		{
			get
			{
				return this.triedToFire;
			}
		}

		
		public QueuedIncident()
		{
		}

		
		public QueuedIncident(FiringIncident firingInc, int fireTick, int retryDurationTicks = 0)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
			this.retryDurationTicks = retryDurationTicks;
		}

		
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
			Scribe_Values.Look<int>(ref this.retryDurationTicks, "retryDurationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.triedToFire, "triedToFire", false, false);
		}

		
		public void Notify_TriedToFire()
		{
			this.triedToFire = true;
		}

		
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}

		
		private FiringIncident firingInc;

		
		private int fireTick = -1;

		
		private int retryDurationTicks;

		
		private bool triedToFire;

		
		public const int RetryIntervalTicks = 833;
	}
}
