using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_SnowExpand : CompProperties
	{
		
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		
		public int expandInterval = 500;

		
		public float addAmount = 0.12f;

		
		public float maxRadius = 55f;
	}
}
