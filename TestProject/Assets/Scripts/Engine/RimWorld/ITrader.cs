using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public interface ITrader
	{
		
		// (get) Token: 0x060054E1 RID: 21729
		TraderKindDef TraderKind { get; }

		
		// (get) Token: 0x060054E2 RID: 21730
		IEnumerable<Thing> Goods { get; }

		
		// (get) Token: 0x060054E3 RID: 21731
		int RandomPriceFactorSeed { get; }

		
		// (get) Token: 0x060054E4 RID: 21732
		string TraderName { get; }

		
		// (get) Token: 0x060054E5 RID: 21733
		bool CanTradeNow { get; }

		
		// (get) Token: 0x060054E6 RID: 21734
		float TradePriceImprovementOffsetForPlayer { get; }

		
		// (get) Token: 0x060054E7 RID: 21735
		Faction Faction { get; }

		
		// (get) Token: 0x060054E8 RID: 21736
		TradeCurrency TradeCurrency { get; }

		
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
