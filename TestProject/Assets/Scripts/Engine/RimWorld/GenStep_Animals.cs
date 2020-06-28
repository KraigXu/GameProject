using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A46 RID: 2630
	public class GenStep_Animals : GenStep
	{
		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06003E2E RID: 15918 RVA: 0x00147866 File Offset: 0x00145A66
		public override int SeedPart
		{
			get
			{
				return 1298760307;
			}
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x00147870 File Offset: 0x00145A70
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			while (!map.wildAnimalSpawner.AnimalEcosystemFull)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.", false);
					return;
				}
				IntVec3 loc = RCellFinder.RandomAnimalSpawnCell_MapGen(map);
				if (!map.wildAnimalSpawner.SpawnRandomWildAnimalAt(loc))
				{
					break;
				}
			}
		}
	}
}
