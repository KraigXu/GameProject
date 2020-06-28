using System;

namespace RimWorld
{
	// Token: 0x02000F0A RID: 3850
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x06005E4E RID: 24142 RVA: 0x0020A7B4 File Offset: 0x002089B4
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06005E4F RID: 24143 RVA: 0x0020A7D8 File Offset: 0x002089D8
		private int GetValueFor(Transferable t)
		{
			QualityCategory result;
			if (!t.AnyThing.TryGetQuality(out result))
			{
				return -1;
			}
			return (int)result;
		}
	}
}
