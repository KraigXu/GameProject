using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000933 RID: 2355
	public class HistoryAutoRecorderWorker_WealthTotal : HistoryAutoRecorderWorker
	{
		// Token: 0x060037DF RID: 14303 RVA: 0x0012BB98 File Offset: 0x00129D98
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthTotal;
				}
			}
			return num;
		}
	}
}
