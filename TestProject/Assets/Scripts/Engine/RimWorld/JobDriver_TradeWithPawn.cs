using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000678 RID: 1656
	public class JobDriver_TradeWithPawn : JobDriver
	{
		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002D28 RID: 11560 RVA: 0x000FF549 File Offset: 0x000FD749
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000FF556 File Offset: 0x000FD756
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Trader, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000FF578 File Offset: 0x000FD778
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.Trader.CanTradeNow);
			Toil trade = new Toil();
			trade.initAction = delegate
			{
				Pawn actor = trade.actor;
				if (this.Trader.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(actor, this.Trader, false));
				}
			};
			yield return trade;
			yield break;
		}
	}
}
