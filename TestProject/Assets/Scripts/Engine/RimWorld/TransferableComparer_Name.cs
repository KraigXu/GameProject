using System;

namespace RimWorld
{
	// Token: 0x02000F08 RID: 3848
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x06005E4A RID: 24138 RVA: 0x0020A79F File Offset: 0x0020899F
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
