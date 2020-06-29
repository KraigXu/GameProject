using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_SendSignalOnCountdown : CompProperties
	{
		
		public CompProperties_SendSignalOnCountdown()
		{
			this.compClass = typeof(CompSendSignalOnCountdown);
		}

		
		public SimpleCurve countdownCurveTicks;

		
		public string signalTag;
	}
}
