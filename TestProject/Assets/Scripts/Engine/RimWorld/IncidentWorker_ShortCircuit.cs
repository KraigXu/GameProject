using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).Any<Building>();
		}

		
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
