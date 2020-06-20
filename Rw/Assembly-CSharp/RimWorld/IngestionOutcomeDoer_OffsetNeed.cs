using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000897 RID: 2199
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x06003566 RID: 13670 RVA: 0x00123818 File Offset: 0x00121A18
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			if (pawn.needs == null)
			{
				return;
			}
			Need need = pawn.needs.TryGetNeed(this.need);
			if (need == null)
			{
				return;
			}
			float num = this.offset;
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num);
			need.CurLevel += num;
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x00123867 File Offset: 0x00121A67
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			string str = (this.offset >= 0f) ? "+" : string.Empty;
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, this.need.LabelCap, str + this.offset.ToStringPercent(), this.need.description, this.need.listPriority, null, null, false);
			yield break;
		}

		// Token: 0x04001D17 RID: 7447
		public NeedDef need;

		// Token: 0x04001D18 RID: 7448
		public float offset;

		// Token: 0x04001D19 RID: 7449
		public ChemicalDef toleranceChemical;
	}
}
