using System;

namespace Verse
{
	
	public class HediffComp_GetsPermanent : HediffComp
	{
		
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x0005DEBB File Offset: 0x0005C0BB
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0005DEC8 File Offset: 0x0005C0C8
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x0005DED0 File Offset: 0x0005C0D0
		public bool IsPermanent
		{
			get
			{
				return this.isPermanentInt;
			}
			set
			{
				if (value == this.isPermanentInt)
				{
					return;
				}
				this.isPermanentInt = value;
				if (this.isPermanentInt)
				{
					this.painCategory = HealthTuning.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted cat) => cat.weight).category;
					this.permanentDamageThreshold = 9999f;
				}
			}
		}

		
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0005DF35 File Offset: 0x0005C135
		public PainCategory PainCategory
		{
			get
			{
				return this.painCategory;
			}
		}

		
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x0005DF3D File Offset: 0x0005C13D
		public float PainFactor
		{
			get
			{
				return (float)this.painCategory;
			}
		}

		
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isPermanentInt, "isPermanent", false, false);
			Scribe_Values.Look<float>(ref this.permanentDamageThreshold, "permanentDamageThreshold", 9999f, false);
			Scribe_Values.Look<PainCategory>(ref this.painCategory, "painCategory", PainCategory.Painless, false);
			BackCompatibility.PostExposeData(this);
		}

		
		public void PreFinalizeInjury()
		{
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				return;
			}
			float num = 0.02f * this.parent.Part.def.permanentInjuryChanceFactor * this.Props.becomePermanentChanceFactor;
			if (!this.parent.Part.def.delicate)
			{
				num *= HealthTuning.BecomePermanentChanceFactorBySeverityCurve.Evaluate(this.parent.Severity);
			}
			if (Rand.Chance(num))
			{
				if (this.parent.Part.def.delicate)
				{
					this.IsPermanent = true;
					return;
				}
				this.permanentDamageThreshold = Rand.Range(1f, this.parent.Severity / 2f);
			}
		}

		
		public override void CompPostInjuryHeal(float amount)
		{
			if (this.permanentDamageThreshold >= 9999f || this.IsPermanent)
			{
				return;
			}
			if (this.parent.Severity <= this.permanentDamageThreshold && this.parent.Severity >= this.permanentDamageThreshold - amount)
			{
				this.parent.Severity = this.permanentDamageThreshold;
				this.IsPermanent = true;
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		
		public override string CompDebugString()
		{
			return string.Concat(new object[]
			{
				"isPermanent: ",
				this.isPermanentInt.ToString(),
				"\npermanentDamageThreshold: ",
				this.permanentDamageThreshold,
				"\npainCategory: ",
				this.painCategory
			});
		}

		
		public float permanentDamageThreshold = 9999f;

		
		public bool isPermanentInt;

		
		private PainCategory painCategory;

		
		private const float NonActivePermanentDamageThresholdValue = 9999f;
	}
}
