using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public interface IBillGiver
	{
		
		// (get) Token: 0x06002A96 RID: 10902
		Map Map { get; }

		
		// (get) Token: 0x06002A97 RID: 10903
		BillStack BillStack { get; }

		
		// (get) Token: 0x06002A98 RID: 10904
		IEnumerable<IntVec3> IngredientStackCells { get; }

		
		// (get) Token: 0x06002A99 RID: 10905
		string LabelShort { get; }

		
		bool CurrentlyUsableForBills();

		
		bool UsableForBillsAfterFueling();
	}
}
