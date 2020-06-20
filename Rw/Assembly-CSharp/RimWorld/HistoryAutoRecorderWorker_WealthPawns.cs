using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000932 RID: 2354
	public class HistoryAutoRecorderWorker_WealthPawns : HistoryAutoRecorderWorker
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x0012BB48 File Offset: 0x00129D48
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthPawns;
				}
			}
			return num;
		}
	}
}
