using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF4 RID: 3060
	public class PredatorThreat : IExposable
	{
		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x060048C6 RID: 18630 RVA: 0x0018C122 File Offset: 0x0018A322
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x0018C14E File Offset: 0x0018A34E
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		// Token: 0x040029A8 RID: 10664
		public Pawn predator;

		// Token: 0x040029A9 RID: 10665
		public int lastAttackTicks;

		// Token: 0x040029AA RID: 10666
		private const int ExpireAfterTicks = 600;
	}
}
