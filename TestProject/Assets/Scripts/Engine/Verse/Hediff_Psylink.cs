using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	
	public class Hediff_Psylink : Hediff_ImplantWithLevel
	{
		
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.TryGiveAbilityOfLevel(this.level);
			Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.Notify_GainedPsylink();
		}

		
		public override void ChangeLevel(int levelOffset)
		{
			if (levelOffset > 0)
			{
				float num = Math.Min((float)levelOffset, this.def.maxSeverity - (float)this.level);
				int num2 = 0;
				while ((float)num2 < num)
				{
					int abilityLevel = this.level + 1 + num2;
					this.TryGiveAbilityOfLevel(abilityLevel);
					Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
					if (psychicEntropy != null)
					{
						psychicEntropy.Notify_GainedPsylink();
					}
					num2++;
				}
			}
			base.ChangeLevel(levelOffset);
		}

		
		public void TryGiveAbilityOfLevel(int abilityLevel)
		{
			string str = "LetterLabelPsylinkLevelGained".Translate() + ": " + this.pawn.LabelShortCap;
			string text = ((abilityLevel == 1) ? "LetterPsylinkLevelGained_First" : "LetterPsylinkLevelGained_NotFirst").Translate(this.pawn.Named("USER"));
			if (!this.pawn.abilities.abilities.Any((Ability a) => a.def.level == abilityLevel))
			{
				AbilityDef abilityDef = (from a in DefDatabase<AbilityDef>.AllDefs
				where a.level == abilityLevel
				select a).RandomElement<AbilityDef>();
				this.pawn.abilities.GainAbility(abilityDef);
				text += "\n\n" + "LetterPsylinkLevelGained_PsycastLearned".Translate(this.pawn.Named("USER"), abilityLevel, abilityDef.LabelCap);
			}
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
			}
		}

		
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs == null)
			{
				return;
			}
			needs.AddOrRemoveNeedsAsAppropriate();
		}
	}
}
