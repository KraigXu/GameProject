using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000618 RID: 1560
	public interface IBillGiver
	{
		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002A96 RID: 10902
		Map Map { get; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002A97 RID: 10903
		BillStack BillStack { get; }

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002A98 RID: 10904
		IEnumerable<IntVec3> IngredientStackCells { get; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002A99 RID: 10905
		string LabelShort { get; }

		// Token: 0x06002A9A RID: 10906
		bool CurrentlyUsableForBills();

		// Token: 0x06002A9B RID: 10907
		bool UsableForBillsAfterFueling();
	}
}
