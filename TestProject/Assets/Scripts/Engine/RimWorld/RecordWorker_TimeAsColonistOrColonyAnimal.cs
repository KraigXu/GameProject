using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B33 RID: 2867
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x0600439D RID: 17309 RVA: 0x0016C6B5 File Offset: 0x0016A8B5
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && !pawn.HasExtraHomeFaction(null);
		}
	}
}
