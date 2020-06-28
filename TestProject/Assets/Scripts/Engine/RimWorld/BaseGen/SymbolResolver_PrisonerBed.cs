using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010B9 RID: 4281
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		// Token: 0x06006531 RID: 25905 RVA: 0x00234E6C File Offset: 0x0023306C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			Action<Thing> prevPostThingSpawn = resolveParams.postThingSpawn;
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				if (prevPostThingSpawn != null)
				{
					prevPostThingSpawn(x);
				}
				Building_Bed building_Bed = x as Building_Bed;
				if (building_Bed != null)
				{
					building_Bed.ForPrisoners = true;
				}
			};
			BaseGen.symbolStack.Push("bed", resolveParams, null);
		}
	}
}
