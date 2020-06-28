﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000A54 RID: 2644
	public class GenStep_RockChunks : GenStep
	{
		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0014B6C6 File Offset: 0x001498C6
		public override int SeedPart
		{
			get
			{
				return 1898758716;
			}
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0014B6D0 File Offset: 0x001498D0
		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.TileInfo.WaterCovered)
			{
				return;
			}
			this.freqFactorNoise = new Perlin(0.014999999664723873, 2.0, 0.5, 6, Rand.Range(0, 999999), QualityMode.Medium);
			this.freqFactorNoise = new ScaleBias(1.0, 1.0, this.freqFactorNoise);
			NoiseDebugUI.StoreNoiseRender(this.freqFactorNoise, "rock_chunks_freq_factor");
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float num = 0.006f * this.freqFactorNoise.GetValue(intVec);
				if (elevation[intVec] < 0.55f && Rand.Value < num)
				{
					this.GrowLowRockFormationFrom(intVec, map);
				}
			}
			this.freqFactorNoise = null;
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x0014B7C8 File Offset: 0x001499C8
		private void GrowLowRockFormationFrom(IntVec3 root, Map map)
		{
			ThingDef filth_RubbleRock = ThingDefOf.Filth_RubbleRock;
			ThingDef mineableThing = Find.World.NaturalRockTypesIn(map.Tile).RandomElement<ThingDef>().building.mineableThing;
			Rot4 random = Rot4.Random;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			IntVec3 intVec = root;
			for (;;)
			{
				Rot4 random2 = Rot4.Random;
				if (!(random2 == random))
				{
					intVec += random2.FacingCell;
					if (!intVec.InBounds(map) || intVec.GetEdifice(map) != null || intVec.GetFirstItem(map) != null)
					{
						break;
					}
					if (elevation[intVec] > 0.55f)
					{
						return;
					}
					if (!map.terrainGrid.TerrainAt(intVec).affordances.Contains(TerrainAffordanceDefOf.Heavy))
					{
						return;
					}
					GenSpawn.Spawn(mineableThing, intVec, map, WipeMode.Vanish);
					foreach (IntVec3 b in GenAdj.AdjacentCellsAndInside)
					{
						if (Rand.Value < 0.5f)
						{
							IntVec3 c = intVec + b;
							if (c.InBounds(map))
							{
								bool flag = false;
								List<Thing> thingList = c.GetThingList(map);
								for (int j = 0; j < thingList.Count; j++)
								{
									Thing thing = thingList[j];
									if (thing.def.category != ThingCategory.Plant && thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Pawn)
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									FilthMaker.TryMakeFilth(c, map, filth_RubbleRock, 1, FilthSourceFlags.None);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400246C RID: 9324
		private ModuleBase freqFactorNoise;

		// Token: 0x0400246D RID: 9325
		private const float ThreshLooseRock = 0.55f;

		// Token: 0x0400246E RID: 9326
		private const float PlaceProbabilityPerCell = 0.006f;

		// Token: 0x0400246F RID: 9327
		private const float RubbleProbability = 0.5f;
	}
}
