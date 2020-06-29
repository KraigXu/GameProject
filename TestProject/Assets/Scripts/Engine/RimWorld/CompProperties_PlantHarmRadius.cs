using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_PlantHarmRadius : CompProperties
	{
		
		public CompProperties_PlantHarmRadius()
		{
			this.compClass = typeof(CompPlantHarmRadius);
		}

		
		public float harmFrequencyPerArea = 0.011f;

		
		public float leaflessPlantKillChance = 0.05f;

		
		public SimpleCurve radiusPerDayCurve;
	}
}
