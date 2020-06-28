using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000561 RID: 1377
	public class MentalStateWorker_BingingDrug : MentalStateWorker
	{
		// Token: 0x06002724 RID: 10020 RVA: 0x000E5104 File Offset: 0x000E3304
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], this.def.drugCategory))
				{
					return true;
				}
				if (this.def.drugCategory == DrugCategory.Hard && AddictionUtility.CanBingeOnNow(pawn, allDefsListForReading[i], DrugCategory.Social))
				{
					return true;
				}
			}
			return false;
		}
	}
}
