using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	
	public static class RockNoises
	{
		
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

		
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		
		public static List<RockNoises.RockNoise> rockNoises;

		
		private const float RockNoiseFreq = 0.005f;

		
		public class RockNoise
		{
			
			public ThingDef rockDef;

			
			public ModuleBase noise;
		}
	}
}
