using System;

namespace RimWorld.Planet
{
	// Token: 0x02001263 RID: 4707
	public class SitePartWorker_Ambush : SitePartWorker
	{
		// Token: 0x06006E2F RID: 28207 RVA: 0x00267B55 File Offset: 0x00265D55
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints *= 0.8f;
			return sitePartParams;
		}

		// Token: 0x0400441A RID: 17434
		private const float ThreatPointsFactor = 0.8f;
	}
}
