using System;

namespace RimWorld
{
	// Token: 0x02000DCE RID: 3534
	public static class TradeabilityUtility
	{
		// Token: 0x060055CB RID: 21963 RVA: 0x001C76F6 File Offset: 0x001C58F6
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x001C7702 File Offset: 0x001C5902
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
