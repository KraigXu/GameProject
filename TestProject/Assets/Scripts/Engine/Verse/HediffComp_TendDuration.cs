using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000272 RID: 626
	[StaticConstructorOnStartup]
	public class HediffComp_TendDuration : HediffComp_SeverityPerDay
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0005F1D1 File Offset: 0x0005D3D1
		public HediffCompProperties_TendDuration TProps
		{
			get
			{
				return (HediffCompProperties_TendDuration)this.props;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0005F1DE File Offset: 0x0005D3DE
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.TProps.disappearsAtTotalTendQuality >= 0 && this.totalTendQuality >= (float)this.TProps.disappearsAtTotalTendQuality);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0005F211 File Offset: 0x0005D411
		public bool IsTended
		{
			get
			{
				return Current.ProgramState == ProgramState.Playing && this.tendTicksLeft > 0;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x0005F226 File Offset: 0x0005D426
		public bool AllowTend
		{
			get
			{
				if (this.TProps.TendIsPermanent)
				{
					return !this.IsTended;
				}
				return this.TProps.TendTicksOverlap > this.tendTicksLeft;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0005F254 File Offset: 0x0005D454
		public override string CompTipStringExtra
		{
			get
			{
				if (this.parent.IsPermanent())
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (!this.IsTended)
				{
					if (!base.Pawn.Dead && this.parent.TendableNow(false))
					{
						stringBuilder.AppendLine("NeedsTendingNow".Translate());
					}
				}
				else
				{
					if (this.TProps.showTendQuality)
					{
						string text;
						if (this.parent.Part != null && this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
						{
							text = this.TProps.labelSolidTendedWell;
						}
						else if (this.parent.Part != null && this.parent.Part.depth == BodyPartDepth.Inside)
						{
							text = this.TProps.labelTendedWellInner;
						}
						else
						{
							text = this.TProps.labelTendedWell;
						}
						if (text != null)
						{
							stringBuilder.AppendLine(text.CapitalizeFirst() + " (" + "Quality".Translate().ToLower() + " " + this.tendQuality.ToStringPercent("F0") + ")");
						}
						else
						{
							stringBuilder.AppendLine(string.Format("{0}: {1}", "TendQuality".Translate(), this.tendQuality.ToStringPercent()));
						}
					}
					if (!base.Pawn.Dead && !this.TProps.TendIsPermanent && this.parent.TendableNow(true))
					{
						int num = this.tendTicksLeft - this.TProps.TendTicksOverlap;
						if (num < 0)
						{
							stringBuilder.AppendLine("CanTendNow".Translate());
						}
						else if ("NextTendIn".CanTranslate())
						{
							stringBuilder.AppendLine("NextTendIn".Translate(num.ToStringTicksToPeriod(true, false, true, true)));
						}
						else
						{
							stringBuilder.AppendLine("NextTreatmentIn".Translate(num.ToStringTicksToPeriod(true, false, true, true)));
						}
						stringBuilder.AppendLine("TreatmentExpiresIn".Translate(this.tendTicksLeft.ToStringTicksToPeriod(true, false, true, true)));
					}
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x0005F4D4 File Offset: 0x0005D6D4
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.parent is Hediff_Injury)
				{
					if (this.IsTended && !this.parent.IsPermanent())
					{
						Color color = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_Injury, color);
					}
				}
				else if (!(this.parent is Hediff_MissingPart) && !this.parent.FullyImmune())
				{
					if (this.IsTended)
					{
						Color color2 = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_General, color2);
					}
					return HediffComp_TendDuration.TendedIcon_Need_General;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0005F586 File Offset: 0x0005D786
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.tendTicksLeft, "tendTicksLeft", -1, false);
			Scribe_Values.Look<float>(ref this.tendQuality, "tendQuality", 0f, false);
			Scribe_Values.Look<float>(ref this.totalTendQuality, "totalTendQuality", 0f, false);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0005F5C6 File Offset: 0x0005D7C6
		protected override float SeverityChangePerDay()
		{
			if (this.IsTended)
			{
				return this.TProps.severityPerDayTended * this.tendQuality;
			}
			return 0f;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0005F5E8 File Offset: 0x0005D7E8
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.tendTicksLeft > 0 && !this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft--;
			}
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0005F618 File Offset: 0x0005D818
		public override void CompTended(float quality, int batchPosition = 0)
		{
			this.tendQuality = Mathf.Clamp01(quality + Rand.Range(-0.25f, 0.25f));
			this.totalTendQuality += this.tendQuality;
			if (this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft = 1;
			}
			else
			{
				this.tendTicksLeft = Mathf.Max(0, this.tendTicksLeft) + this.TProps.TendTicksFull;
			}
			if (batchPosition == 0 && base.Pawn.Spawned)
			{
				string text = "TextMote_Tended".Translate(this.parent.Label).CapitalizeFirst() + "\n" + "Quality".Translate() + " " + this.tendQuality.ToStringPercent();
				MoteMaker.ThrowText(base.Pawn.DrawPos, base.Pawn.Map, text, Color.white, 3.65f);
			}
			base.Pawn.health.Notify_HediffChanged(this.parent);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0005F738 File Offset: 0x0005D938
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsTended)
			{
				stringBuilder.AppendLine("tendQuality: " + this.tendQuality.ToStringPercent());
				if (!this.TProps.TendIsPermanent)
				{
					stringBuilder.AppendLine("tendTicksLeft: " + this.tendTicksLeft);
				}
			}
			else
			{
				stringBuilder.AppendLine("untended");
			}
			stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString());
			if (this.TProps.disappearsAtTotalTendQuality >= 0)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"totalTendQuality: ",
					this.totalTendQuality.ToString("F2"),
					" / ",
					this.TProps.disappearsAtTotalTendQuality
				}));
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x04000C35 RID: 3125
		public int tendTicksLeft = -1;

		// Token: 0x04000C36 RID: 3126
		public float tendQuality;

		// Token: 0x04000C37 RID: 3127
		private float totalTendQuality;

		// Token: 0x04000C38 RID: 3128
		public const float TendQualityRandomVariance = 0.25f;

		// Token: 0x04000C39 RID: 3129
		private static readonly Color UntendedColor = new ColorInt(116, 101, 72).ToColor;

		// Token: 0x04000C3A RID: 3130
		private static readonly Texture2D TendedIcon_Need_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedNeed", true);

		// Token: 0x04000C3B RID: 3131
		private static readonly Texture2D TendedIcon_Well_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedWell", true);

		// Token: 0x04000C3C RID: 3132
		private static readonly Texture2D TendedIcon_Well_Injury = ContentFinder<Texture2D>.Get("UI/Icons/Medical/BandageWell", true);
	}
}
