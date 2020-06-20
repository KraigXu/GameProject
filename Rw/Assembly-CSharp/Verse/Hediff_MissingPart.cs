using System;
using System.Text;

namespace Verse
{
	// Token: 0x0200023A RID: 570
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0005C044 File Offset: 0x0005A244
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (!this.IsFreshNonSolidExtremity)
				{
					return 0f;
				}
				if (base.Part.def.tags.NullOrEmpty<BodyPartTagDef>() && base.Part.parts.NullOrEmpty<BodyPartRecord>() && !base.Bleeding)
				{
					return 0f;
				}
				return (float)base.Part.def.hitPoints / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0005C0BC File Offset: 0x0005A2BC
		public override string LabelBase
		{
			get
			{
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					return "RemovedBodyPart".Translate();
				}
				if (this.lastInjury == null || base.Part.depth == BodyPartDepth.Inside)
				{
					bool solid = base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs);
					return HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
				}
				if (base.Part.def.socketed && !this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty())
				{
					return this.lastInjury.injuryProps.destroyedOutLabel;
				}
				return this.lastInjury.injuryProps.destroyedLabel;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0005C194 File Offset: 0x0005A394
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.IsFreshNonSolidExtremity)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("FreshMissingBodyPart".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0005C1EC File Offset: 0x0005A3EC
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.bleedRate * base.Part.def.bleedRate;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0005C254 File Offset: 0x0005A454
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.causesNoPain || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.painPerSeverity;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0005C2B4 File Offset: 0x0005A4B4
		private bool ParentIsMissing
		{
			get
			{
				for (int i = 0; i < this.pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = this.pawn.health.hediffSet.hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0005C320 File Offset: 0x0005A520
		// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0005C335 File Offset: 0x0005A535
		public bool IsFresh
		{
			get
			{
				return this.isFreshInt && !this.TicksAfterNoLongerFreshPassed;
			}
			set
			{
				this.isFreshInt = value;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0005C340 File Offset: 0x0005A540
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x0005C3A4 File Offset: 0x0005A5A4
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0005C3B6 File Offset: 0x0005A5B6
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0005C3C0 File Offset: 0x0005A5C0
		public override void Tick()
		{
			bool ticksAfterNoLongerFreshPassed = this.TicksAfterNoLongerFreshPassed;
			base.Tick();
			bool ticksAfterNoLongerFreshPassed2 = this.TicksAfterNoLongerFreshPassed;
			if (ticksAfterNoLongerFreshPassed != ticksAfterNoLongerFreshPassed2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0005C3F4 File Offset: 0x0005A5F4
		public override void Tended(float quality, int batchPosition = 0)
		{
			base.Tended(quality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0005C418 File Offset: 0x0005A618
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState != ProgramState.Playing || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				this.IsFresh = false;
			}
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(this.def, this.pawn, null);
				hediff_MissingPart.IsFresh = false;
				hediff_MissingPart.lastInjury = this.lastInjury;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0005C4E0 File Offset: 0x0005A6E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_MissingPart has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04000BCB RID: 3019
		public HediffDef lastInjury;

		// Token: 0x04000BCC RID: 3020
		private bool isFreshInt;
	}
}
