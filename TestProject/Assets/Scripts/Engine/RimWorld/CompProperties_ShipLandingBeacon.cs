using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_ShipLandingBeacon : CompProperties
	{
		
		public CompProperties_ShipLandingBeacon()
		{
			this.compClass = typeof(CompShipLandingBeacon);
		}

		
		public FloatRange edgeLengthRange;
	}
}
