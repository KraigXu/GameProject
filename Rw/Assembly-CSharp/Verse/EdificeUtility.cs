using System;

namespace Verse
{
	// Token: 0x0200016E RID: 366
	public static class EdificeUtility
	{
		// Token: 0x06000A46 RID: 2630 RVA: 0x00037644 File Offset: 0x00035844
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
