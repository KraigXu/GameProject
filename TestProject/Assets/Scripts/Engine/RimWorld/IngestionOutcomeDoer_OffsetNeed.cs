using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		
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

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			string str = (this.offset >= 0f) ? "+" : string.Empty;
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, this.need.LabelCap, str + this.offset.ToStringPercent(), this.need.description, this.need.listPriority, null, null, false);
			yield break;
		}

		
		public NeedDef need;

		
		public float offset;

		
		public ChemicalDef toleranceChemical;
	}
}
