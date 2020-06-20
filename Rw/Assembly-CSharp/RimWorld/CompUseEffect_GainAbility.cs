using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA1 RID: 3489
	public class CompUseEffect_GainAbility : CompUseEffect
	{
		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x060054CB RID: 21707 RVA: 0x001C4344 File Offset: 0x001C2544
		private AbilityDef Ability
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().ability;
			}
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x001C4358 File Offset: 0x001C2558
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

		// Token: 0x060054CD RID: 21709 RVA: 0x001C43C0 File Offset: 0x001C25C0
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

		// Token: 0x060054CE RID: 21710 RVA: 0x001C4460 File Offset: 0x001C2660
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
