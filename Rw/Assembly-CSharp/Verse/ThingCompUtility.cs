using System;

namespace Verse
{
	// Token: 0x02000325 RID: 805
	public static class ThingCompUtility
	{
		// Token: 0x06001792 RID: 6034 RVA: 0x00085C14 File Offset: 0x00083E14
		public static T TryGetComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return default(T);
			}
			return thingWithComps.GetComp<T>();
		}
	}
}
