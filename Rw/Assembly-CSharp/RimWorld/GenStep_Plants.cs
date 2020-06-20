using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A4E RID: 2638
	public class GenStep_Plants : GenStep
	{
		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003E5A RID: 15962 RVA: 0x00149138 File Offset: 0x00147338
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00149140 File Offset: 0x00147340
		public override void Generate(Map map, GenStepParams parms)
		{
			map.regionAndRoomUpdater.Enabled = false;
			float currentPlantDensity = map.wildPlantSpawner.CurrentPlantDensity;
			float currentWholeMapNumDesiredPlants = map.wildPlantSpawner.CurrentWholeMapNumDesiredPlants;
			foreach (IntVec3 c in map.cellsInRandomOrder.GetAll())
			{
				if (!Rand.Chance(0.001f))
				{
					map.wildPlantSpawner.CheckSpawnWildPlantAt(c, currentPlantDensity, currentWholeMapNumDesiredPlants, true);
				}
			}
			map.regionAndRoomUpdater.Enabled = true;
		}

		// Token: 0x04002457 RID: 9303
		private const float ChanceToSkip = 0.001f;
	}
}
