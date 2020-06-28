using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B34 RID: 2868
	public class RecordWorker_TimeAsQuestLodger : RecordWorker
	{
		// Token: 0x0600439F RID: 17311 RVA: 0x0016C6D8 File Offset: 0x0016A8D8
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.HasExtraHomeFaction(null);
		}
	}
}
