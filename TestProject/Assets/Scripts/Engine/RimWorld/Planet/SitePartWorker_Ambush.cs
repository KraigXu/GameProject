using System;

namespace RimWorld.Planet
{
	
	public class SitePartWorker_Ambush : SitePartWorker
	{
		
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints *= 0.8f;
			return sitePartParams;
		}

		
		private const float ThreatPointsFactor = 0.8f;
	}
}
