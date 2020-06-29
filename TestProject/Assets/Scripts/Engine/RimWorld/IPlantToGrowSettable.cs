using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public interface IPlantToGrowSettable
	{
		
		// (get) Token: 0x0600411B RID: 16667
		Map Map { get; }

		
		// (get) Token: 0x0600411C RID: 16668
		IEnumerable<IntVec3> Cells { get; }

		
		ThingDef GetPlantDefToGrow();

		
		void SetPlantDefToGrow(ThingDef plantDef);

		
		bool CanAcceptSowNow();
	}
}
