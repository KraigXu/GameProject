using System;

namespace RimWorld
{
	
	public class CompProperties_MoteEmitterProximityScan : CompProperties_MoteEmitter
	{
		
		public CompProperties_MoteEmitterProximityScan()
		{
			this.compClass = typeof(CompMoteEmitterProximityScan);
		}

		
		public float warmupPulseFadeInTime;

		
		public float warmupPulseSolidTime;

		
		public float warmupPulseFadeOutTime;
	}
}
