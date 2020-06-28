using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001009 RID: 4105
	public class StatPart_Quality : StatPart
	{
		// Token: 0x06006245 RID: 25157 RVA: 0x00221170 File Offset: 0x0021F370
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val <= 0f && !this.applyToNegativeValues)
			{
				return;
			}
			float num = val * this.QualityMultiplier(req.QualityCategory) - val;
			num = Mathf.Min(num, this.MaxGain(req.QualityCategory));
			val += num;
		}

		// Token: 0x06006246 RID: 25158 RVA: 0x002211C0 File Offset: 0x0021F3C0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && !this.applyToNegativeValues && req.Thing.GetStatValue(this.parentStat, true) <= 0f)
			{
				return null;
			}
			QualityCategory qc;
			if (req.HasThing && req.Thing.TryGetQuality(out qc))
			{
				string text = "StatsReport_QualityMultiplier".Translate() + ": x" + this.QualityMultiplier(qc).ToStringPercent();
				float num = this.MaxGain(qc);
				if (num < 999999f)
				{
					text += "\n    (" + "StatsReport_MaxGain".Translate() + ": " + num.ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, this.parentStat.toStringNumberSense) + ")";
				}
				return text;
			}
			return null;
		}

		// Token: 0x06006247 RID: 25159 RVA: 0x002212AC File Offset: 0x0021F4AC
		private float QualityMultiplier(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.factorAwful;
			case QualityCategory.Poor:
				return this.factorPoor;
			case QualityCategory.Normal:
				return this.factorNormal;
			case QualityCategory.Good:
				return this.factorGood;
			case QualityCategory.Excellent:
				return this.factorExcellent;
			case QualityCategory.Masterwork:
				return this.factorMasterwork;
			case QualityCategory.Legendary:
				return this.factorLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06006248 RID: 25160 RVA: 0x00221314 File Offset: 0x0021F514
		private float MaxGain(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.maxGainAwful;
			case QualityCategory.Poor:
				return this.maxGainPoor;
			case QualityCategory.Normal:
				return this.maxGainNormal;
			case QualityCategory.Good:
				return this.maxGainGood;
			case QualityCategory.Excellent:
				return this.maxGainExcellent;
			case QualityCategory.Masterwork:
				return this.maxGainMasterwork;
			case QualityCategory.Legendary:
				return this.maxGainLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04003BE4 RID: 15332
		private bool applyToNegativeValues;

		// Token: 0x04003BE5 RID: 15333
		private float factorAwful = 1f;

		// Token: 0x04003BE6 RID: 15334
		private float factorPoor = 1f;

		// Token: 0x04003BE7 RID: 15335
		private float factorNormal = 1f;

		// Token: 0x04003BE8 RID: 15336
		private float factorGood = 1f;

		// Token: 0x04003BE9 RID: 15337
		private float factorExcellent = 1f;

		// Token: 0x04003BEA RID: 15338
		private float factorMasterwork = 1f;

		// Token: 0x04003BEB RID: 15339
		private float factorLegendary = 1f;

		// Token: 0x04003BEC RID: 15340
		private float maxGainAwful = 9999999f;

		// Token: 0x04003BED RID: 15341
		private float maxGainPoor = 9999999f;

		// Token: 0x04003BEE RID: 15342
		private float maxGainNormal = 9999999f;

		// Token: 0x04003BEF RID: 15343
		private float maxGainGood = 9999999f;

		// Token: 0x04003BF0 RID: 15344
		private float maxGainExcellent = 9999999f;

		// Token: 0x04003BF1 RID: 15345
		private float maxGainMasterwork = 9999999f;

		// Token: 0x04003BF2 RID: 15346
		private float maxGainLegendary = 9999999f;
	}
}
