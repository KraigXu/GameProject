using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E84 RID: 3716
	public static class PageUtility
	{
		// Token: 0x06005A70 RID: 23152 RVA: 0x001EAE34 File Offset: 0x001E9034
		public static Page StitchedPages(IEnumerable<Page> pages)
		{
			List<Page> list = pages.ToList<Page>();
			if (list.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					list[i].prev = list[i - 1];
				}
				if (i < list.Count - 1)
				{
					list[i].next = list[i + 1];
				}
			}
			return list[0];
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x001EAEA3 File Offset: 0x001E90A3
		public static void InitGameStart()
		{
			LongEventHandler.QueueLongEvent(delegate
			{
				Find.GameInitData.PrepForMapGen();
				Find.GameInitData.startedFromEntry = true;
				Find.Scenario.PreMapGenerate();
			}, "Play", "GeneratingMap", true, null, true);
		}
	}
}
