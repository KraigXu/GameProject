using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class IngestionOutcomeDoer_OffsetPsyfocus : IngestionOutcomeDoer
	{
		
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(this.offset);
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (ModsConfig.RoyaltyActive)
			{
				string str = (this.offset >= 0f) ? "+" : string.Empty;
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "Psyfocus".Translate(), str + this.offset.ToStringPercent(), "PsyfocusDesc".Translate(), 1000, null, null, false);
			}
			yield break;
		}

		
		public float offset;
	}
}
