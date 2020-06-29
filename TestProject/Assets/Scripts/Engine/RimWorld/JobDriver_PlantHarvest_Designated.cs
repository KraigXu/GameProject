using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		
		// (get) Token: 0x06002D60 RID: 11616 RVA: 0x000FFE43 File Offset: 0x000FE043
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.HarvestPlant;
			}
		}
	}
}
