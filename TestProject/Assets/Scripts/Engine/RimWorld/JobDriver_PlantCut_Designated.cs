using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_PlantCut_Designated : JobDriver_PlantCut
	{
		
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x000FFE52 File Offset: 0x000FE052
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.CutPlant;
			}
		}
	}
}
