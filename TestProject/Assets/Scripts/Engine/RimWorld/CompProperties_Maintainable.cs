using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Maintainable : CompProperties
	{
		
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		
		public int ticksHealthy = 1000;

		
		public int ticksNeedsMaintenance = 1000;

		
		public int damagePerTickRare = 10;
	}
}
