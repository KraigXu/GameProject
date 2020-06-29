using System;
using Verse;

namespace RimWorld
{
	
	public abstract class CompAbilityEffect_WithDuration : CompAbilityEffect
	{
		
		
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
