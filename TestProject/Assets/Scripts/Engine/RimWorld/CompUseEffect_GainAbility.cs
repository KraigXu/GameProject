using System;
using Verse;

namespace RimWorld
{
	
	public class CompUseEffect_GainAbility : CompUseEffect
	{
		
		
		private AbilityDef Ability
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().ability;
			}
		}

		
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			AbilityDef ability = this.Ability;
			user.abilities.GainAbility(ability);
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("AbilityNeurotrainerUsed".Translate(user.Named("USER"), ability.LabelCap), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if (!p.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false))
			{
				failReason = "PsycastNeurotrainerNoPsylink".TranslateWithBackup("PsycastNeurotrainerNoPsychicAmplifier");
				return false;
			}
			if (p.abilities != null && p.abilities.abilities.Any((Ability a) => a.def == this.Ability))
			{
				failReason = "PsycastNeurotrainerAbilityAlreadyLearned".Translate(p.Named("USER"), this.Ability.LabelCap);
				return false;
			}
			return base.CanBeUsedBy(p, out failReason);
		}

		
		public override TaggedString ConfirmMessage(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
			if (firstHediffOfDef == null)
			{
				return null;
			}
			if (this.Ability.level > ((Hediff_ImplantWithLevel)firstHediffOfDef).level)
			{
				return "PsylinkTooLowForGainAbility".Translate(p.Named("PAWN"), this.Ability.label.Named("ABILITY"));
			}
			return null;
		}
	}
}
