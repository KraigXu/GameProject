using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA8 RID: 3496
	public interface ITrader
	{
		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x060054E1 RID: 21729
		TraderKindDef TraderKind { get; }

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x060054E2 RID: 21730
		IEnumerable<Thing> Goods { get; }

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x060054E3 RID: 21731
		int RandomPriceFactorSeed { get; }

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x060054E4 RID: 21732
		string TraderName { get; }

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x060054E5 RID: 21733
		bool CanTradeNow { get; }

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x060054E6 RID: 21734
		float TradePriceImprovementOffsetForPlayer { get; }

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x060054E7 RID: 21735
		Faction Faction { get; }

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x060054E8 RID: 21736
		TradeCurrency TradeCurrency { get; }

		// Token: 0x060054E9 RID: 21737
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		// Token: 0x060054EA RID: 21738
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		// Token: 0x060054EB RID: 21739
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
