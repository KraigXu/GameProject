using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092E RID: 2350
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x060037D5 RID: 14293 RVA: 0x0012BA8E File Offset: 0x00129C8E
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
		}
	}
}
