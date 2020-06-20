using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000C02 RID: 3074
	public static class OutpostSitePartUtility
	{
		// Token: 0x06004911 RID: 18705 RVA: 0x0018D0AB File Offset: 0x0018B2AB
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
