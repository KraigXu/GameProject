using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000252 RID: 594
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0005DC99 File Offset: 0x0005BE99
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x0005DCA6 File Offset: 0x0005BEA6
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0005DCC0 File Offset: 0x0005BEC0
		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(this.Props.chemical.label, this.CurrentFactor.ToStringPercent()).CapitalizeFirst();
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0005DD09 File Offset: 0x0005BF09
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x04000BF5 RID: 3061
		private static readonly SimpleCurve EffectFactorSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};
	}
}
