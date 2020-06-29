using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityPerDay
	{
		
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0005FA54 File Offset: 0x0005DC54
		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)this.props;
			}
		}

		
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x0005FA61 File Offset: 0x0005DC61
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (this.FullyImmune)
				{
					return "DevelopedImmunityLower".Translate();
				}
				return null;
			}
		}

		
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0005FA7C File Offset: 0x0005DC7C
		public override string CompTipStringExtra
		{
			get
			{
				if (base.Def.PossibleToDevelopImmunityNaturally() && !this.FullyImmune)
				{
					return "Immunity".Translate() + ": " + (Mathf.Floor(this.Immunity * 100f) / 100f).ToStringPercent();
				}
				return null;
			}
		}

		
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x0005FADA File Offset: 0x0005DCDA
		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def);
			}
		}

		
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x0005FAF7 File Offset: 0x0005DCF7
		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1f;
			}
		}

		
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x0005FB09 File Offset: 0x0005DD09
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.FullyImmune)
				{
					return HediffComp_Immunizable.IconImmune;
				}
				return TextureAndColor.None;
			}
		}

		
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		
		protected override float SeverityChangePerDay()
		{
			if (!this.FullyImmune)
			{
				return this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor;
			}
			return this.Props.severityPerDayImmune;
		}

		
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (this.severityPerDayNotImmuneRandomFactor != 1f)
			{
				stringBuilder.AppendLine("severityPerDayNotImmuneRandomFactor: " + this.severityPerDayNotImmuneRandomFactor.ToString("0.##"));
			}
			if (!base.Pawn.Dead)
			{
				ImmunityRecord immunityRecord = base.Pawn.health.immunity.GetImmunityRecord(base.Def);
				if (immunityRecord != null)
				{
					stringBuilder.AppendLine("immunity change per day: " + (immunityRecord.ImmunityChangePerTick(base.Pawn, true, this.parent) * 60000f).ToString("F3"));
					stringBuilder.AppendLine("  pawn immunity gain speed: " + StatDefOf.ImmunityGainSpeed.ValueToString(base.Pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true), ToStringNumberSense.Absolute, true));
				}
			}
			return stringBuilder.ToString();
		}

		
		private float severityPerDayNotImmuneRandomFactor = 1f;

		
		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);
	}
}
