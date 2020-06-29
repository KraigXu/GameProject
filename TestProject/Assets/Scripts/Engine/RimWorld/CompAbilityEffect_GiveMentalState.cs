using System;
using Verse;

namespace RimWorld
{
	
	public class CompAbilityEffect_GiveMentalState : CompAbilityEffect
	{
		
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x0015EC2F File Offset: 0x0015CE2F
		public new CompProperties_AbilityGiveMentalState Props
		{
			get
			{
				return (CompProperties_AbilityGiveMentalState)this.props;
			}
		}

		
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null && !pawn.InMentalState && pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.stateDef, null, true, false, null, false))
			{
				float num = this.parent.def.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 10f);
				if (this.Props.durationMultiplier != null)
				{
					num *= pawn.GetStatValue(this.Props.durationMultiplier, true);
				}
				pawn.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = num.SecondsToTicks();
			}
		}

		
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null && pawn.InMentalState)
			{
				if (throwMessages)
				{
					Messages.Message("AbilityCantApplyToMentallyBroken".Translate(pawn.LabelShort), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
