using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x000FA48D File Offset: 0x000F868D
		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
