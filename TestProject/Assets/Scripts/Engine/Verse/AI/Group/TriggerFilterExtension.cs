using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020005EE RID: 1518
	public static class TriggerFilterExtension
	{
		// Token: 0x060029F9 RID: 10745 RVA: 0x000F5DFF File Offset: 0x000F3FFF
		public static Trigger WithFilter(this Trigger t, TriggerFilter f)
		{
			if (t.filters == null)
			{
				t.filters = new List<TriggerFilter>();
			}
			t.filters.Add(f);
			return t;
		}
	}
}
