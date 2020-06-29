using System;
using Verse;

namespace RimWorld
{
	
	public class RecordWorker_TimeAsQuestLodger : RecordWorker
	{
		
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.HasExtraHomeFaction(null);
		}
	}
}
