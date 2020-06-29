using System;
using Verse;

namespace RimWorld
{
	
	public class WorkGiver_Milk : WorkGiver_GatherAnimalBodyResources
	{
		
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x0010E3C9 File Offset: 0x0010C5C9
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Milk;
			}
		}

		
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
