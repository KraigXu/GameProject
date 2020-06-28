using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000239 RID: 569
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x0005BA98 File Offset: 0x00059C98
		public override int UIGroupKey
		{
			get
			{
				int num = base.UIGroupKey;
				if (this.IsTended())
				{
					num = Gen.HashCombineInt(num, 152235495);
				}
				return num;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0005BAC4 File Offset: 0x00059CC4
		public override string LabelBase
		{
			get
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					if (base.Part.def.delicate && !hediffComp_GetsPermanent.Props.instantlyPermanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.instantlyPermanentLabel;
					}
					if (!hediffComp_GetsPermanent.Props.permanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.permanentLabel;
					}
				}
				return base.LabelBase;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0005BB38 File Offset: 0x00059D38
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.sourceHediffDef.label);
				}
				else if (this.source != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.source.label);
					if (this.sourceBodyPartGroup != null)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(this.sourceBodyPartGroup.LabelShort);
					}
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent && hediffComp_GetsPermanent.PainCategory != PainCategory.Painless)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(("PainCategory_" + hediffComp_GetsPermanent.PainCategory.ToString()).Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0005BC3F File Offset: 0x00059E3F
		public override Color LabelColor
		{
			get
			{
				if (this.IsPermanent())
				{
					return Hediff_Injury.PermanentInjuryColor;
				}
				return Color.white;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x0005BC54 File Offset: 0x00059E54
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToString("F1");
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0005BC83 File Offset: 0x00059E83
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (this.IsPermanent() || !this.Visible)
				{
					return 0f;
				}
				return this.Severity / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0005BCB4 File Offset: 0x00059EB4
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part) || this.causesNoPain)
				{
					return 0f;
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					return this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.PainFactor;
				}
				return this.Severity * this.def.injuryProps.painPerSeverity;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0005BD44 File Offset: 0x00059F44
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.BleedingStoppedDueToAge)
				{
					return 0f;
				}
				if (base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) || this.IsTended() || this.IsPermanent())
				{
					return 0f;
				}
				if (this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part))
				{
					return 0f;
				}
				float num = this.Severity * this.def.injuryProps.bleedRate;
				if (base.Part != null)
				{
					num *= base.Part.def.bleedRate;
				}
				return num;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x0005BE0C File Offset: 0x0005A00C
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0005BE59 File Offset: 0x0005A059
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0005BE6C File Offset: 0x0005A06C
		public override void Tick()
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.Tick();
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0005BEA0 File Offset: 0x0005A0A0
		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostInjuryHeal(amount);
				}
			}
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0005BEFC File Offset: 0x0005A0FC
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0005BF6C File Offset: 0x0005A16C
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part != null && base.Part.coverageAbs <= 0f)
			{
				Log.Error(string.Concat(new object[]
				{
					"Added injury to ",
					base.Part.def,
					" but it should be impossible to hit it. pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					" dinfo=",
					dinfo.ToStringSafe<DamageInfo?>()
				}), false);
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0005BFE8 File Offset: 0x0005A1E8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04000BCA RID: 3018
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
	}
}
