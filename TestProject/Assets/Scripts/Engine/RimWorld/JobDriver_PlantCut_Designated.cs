using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000685 RID: 1669
	public class JobDriver_PlantCut_Designated : JobDriver_PlantCut
	{
		// Token: 0x17000887 RID: 2183
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
