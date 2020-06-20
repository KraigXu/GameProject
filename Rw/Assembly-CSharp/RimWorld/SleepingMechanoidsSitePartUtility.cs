using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000C05 RID: 3077
	public static class SleepingMechanoidsSitePartUtility
	{
		// Token: 0x0600491C RID: 18716 RVA: 0x0018D0AB File Offset: 0x0018B2AB
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
