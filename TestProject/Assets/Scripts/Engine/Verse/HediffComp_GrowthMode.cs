using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200025D RID: 605
	public class HediffComp_GrowthMode : HediffComp_SeverityPerDay
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0005E1FA File Offset: 0x0005C3FA
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0005E207 File Offset: 0x0005C407
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0005E214 File Offset: 0x0005C414
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0005E268 File Offset: 0x0005C468
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0005E2C7 File Offset: 0x0005C4C7
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(5000) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0005E300 File Offset: 0x0005C500
		protected override float SeverityChangePerDay()
		{
			switch (this.growthMode)
			{
			case HediffGrowthMode.Growing:
				return this.Props.severityPerDayGrowing * this.severityPerDayGrowingRandomFactor;
			case HediffGrowthMode.Stable:
				return 0f;
			case HediffGrowthMode.Remission:
				return this.Props.severityPerDayRemission * this.severityPerDayRemissionRandomFactor;
			default:
				throw new NotImplementedException("GrowthMode");
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0005E360 File Offset: 0x0005C560
		private void ChangeGrowthMode()
		{
			this.growthMode = (from x in (HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))
			where x != this.growthMode
			select x).RandomElement<HediffGrowthMode>();
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				switch (this.growthMode)
				{
				case HediffGrowthMode.Growing:
					Messages.Message("DiseaseGrowthModeChanged_Growing".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					return;
				case HediffGrowthMode.Stable:
					Messages.Message("DiseaseGrowthModeChanged_Stable".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NeutralEvent, true);
					return;
				case HediffGrowthMode.Remission:
					Messages.Message("DiseaseGrowthModeChanged_Remission".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0005E4C8 File Offset: 0x0005C6C8
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity >= base.Def.maxSeverity) ? " (reached max)" : ""));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		// Token: 0x04000C0D RID: 3085
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x04000C0E RID: 3086
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x04000C0F RID: 3087
		public HediffGrowthMode growthMode;

		// Token: 0x04000C10 RID: 3088
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x04000C11 RID: 3089
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}
