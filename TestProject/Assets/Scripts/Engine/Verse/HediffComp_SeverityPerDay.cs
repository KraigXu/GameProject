using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	
	public class HediffComp_SeverityPerDay : HediffComp
	{
		
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0005FCAD File Offset: 0x0005DEAD
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		
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

		
		protected virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		
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

		
		protected const int SeverityUpdateInterval = 200;
	}
}
