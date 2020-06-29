using System;
using Verse;

namespace RimWorld
{
	
	public class PredatorThreat : IExposable
	{
		
		// (get) Token: 0x060048C6 RID: 18630 RVA: 0x0018C122 File Offset: 0x0018A322
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		
		public Pawn predator;

		
		public int lastAttackTicks;

		
		private const int ExpireAfterTicks = 600;
	}
}
