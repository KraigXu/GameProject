using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200091C RID: 2332
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		// Token: 0x06003764 RID: 14180
		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
