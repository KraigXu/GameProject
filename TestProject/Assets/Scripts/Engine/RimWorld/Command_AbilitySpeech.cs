using System;
using Verse;

namespace RimWorld
{
	
	public class Command_AbilitySpeech : Command_Ability
	{
		
		
		public override string Tooltip
		{
			get
			{
				TaggedString taggedString = this.ability.def.LabelCap + "\n\n" + "AbilitySpeechTooltip".Translate(this.ability.pawn.Named("ORGANIZER")) + "\n";
				if (this.ability.CooldownTicksRemaining > 0)
				{
					taggedString += "\n" + "AbilitySpeechCooldown".Translate() + ": " + this.ability.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true);
				}
				taggedString += "\n" + GatheringWorker_Speech.OutcomeBreakdownForPawn(this.ability.pawn);
				return taggedString;
			}
		}

		
		public Command_AbilitySpeech(Ability ability) : base(ability)
		{
		}
	}
}
