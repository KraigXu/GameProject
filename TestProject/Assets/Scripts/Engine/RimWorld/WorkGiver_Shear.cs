using System;
using Verse;

namespace RimWorld
{
	
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x0010E3D8 File Offset: 0x0010C5D8
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
