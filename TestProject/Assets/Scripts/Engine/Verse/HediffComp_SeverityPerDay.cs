using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000278 RID: 632
	public class HediffComp_SeverityPerDay : HediffComp
	{
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0005FCAD File Offset: 0x0005DEAD
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0005FCBC File Offset: 0x0005DEBC
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(200))
			{
				float num = this.SeverityChangePerDay();
				num *= 0.00333333341f;
				severityAdjustment += num;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0005FCF7 File Offset: 0x0005DEF7
		protected virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0005FD04 File Offset: 0x0005DF04
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (this.props is HediffCompProperties_SeverityPerDay && this.Props.showHoursToRecover && this.SeverityChangePerDay() < 0f)
				{
					return Mathf.RoundToInt(this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay()) * 24f) + "LetterHour".Translate();
				}
				return null;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0005FD78 File Offset: 0x0005DF78
		public override string CompTipStringExtra
		{
			get
			{
				if (this.props is HediffCompProperties_SeverityPerDay && this.Props.showDaysToRecover && this.SeverityChangePerDay() < 0f)
				{
					return "DaysToRecover".Translate((this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay())).ToString("0.0"));
				}
				return null;
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0005FDE8 File Offset: 0x0005DFE8
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04000C4A RID: 3146
		protected const int SeverityUpdateInterval = 200;
	}
}
