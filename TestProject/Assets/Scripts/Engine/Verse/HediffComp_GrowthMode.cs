﻿using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_GrowthMode : HediffComp_SeverityPerDay
	{
		
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0005E1FA File Offset: 0x0005C3FA
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0005E207 File Offset: 0x0005C407
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(5000) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		
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

		
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity >= base.Def.maxSeverity) ? " (reached max)" : ""));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		
		private const int CheckGrowthModeChangeInterval = 5000;

		
		private const float GrowthModeChangeMtbDays = 100f;

		
		public HediffGrowthMode growthMode;

		
		private float severityPerDayGrowingRandomFactor = 1f;

		
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}
