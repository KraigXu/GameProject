using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_SpawnSubplant : CompProperties
	{
		
		public CompProperties_SpawnSubplant()
		{
			this.compClass = typeof(CompSpawnSubplant);
		}

		
		public ThingDef subplant;

		
		public SoundDef spawnSound;
	}
}
