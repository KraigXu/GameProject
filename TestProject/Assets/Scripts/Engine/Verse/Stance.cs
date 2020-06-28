using System;

namespace Verse
{
	// Token: 0x020002A2 RID: 674
	public abstract class Stance : IExposable
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0006F73B File Offset: 0x0006D93B
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void StanceTick()
		{
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x04000D17 RID: 3351
		public Pawn_StanceTracker stanceTracker;
	}
}
