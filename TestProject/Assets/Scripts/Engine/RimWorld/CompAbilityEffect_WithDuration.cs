using System;
using Verse;

namespace RimWorld
{
	
	public abstract class CompAbilityEffect_WithDuration : CompAbilityEffect
	{
		
		// (get) Token: 0x060041DA RID: 16858 RVA: 0x0015FD32 File Offset: 0x0015DF32
		public new CompProperties_AbilityEffectWithDuration Props
		{
			get
			{
				return (CompProperties_AbilityEffectWithDuration)this.props;
			}
		}

		
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
