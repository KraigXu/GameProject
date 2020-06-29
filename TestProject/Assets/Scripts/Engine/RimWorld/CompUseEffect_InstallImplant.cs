using System;
using Verse;

namespace RimWorld
{
	
	public class CompUseEffect_InstallImplant : CompUseEffect
	{
		
		
		public CompProperties_UseEffectInstallImplant Props
		{
			get
			{
				return (CompProperties_UseEffectInstallImplant)this.props;
			}
		}

		
		public override void DoEffect(Pawn user)
		{
			BodyPartRecord bodyPartRecord = user.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null);
			if (bodyPartRecord == null)
			{
				return;
			}
			Hediff firstHediffOfDef = user.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
			if (firstHediffOfDef == null)
			{
				user.health.AddHediff(this.Props.hediffDef, bodyPartRecord, null, null);
				return;
			}
			if (this.Props.canUpgrade)
			{
				((Hediff_ImplantWithLevel)firstHediffOfDef).ChangeLevel(1);
			}
		}

		
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if ((!p.IsFreeColonist || p.HasExtraHomeFaction(null)) && !this.Props.allowNonColonists)
			{
				failReason = "InstallImplantNotAllowedForNonColonists".Translate();
				return false;
			}
			if (p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null) == null)
			{
				failReason = "InstallImplantNoBodyPart".Translate() + ": " + this.Props.bodyPart.LabelShort;
				return false;
			}
			Hediff existingImplant = this.GetExistingImplant(p);
			if (existingImplant != null)
			{
				if (!this.Props.canUpgrade)
				{
					failReason = "InstallImplantAlreadyInstalled".Translate();
					return false;
				}
				Hediff_ImplantWithLevel hediff_ImplantWithLevel = (Hediff_ImplantWithLevel)existingImplant;
				if ((float)hediff_ImplantWithLevel.level >= hediff_ImplantWithLevel.def.maxSeverity)
				{
					failReason = "InstallImplantAlreadyMaxLevel".Translate();
					return false;
				}
			}
			failReason = null;
			return true;
		}

		
		public Hediff GetExistingImplant(Pawn p)
		{
			for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
			{
				Hediff hediff = p.health.hediffSet.hediffs[i];
				if (hediff.def == this.Props.hediffDef && hediff.Part == p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null))
				{
					return hediff;
				}
			}
			return null;
		}
	}
}
