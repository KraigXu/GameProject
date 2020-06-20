using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE3 RID: 2787
	public abstract class CompAbilityEffect_WithDuration : CompAbilityEffect
	{
		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x060041DA RID: 16858 RVA: 0x0015FD32 File Offset: 0x0015DF32
		public new CompProperties_AbilityEffectWithDuration Props
		{
			get
			{
				return (CompProperties_AbilityEffectWithDuration)this.props;
			}
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x0015FD40 File Offset: 0x0015DF40
		public float GetDurationSeconds(Pawn target)
		{
			float num = this.parent.def.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 10f);
			if (this.Props.durationMultiplier != null)
			{
				num *= target.GetStatValue(this.Props.durationMultiplier, true);
			}
			return num;
		}
	}
}
