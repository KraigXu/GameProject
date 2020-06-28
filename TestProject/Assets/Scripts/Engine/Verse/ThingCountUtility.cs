using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000419 RID: 1049
	public static class ThingCountUtility
	{
		// Token: 0x06001F64 RID: 8036 RVA: 0x000C103C File Offset: 0x000BF23C
		public static int CountOf(List<ThingCount> list, Thing thing)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					num += list[i].Count;
				}
			}
			return num;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000C1084 File Offset: 0x000BF284
		public static void AddToList(List<ThingCount> list, Thing thing, int countToAdd)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					list[i] = list[i].WithCount(list[i].Count + countToAdd);
					return;
				}
			}
			list.Add(new ThingCount(thing, countToAdd));
		}
	}
}
