using System;

namespace RimWorld
{
	// Token: 0x02000F07 RID: 3847
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x06005E48 RID: 24136 RVA: 0x0020A768 File Offset: 0x00208968
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true));
		}
	}
}
