using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092F RID: 2351
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x060037D7 RID: 14295 RVA: 0x0012BA9B File Offset: 0x00129C9B
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
