using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000895 RID: 2197
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x0600355F RID: 13663 RVA: 0x00123741 File Offset: 0x00121941
		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		// Token: 0x06003560 RID: 13664
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		// Token: 0x06003561 RID: 13665 RVA: 0x00123758 File Offset: 0x00121958
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}

		// Token: 0x04001D11 RID: 7441
		public float chance = 1f;

		// Token: 0x04001D12 RID: 7442
		public bool doToGeneratedPawnIfAddicted;
	}
}
