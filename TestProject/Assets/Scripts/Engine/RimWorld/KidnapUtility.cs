using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF2 RID: 3058
	public static class KidnapUtility
	{
		// Token: 0x060048BE RID: 18622 RVA: 0x0018BF38 File Offset: 0x0018A138
		public static bool IsKidnapped(this Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
