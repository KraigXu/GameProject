using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_RadialPulse : CompProperties
	{
		
		public CompProperties_RadialPulse()
		{
			this.compClass = typeof(CompRadialPulse);
		}

		
		public int ticksBetweenPulses = 300;

		
		public int ticksPerPulse = 60;

		
		public Color color;

		
		public float radius;
	}
}
