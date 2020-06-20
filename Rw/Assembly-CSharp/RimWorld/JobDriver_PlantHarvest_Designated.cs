using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000684 RID: 1668
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		// Token: 0x17000886 RID: 2182
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
