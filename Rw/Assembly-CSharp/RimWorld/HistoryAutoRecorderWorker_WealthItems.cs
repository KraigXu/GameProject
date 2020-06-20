using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000931 RID: 2353
	public class HistoryAutoRecorderWorker_WealthItems : HistoryAutoRecorderWorker
	{
		// Token: 0x060037DB RID: 14299 RVA: 0x0012BAF8 File Offset: 0x00129CF8
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthItems;
				}
			}
			return num;
		}
	}
}
