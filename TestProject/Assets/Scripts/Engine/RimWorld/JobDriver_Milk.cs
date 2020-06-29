using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x000FA476 File Offset: 0x000F8676
		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
