using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000930 RID: 2352
	public class HistoryAutoRecorderWorker_WealthBuildings : HistoryAutoRecorderWorker
	{
		// Token: 0x060037D9 RID: 14297 RVA: 0x0012BAA8 File Offset: 0x00129CA8
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthBuildings;
				}
			}
			return num;
		}
	}
}
