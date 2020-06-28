using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000898 RID: 2200
	public class IngestionOutcomeDoer_OffsetPsyfocus : IngestionOutcomeDoer
	{
		// Token: 0x06003569 RID: 13673 RVA: 0x0012387F File Offset: 0x00121A7F
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(this.offset);
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x00123897 File Offset: 0x00121A97
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (ModsConfig.RoyaltyActive)
			{
				string str = (this.offset >= 0f) ? "+" : string.Empty;
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "Psyfocus".Translate(), str + this.offset.ToStringPercent(), "PsyfocusDesc".Translate(), 1000, null, null, false);
			}
			yield break;
		}

		// Token: 0x04001D1A RID: 7450
		public float offset;
	}
}
