using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F2 RID: 2546
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x06003C91 RID: 15505 RVA: 0x0013FFD4 File Offset: 0x0013E1D4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).Any<Building>();
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x0013FFEC File Offset: 0x0013E1EC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Building culprit;
			if (!ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).TryRandomElement(out culprit))
			{
				return false;
			}
			ShortCircuitUtility.DoShortCircuit(culprit);
			return true;
		}
	}
}
