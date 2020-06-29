using System;
using Verse;

namespace RimWorld
{
	
	public class GenStep_Plants : GenStep
	{
		
		// (get) Token: 0x06003E5A RID: 15962 RVA: 0x00149138 File Offset: 0x00147338
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		
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

		
		private const float ChanceToSkip = 0.001f;
	}
}
