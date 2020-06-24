using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Spirit
{
	// Token: 0x020001AC RID: 428
	public static class RockNoises
	{
		// Token: 0x06000BF8 RID: 3064 RVA: 0x00043F20 File Offset: 0x00042120
		public static void Init(Map map)
		{
			RockNoises.rockNoises = new List<RockNoises.RockNoise>();
			foreach (ThingDef rockDef in Find.World.NaturalRockTypesIn(map.Tile))
			{
				RockNoises.RockNoise rockNoise = new RockNoises.RockNoise();
				rockNoise.rockDef = rockDef;
				rockNoise.noise = new Perlin(0.004999999888241291, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.Medium);
				RockNoises.rockNoises.Add(rockNoise);
				NoiseDebugUI.StoreNoiseRender(rockNoise.noise, rockNoise.rockDef + " score", map.Size.ToIntVec2);
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00043FF4 File Offset: 0x000421F4
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04000990 RID: 2448
		public static List<RockNoises.RockNoise> rockNoises;

		// Token: 0x04000991 RID: 2449
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x020013CD RID: 5069
		public class RockNoise
		{
			// Token: 0x04004B40 RID: 19264
			public ThingDef rockDef;

			// Token: 0x04004B41 RID: 19265
			public ModuleBase noise;
		}
	}
}
