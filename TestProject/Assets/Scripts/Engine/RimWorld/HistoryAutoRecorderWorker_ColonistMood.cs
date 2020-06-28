using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092D RID: 2349
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		// Token: 0x060037D3 RID: 14291 RVA: 0x0012BA1C File Offset: 0x00129C1C
		public override float PullRecord()
		{
			List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			if (!allMaps_FreeColonists.Any<Pawn>())
			{
				return 0f;
			}
			return (from x in allMaps_FreeColonists
			where x.needs.mood != null
			select x).Average((Pawn x) => x.needs.mood.CurLevel * 100f);
		}
	}
}
