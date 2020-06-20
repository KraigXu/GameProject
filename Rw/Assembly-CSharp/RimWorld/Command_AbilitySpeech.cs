using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC4 RID: 2756
	public class Command_AbilitySpeech : Command_Ability
	{
		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004178 RID: 16760 RVA: 0x0015E0A8 File Offset: 0x0015C2A8
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

		// Token: 0x06004179 RID: 16761 RVA: 0x0015E171 File Offset: 0x0015C371
		public Command_AbilitySpeech(Ability ability) : base(ability)
		{
		}
	}
}
