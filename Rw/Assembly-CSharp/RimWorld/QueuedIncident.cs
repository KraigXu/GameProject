using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CB RID: 2507
	public class QueuedIncident : IExposable
	{
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x0013BAE6 File Offset: 0x00139CE6
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003BD4 RID: 15316 RVA: 0x0013BAEE File Offset: 0x00139CEE
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x0013BAF6 File Offset: 0x00139CF6
		public int RetryDurationTicks
		{
			get
			{
				return this.retryDurationTicks;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x0013BAFE File Offset: 0x00139CFE
		public bool TriedToFire
		{
			get
			{
				return this.triedToFire;
			}
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0013BB06 File Offset: 0x00139D06
		public QueuedIncident()
		{
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0013BB15 File Offset: 0x00139D15
		public QueuedIncident(FiringIncident firingInc, int fireTick, int retryDurationTicks = 0)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
			this.retryDurationTicks = retryDurationTicks;
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x0013BB3C File Offset: 0x00139D3C
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
			Scribe_Values.Look<int>(ref this.retryDurationTicks, "retryDurationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.triedToFire, "triedToFire", false, false);
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x0013BB94 File Offset: 0x00139D94
		public void Notify_TriedToFire()
		{
			this.triedToFire = true;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x0013BB9D File Offset: 0x00139D9D
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}

		// Token: 0x0400235F RID: 9055
		private FiringIncident firingInc;

		// Token: 0x04002360 RID: 9056
		private int fireTick = -1;

		// Token: 0x04002361 RID: 9057
		private int retryDurationTicks;

		// Token: 0x04002362 RID: 9058
		private bool triedToFire;

		// Token: 0x04002363 RID: 9059
		public const int RetryIntervalTicks = 833;
	}
}
