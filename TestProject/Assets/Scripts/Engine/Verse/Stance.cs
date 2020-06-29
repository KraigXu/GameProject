using System;

namespace Verse
{
	
	public abstract class Stance : IExposable
	{
		
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0006F73B File Offset: 0x0006D93B
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		
		public virtual void StanceTick()
		{
		}

		
		public virtual void StanceDraw()
		{
		}

		
		public virtual void ExposeData()
		{
		}

		
		public Pawn_StanceTracker stanceTracker;
	}
}
