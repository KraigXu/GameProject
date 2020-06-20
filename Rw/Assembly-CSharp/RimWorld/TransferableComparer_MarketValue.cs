using System;

namespace RimWorld
{
	// Token: 0x02000F06 RID: 3846
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x06005E46 RID: 24134 RVA: 0x0020A73C File Offset: 0x0020893C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
