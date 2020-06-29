using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_SendSignalOnPawnProximity : CompProperties
	{
		
		public CompProperties_SendSignalOnPawnProximity()
		{
			this.compClass = typeof(CompSendSignalOnPawnProximity);
		}

		
		public bool triggerOnPawnInRoom;

		
		public float radius;

		
		public int enableAfterTicks;

		
		public bool onlyHumanlike;

		
		public string signalTag;
	}
}
