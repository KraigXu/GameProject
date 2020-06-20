using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCB RID: 3531
	public static class TradeSession
	{
		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x060055A9 RID: 21929 RVA: 0x001C70FB File Offset: 0x001C52FB
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x060055AA RID: 21930 RVA: 0x001C7105 File Offset: 0x001C5305
		public static TradeCurrency TradeCurrency
		{
			get
			{
				return TradeSession.trader.TradeCurrency;
			}
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x001C7114 File Offset: 0x001C5314
		public static void SetupWith(ITrader newTrader, Pawn newPlayerNegotiator, bool giftMode)
		{
			if (!newTrader.CanTradeNow)
			{
				Log.Warning("Called SetupWith with a trader not willing to trade now.", false);
			}
			TradeSession.trader = newTrader;
			TradeSession.playerNegotiator = newPlayerNegotiator;
			TradeSession.giftMode = giftMode;
			TradeSession.deal = new TradeDeal();
			if (!giftMode && TradeSession.deal.cannotSellReasons.Count > 0)
			{
				Messages.Message("MessageCannotSellItemsReason".Translate() + TradeSession.deal.cannotSellReasons.ToCommaList(true), MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x060055AC RID: 21932 RVA: 0x001C7194 File Offset: 0x001C5394
		public static void Close()
		{
			TradeSession.trader = null;
		}

		// Token: 0x04002ED6 RID: 11990
		public static ITrader trader;

		// Token: 0x04002ED7 RID: 11991
		public static Pawn playerNegotiator;

		// Token: 0x04002ED8 RID: 11992
		public static TradeDeal deal;

		// Token: 0x04002ED9 RID: 11993
		public static bool giftMode;
	}
}
