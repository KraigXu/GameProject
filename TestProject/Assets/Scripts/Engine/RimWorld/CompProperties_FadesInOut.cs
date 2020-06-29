using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_FadesInOut : CompProperties
	{
		
		public CompProperties_FadesInOut()
		{
			this.compClass = typeof(CompFadesInOut);
		}

		
		public float fadeInSecs;

		
		public float fadeOutSecs;

		
		public float solidTimeSecs;
	}
}
