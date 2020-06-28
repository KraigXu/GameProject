using System;

namespace Verse
{
	// Token: 0x020001DE RID: 478
	public static class ThingIDMaker
	{
		// Token: 0x06000D84 RID: 3460 RVA: 0x0004D328 File Offset: 0x0004B528
		public static void GiveIDTo(Thing t)
		{
			if (!t.def.HasThingIDNumber)
			{
				return;
			}
			if (t.thingIDNumber != -1)
			{
				Log.Error(string.Concat(new object[]
				{
					"Giving ID to ",
					t,
					" which already has id ",
					t.thingIDNumber
				}), false);
			}
			t.thingIDNumber = Find.UniqueIDsManager.GetNextThingID();
		}
	}
}
